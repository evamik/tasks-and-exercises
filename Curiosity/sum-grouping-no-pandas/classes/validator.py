from classes.dataframe import DataFrame


class Validator:
    @staticmethod
    def dataframe_is_valid(df: DataFrame):
        if not set(["Darbuotojas", "Tipas", "Alga"]).issubset(df.columns):
            return False

        if df.dtypes["Alga"] not in ["int64", "float64"]:
            return False

        return True
