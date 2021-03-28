from typing import List
import pandas as pd
from pathlib import Path
from dataclasses import dataclass
from matplotlib import pyplot as plt
import numpy as np


def read_file(filename) -> pd.DataFrame:
    try:
        df = pd.read_csv(
            Path(__file__).resolve().parent.__str__()+"\\"+filename, ";")
        return df
    except Exception:
        print("Failed to read data from file")


@dataclass
class Trapezoid:
    name: str
    a: float
    b: float
    c: float
    d: float

    def to_array(self):
        return [self.a, self.b, self.c, self.d]


class FuzzyVar:
    def __init__(self, name):
        self.name = name
        self.trapezoids = dict()  # type: dict[str, Trapezoid]

    def add_fuzzy(self, trapezoid: Trapezoid):
        self.trapezoids[trapezoid.name] = trapezoid

    def plot(self):
        for t in self.trapezoids.values():
            plt.text(t.b, 1, t.name, size=15)
            plt.plot(t.to_array(), [0, 1, 1, 0])
        plt.xlim(left=0, right=1)
        plt.title(self.name)
        plt.show()


class Rule:
    def __init__(self, func, t):
        self.value = func
        self.trapezoid = t  # type: Trapezoid


class RulesService:
    def __init__(self, rules):
        self.rules = rules  # type: List[Rule]

    def get_output(self, x, y, z):
        outputs = []
        for rule in self.rules:
            outputs.append({"value": rule.value(x, y, z),
                            "name": rule.trapezoid.name})

        df = pd.DataFrame(outputs).groupby("name")['value'].max()

        return df


def f(x, t):
    if x < t.a:
        return 0
    if t.a <= x < t.b:
        return (x - t.a) / (t.b - t.a)
    if t.b <= x < t.c:
        return 1
    if t.c <= x < t.d:
        return (t.d - x) / (t.d - t.c)
    else:
        return 0


