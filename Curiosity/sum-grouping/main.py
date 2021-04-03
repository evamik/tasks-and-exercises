from pandas.core.frame import DataFrame
from classes.validator import Validator
from pathlib import Path
import pandas as pd


def read_from_file(filename, separator=";", verbose=True) -> pd.DataFrame:
    df = pd.DataFrame()
    try:
        df = pd.read_csv(
            Path(__file__).resolve().parent.__str__()+"\\"+filename, separator)
    except Exception:
        print("Failed to read data from file") if verbose == True else None
    return df


def case_one(df: DataFrame) -> DataFrame:
    # Calculate the sum by grouping "Darbuotojas"
    df = df.groupby("Darbuotojas").sum().reset_index().rename(
        columns={"Alga": "Suma"})

    # Calculate "Mokesciai" from "Suma" and add to dataframe
    df["Mokesciai"] = df.apply(lambda row: row["Suma"] * 0.4, axis=1)

    return df


def case_two(df: DataFrame) -> DataFrame:
    # Calculate the sum by grouping "Darbuotojas" and "Tipas"
    df = df.groupby(["Darbuotojas", "Tipas"]).sum().reset_index().rename(
        columns={"Alga": "Suma"})

    return df


def execute_task(inputfile, input_separator, outputfile, output_separator, case):
    print("\nExecuting task...")

    # Read dataframe from inputfile
    df = read_from_file(inputfile, input_separator)

    # Check if dataframe is valid for case manipulation
    if not Validator.dataframe_is_valid(df):
        print(f"{inputfile} data is invalid")
        return

    # Apply dataframe manilupation by case
    df = case(df)

    # Save dataframe to outputfile
    print("Saving results...\n")
    df.to_csv(outputfile, sep=output_separator, index=False)


if __name__ == "__main__":
    # first task
    execute_task("duomenys.csv", ",", "rezultatai1.csv", ";", case_one)

    # second task
    execute_task("duomenys.csv", ",", "rezultatai2.csv", ";", case_two)
