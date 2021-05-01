from pathlib import Path
import pandas as pd
from random import seed
from random import random
import math
import seaborn as sb
from matplotlib import pyplot as plt
sb.set_theme()


def read_people_from_file(filename) -> pd.DataFrame:
    try:
        df = pd.DataFrame()
        df = pd.read_csv(
            Path(__file__).resolve().parent.__str__()+"\\"+filename, ";")
        return df
    except Exception:
        print("Failed to read data from file")


def delete_random_data(filename):
    df = pd.read_csv(
        Path(__file__).resolve().parent.__str__()+"\\"+filename, ";")

    seed(1)
    for i in range(len(df)):
        for j in range(len(df.iloc[i])):
            if random() < 0.01:
                df.iat[i, j] = None
    df.to_csv(Path(__file__).resolve().parent.__str__() +
              "\\"+filename, ";", index=False)


if __name__ == "__main__":
    # delete_random_data("cardio_train.csv")

    df = read_people_from_file("cardio_train.csv")

    print("Total numerical values = %d" %
          (len(df) * len(df.select_dtypes(include=['float64']).columns)))

    missings = pd.DataFrame()
    cardinalities = pd.DataFrame()
    minimums = df.min()
    maximums = df.max()
    means = df.mean()
    medians = df.median()
    q1_quantiles = df.quantile(0.25)
    q3_quantiles = df.quantile(0.75)
    modes = pd.DataFrame()

    for col in df.columns:
        missings = missings.append(pd.DataFrame({
            "col": [col],
            "val": [len(df[df[col].isna()]) / len(df) * 100]
        }))

        cardinalities = cardinalities.append(pd.DataFrame({
            "col": [col],
            "val": [len(df[df[col].notna()][col].unique())]
        }))

        if df[col].dtype != "float64":
            mode = df[col].value_counts()
            modes = modes.append(pd.DataFrame({
                "col": [col],
                "val": [mode.axes[0][0]],
                "count": [mode.iat[0]],
                "percent": [mode.iat[0] / len(df) * 100],
                "val2": [mode.axes[0][1]],
                "count2": [mode.iat[1]],
                "percent2": [mode.iat[1] / len(df) * 100]
            }))

    print("\nmissings:\n", missings)
    print("\ncardinalities:\n", cardinalities)
    print("\nminimums:\n", minimums)
    print("\nmaximums:\n", maximums)
    print("\nmeans:\n", means)
    print("\nmedians:\n", medians)
    print("\nq1_quantiles:\n", q1_quantiles)
    print("\nq3_quantiles:\n", q3_quantiles)
    print("\nstandard deviation:\n", df.std())
    print("\nTotal categorical values = %d" %
          (len(df) * len(df.select_dtypes(exclude=['float64']).columns)))
    print("\nModes:\n", modes)

    df = df.dropna()
    bins = round(1 + 3.22 * math.log(len(df)))
    # for col in df.columns:
    # sb.histplot(data=df[col], bins=bins)
    # plt.show()

    for col in df.columns:
        if df[col].dtype == "float64":
            low = q1_quantiles.loc[col]
            high = q3_quantiles.loc[col]
            val = (high - low)*2
            df = df[df[col] > low - val]
            df = df[df[col] < high + val]
    #         sb.histplot(data=df[col], bins=bins)
    #         plt.show()

    # sb.pairplot(data=df, x_vars=["weight"], y_vars=["height"], kind="hist")
    # plt.show()
    # sb.pairplot(data=df, x_vars=["weight"], y_vars=["ap_hi"], kind="hist")
    # plt.show()
    # sb.pairplot(data=df, x_vars=["ap_hi"], y_vars=["ap_lo"], kind="hist")
    # plt.show()

    # sb.pairplot(data=df, kind="hist", corner=True)
    # plt.show()

    # sb.countplot(data=df, x="cholesterol")
    # plt.show()
    # sb.catplot(data=df, x="cholesterol", col="gluc", kind="count")
    # plt.show()

    # sb.countplot(data=df, x="smoke")
    # plt.show()
    # sb.catplot(data=df, x="smoke", col="alco", kind="count")
    # plt.show()

    # sb.catplot(data=df, x="ap_hi", col="cardio", kind="count")
    # plt.show()

    # sb.catplot(data=df, x="ap_lo", col="cardio", kind="count")
    # plt.show()

    # sb.heatmap(df.select_dtypes(include=['float64']).cov(), annot=True)
    # plt.show()

    # sb.heatmap(df.select_dtypes(include=['float64']).corr(), annot=True)
    # plt.show()

    ndf = df.select_dtypes(include="float64")
    df = pd.concat(
        [(ndf-ndf.min())/(ndf.max()-ndf.min()),
         df.select_dtypes(exclude="float64")], axis=1)
    ndf = None

    df = df.apply(lambda x: pd.factorize(x)[0] if x.dtype != "float64" else x)

    print(df)
    df.to_csv(Path(__file__).resolve().parent.__str__() +
              "\\"+"res.csv", ";", index=False)