if __name__ == "__main__":
    df = read_file("csgo.csv")

    myAccuracy = FuzzyVar(df.columns[0])
    enemyAccuracy = FuzzyVar(df.columns[1])
    myVisibility = FuzzyVar(df.columns[2])

    inputs = [
        myAccuracy,
        enemyAccuracy,
        myVisibility
    ]

    myAccuracy.add_fuzzy(Trapezoid("bad", -0.01, 0, 0.2, 0.4))
    myAccuracy.add_fuzzy(Trapezoid("average", 0.25, 0.45, 0.55, 0.75))
    myAccuracy.add_fuzzy(Trapezoid("pro", 0.65, 0.85, 1, 1.01))

    # myAccuracy.plot()

    enemyAccuracy.add_fuzzy(Trapezoid("bad", -0.01, 0, 0.2, 0.4))
    enemyAccuracy.add_fuzzy(Trapezoid("average", 0.2, 0.55, 0.55, 0.8))
    enemyAccuracy.add_fuzzy(Trapezoid("pro", 0.75, 0.85, 1, 1.01))

    # enemyAccuracy.plot()

    myVisibility.add_fuzzy(Trapezoid("invisible", -0.01, 0, 0.05, 0.4))
    myVisibility.add_fuzzy(Trapezoid("slightly visible", 0.20, 0.4, 0.4, 0.55))
    myVisibility.add_fuzzy(Trapezoid("visible", 0.5, 0.85, 1, 1.01))

    # myVisibility.plot()

    rules = []  # type: List[Rule]
    output = FuzzyVar("winChance")
    output.add_fuzzy(Trapezoid("uninstall", -0.01, 0, 0, 0.5))
    output.add_fuzzy(Trapezoid("50/50", 0.25, 0.5, 0.5, 0.75))
    output.add_fuzzy(Trapezoid("ez", 0.65, 0.8, 1, 1.01))

    # output.plot()

    rules.append(Rule(lambda x, y, z:
                      min(1 - f(x, myAccuracy.trapezoids["bad"]), f(
                          y, enemyAccuracy.trapezoids["bad"])), output.trapezoids["ez"]))  # !(myAccuracy = bad) AND (enemyAccuracy = bad) THEN ez

    rules.append(Rule(lambda x, y, z:
                      min(f(z, myVisibility.trapezoids["invisible"]), 1 - f(
                          y, enemyAccuracy.trapezoids["pro"])), output.trapezoids["ez"]))  # (myVisibility = invisible) AND !(enemyAccuracy = pro) THEN ez

    rules.append(Rule(lambda x, y, z:
                      min(1 - f(z, myVisibility.trapezoids["visible"]), f(
                          x, myAccuracy.trapezoids["pro"])), output.trapezoids["ez"]))  # !(myVisibility = visible) AND (myAccuracy = pro) THEN ez

    rules.append(Rule(lambda x, y, z:
                      max(f(z, myVisibility.trapezoids["invisible"]), f(
                          x, myAccuracy.trapezoids["pro"])), output.trapezoids["ez"]))  # (myVisibility = invisible) OR (myAccuracy = pro) THEN ez

    rules.append(Rule(lambda x, y, z:
                      f(z, myVisibility.trapezoids["visible"]), output.trapezoids["50/50"]))  # (myVisibility = visible) THEN 50/50

    rules.append(Rule(lambda x, y, z:
                      min(min(f(x, myAccuracy.trapezoids["bad"]), f(
                          y, enemyAccuracy.trapezoids["pro"])),
                          1 - f(z, myVisibility.trapezoids["visible"])), output.trapezoids["50/50"]))  # (myAccuracy = bad) AND (enemyAccuracy = pro) AND !(myVisibility = visible) THEN 50/50

    rules.append(Rule(lambda x, y, z:
                      min(1 - f(y, enemyAccuracy.trapezoids["bad"]), f(
                          z, myVisibility.trapezoids["slightly visible"])), output.trapezoids["50/50"]))  # !(enemyAccuracy = bad) AND (myVisibility = slightly visible) THEN 50/50

    rules.append(Rule(lambda x, y, z:
                      min(f(x, myAccuracy.trapezoids["average"]), f(
                          y, enemyAccuracy.trapezoids["average"])), output.trapezoids["50/50"]))  # (myAccuracy = average) AND (enemyAccuracy = average) THEN 50/50

    rules.append(Rule(lambda x, y, z:
                      min(f(x, myAccuracy.trapezoids["bad"]), 1 - f(
                          y, enemyAccuracy.trapezoids["bad"])), output.trapezoids["uninstall"]))  # (myAccuracy = bad) AND !(enemyAccuracy = bad) THEN uninstall

    rules.append(Rule(lambda x, y, z:
                      min(min(f(z, myAccuracy.trapezoids["pro"]), 1 - f(
                          y, enemyAccuracy.trapezoids["pro"])),
                          f(x, myAccuracy.trapezoids["average"])), output.trapezoids["uninstall"]))  # (myVisibility = visible) AND (enemyAccuracy = pro) AND (myAccuracy = average) THEN uninstall

    rulesService = RulesService(rules)

    for i in range(0, 3):
        print(df.iloc[i])
        out = rulesService.get_output(
            df.loc[i][0], df.loc[i][1], df.loc[i][2])
        X = []
        Y = []
        for name in output.trapezoids.keys():
            if(out[name] > 0):
                x = output.trapezoids[name].to_array()
                x[1] = x[0] + (x[1]-x[0])*out[name]
                x[2] = x[3] - (x[3]-x[2])*out[name]
                Y_temp = [0, out[name], out[name], 0]
                X += x
                Y += Y_temp

        sum1 = 0
        sum2 = 0
        XX = np.arange(X[0], X[-1], 1/1000)
        for j in range(0, len(XX)):
            max_f = 0
            for t in output.trapezoids.values():
                max_f = max(max_f, min(f(XX[j], t), out[t.name]))
            sum1 += XX[j]*max_f
            sum2 += max_f
        Centroid = sum1/sum2
        print("Centroid:", Centroid)
        plt.plot([Centroid, Centroid], [0, 1], "r",
                 linewidth=2, label="Centroid")

        max_val = -1
        x_sum = Centroid
        count = 1

        for j in range(0, len(Y)):
            if Y[j] > max_val:
                max_val = Y[j]
                x_sum = X[j]
                count = 1
            elif Y[j] == max_val:
                count += 1
                x_sum += X[j]
        MOM = x_sum/count

        print("MOM:", MOM)
        plt.plot([MOM, MOM], [0, 1], "y", linewidth=2, label="MOM")
        plt.legend()

        plt.fill_between(X, Y)
        output.plot()
