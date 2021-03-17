from typing import List
import pandas as pd
from pathlib import Path
from dataclasses import dataclass
from matplotlib import pyplot as plt


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
        self.trapezoids = []  # type: List[Trapezoid]

    def add_fuzzy(self, trapezoid):
        self.trapezoids.append(trapezoid)

    def plot(self):
        for t in self.trapezoids:
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
            outputs.append({rule.value(x, y, z),
                            rule.trapezoid.name})
        return outputs


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
                      min(1 - f(x, myAccuracy.trapezoids[0]), f(
                          y, enemyAccuracy.trapezoids[0])), output.trapezoids[2]))  # !(myAccuracy = bad) AND (enemyAccuracy = bad) THEN ez

    rules.append(Rule(lambda x, y, z:
                      min(f(z, myVisibility.trapezoids[0]), 1 - f(
                          y, enemyAccuracy.trapezoids[2])), output.trapezoids[2]))  # (myVisibility = invisible) AND !(enemyAccuracy = pro) THEN ez

    rules.append(Rule(lambda x, y, z:
                      min(1 - f(z, myVisibility.trapezoids[2]), f(
                          x, myAccuracy.trapezoids[2])), output.trapezoids[2]))  # !(myVisibility = visible) AND (myAccuracy = pro) THEN ez

    rules.append(Rule(lambda x, y, z:
                      max(f(z, myVisibility.trapezoids[0]), f(
                          x, myAccuracy.trapezoids[2])), output.trapezoids[2]))  # (myVisibility = invisible) OR (myAccuracy = pro) THEN ez

    rules.append(Rule(lambda x, y, z:
                      f(z, myVisibility.trapezoids[2]), output.trapezoids[1]))  # (myVisibility = visible) THEN 50/50

    rules.append(Rule(lambda x, y, z:
                      min(min(f(x, myAccuracy.trapezoids[0]), f(
                          y, enemyAccuracy.trapezoids[2])),
                          1 - f(z, myVisibility.trapezoids[2])), output.trapezoids[1]))  # (myAccuracy = bad) AND (enemyAccuracy = pro) AND !(myVisibility = visible) THEN 50/50

    rules.append(Rule(lambda x, y, z:
                      min(1 - f(y, enemyAccuracy.trapezoids[0]), f(
                          z, myVisibility.trapezoids[1])), output.trapezoids[1]))  # !(enemyAccuracy = bad) AND (myVisibility = slightly visible) THEN 50/50

    rules.append(Rule(lambda x, y, z:
                      min(f(x, myAccuracy.trapezoids[1]), f(
                          y, enemyAccuracy.trapezoids[1])), output.trapezoids[1]))  # (myAccuracy = average) AND (enemyAccuracy = average) THEN 50/50

    rules.append(Rule(lambda x, y, z:
                      min(f(x, myAccuracy.trapezoids[0]), 1 - f(
                          y, enemyAccuracy.trapezoids[0])), output.trapezoids[0]))  # (myAccuracy = bad) AND !(enemyAccuracy = bad) THEN uninstall

    rules.append(Rule(lambda x, y, z:
                      min(min(f(z, myAccuracy.trapezoids[2]), 1 - f(
                          y, enemyAccuracy.trapezoids[2])),
                          f(x, myAccuracy.trapezoids[1])), output.trapezoids[0]))  # (myVisibility = visible) AND (enemyAccuracy = pro) AND (myAccuracy = average) THEN uninstall

    rulesService = RulesService(rules)

    for i in range(0, 5):
        print(rulesService.get_output(
            df.loc[i][0], df.loc[i][1], df.loc[i][2]))
