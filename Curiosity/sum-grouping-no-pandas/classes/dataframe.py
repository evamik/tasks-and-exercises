import numpy as np


class DataFrame:
    def __init__(self, data=None, sep=","):

        self.empty = False
        if data is None or not data:
            self.empty = True
            return

        self.data = data
        self.separator = sep
        self.columns = list(data.keys())

        self.dtypes = {}
        for col in self.columns:
            try:
                np.asarray_chkfinite(self.data[col], dtype=float)
                self.dtypes[col] = "float64"
            except Exception:
                self.dtypes[col] = "object"

    @staticmethod
    def read_csv(filepath, separator) -> "DataFrame":
        data = {}
        columns = []
        with open(filepath, "r", encoding="utf-8-sig") as file:
            while True:
                line = file.readline()

                if not line:
                    break

                line = line[0:-1]

                values = line.split(separator)

                if not data:
                    for value in values:
                        data[value] = []
                    columns = list(data.keys())
                else:
                    for i in range(len(values)):
                        data[columns[i]].append(values[i])

        return DataFrame(data, sep=separator)

    def __len__(self) -> int:
        return len(self.data[self.columns[0]])

    def __str__(self) -> str:
        string = ""

        if self.empty:
            return "<empty DataFrame>"

        # table header
        for col in self.columns:
            string += f"{col}{self.separator}"
        string = f"{string[0:-1]}\n"

        # table body
        for i in range(len(self.data[self.columns[0]])):
            row = ""
            for j in range(len(self.columns)):
                row += f"{self.data[self.columns[j]][i]}{self.separator}"
            row = f"{row[0:-1]}\n"
            string += row

        return string

    def __getitem__(self, key) -> list:
        return self.data[key]

    def __setitem__(self, key, value) -> None:
        if len(self) != len(value):
            raise Exception(f"Value length does not match DataFrame length")
        self.data[key] = value
        self.columns.append(key)

    def groupby(self, columns) -> "GroupedDataFrame":
        return GroupedDataFrame(self, columns)

    def apply(self, func, axis=0) -> list:
        results = []

        # Every column
        if axis == 0:
            raise Exception(
                "axis = 0 is not implemented, please only use axis = 1")

        # Every row
        if axis == 1:
            for i in range(len(self)):
                row = {}
                for col in self.columns:
                    row[col] = self.data[col][i]
                results.append(func(row))

        return results

    def rename(self, columns) -> "DataFrame":
        if columns is None or not columns:
            return self

        for col in list(columns.keys()):
            if col not in self.columns:
                raise Exception(f"{col} is not in DataFrame")
            self.data[columns[col]] = self.data.pop(col)
            self.columns[self.columns.index(col)] = columns[col]

        return self

    def to_csv(self, filepath, sep=",", index=True) -> None:
        with open(filepath, "w", encoding="utf-8-sig") as file:
            if index == True:
                raise Exception(
                    "index=True is not implemented, please only use index=False")
            self.separator = sep
            file.write(self.__str__())


class GroupedDataFrame:
    def __init__(self, df: "DataFrame", columns):
        self.df = df

        if not isinstance(columns, list):
            columns = [columns]

        col_diff = set(columns + df.columns) - set(df.columns)
        if len(col_diff) != 0:
            raise Exception(f"{col_diff} does not exist in DataFrame")

        data = {}
        other_columns = list(set(df.columns) - set(columns))

        for i in range(len(df.data[columns[0]])):
            group = ""
            values = {}
            for col in columns:
                group += f"{self.df.data[col][i]};"
            group = group[0:-1]
            for col in other_columns:
                values[col] = self.df.data[col][i]

            if group not in data:
                data[group] = [values]
            else:
                data[group] += [values]

        data = sorted(data.items())

        columns_str = ""
        for col in columns:
            columns_str += f"{col};"
        for col in other_columns:
            columns_str += f"{col};"
        columns_str = columns_str[0:-1]

        self.columns = columns_str
        self.groupedby = columns
        self.other_columns = other_columns
        self.data = data

    def sum(self) -> DataFrame:
        sum_cols = []
        for col in self.other_columns:
            if self.df.dtypes[col] in ["int64", "float64"]:
                sum_cols.append(col)
        for i in range(len(self.data)):
            item = self.data[i]
            row = list(item)

            groups_values = row[0].split(";")
            values = row[1]
            sums = {}
            for col in sum_cols:
                sums[col] = 0
            for value in values:
                for col in sum_cols:
                    sums[col] += float(value[col])

            row = groups_values + list(sums.values())
            self.data[i] = row

        data = {}
        self.data = np.array(self.data).transpose()
        index = 0
        cols = self.groupedby + sum_cols
        for col in cols:
            data[col] = self.data[index]
            if col in sum_cols:
                data[col] = np.asfarray(data[col])
            index += 1

        return DataFrame(data)
