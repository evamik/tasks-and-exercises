from matplotlib import pyplot as plt
from numpy.lib.function_base import median
from sklearn.linear_model import LinearRegression
import numpy as np


def read_sunspot():
    data = {"year": [], "spots": []}

    with open("sunspot.txt") as f:
        rows = f.readlines()
        for row in rows:
            values = row.split('\t')
            data["year"].append(int(values[0]))
            data["spots"].append(int(values[1]))
    return data


def split_data(values, n):
    P = []
    T = []

    for i in range(n, len(values)):
        P.append(values[i-n:i])
        T.append(values[i])

    return P, T


if __name__ == "__main__":
    data = read_sunspot()
    plt.plot(data["year"], data["spots"])
    plt.xlabel("Year")
    plt.ylabel("Spots")
    plt.show()

    P, T = split_data(data["spots"], 2)

    fig = plt.figure()
    ax = plt.axes(projection="3d")
    P_transposed = np.transpose(P)
    ax.scatter3D(P_transposed[0], P_transposed[1], T)
    ax.set_xlabel("P pirmas")
    ax.set_ylabel("P antras")
    ax.set_zlabel("T")
    plt.show()

    Pu = P[:200]
    Tu = T[:200]

    linearModel = LinearRegression().fit(Pu, Tu)
    print(linearModel.coef_)

    Tsu = linearModel.predict(Pu)
    plt.plot(data["year"][:200], Tu, label="Real")
    plt.xlabel("Year")
    plt.ylabel("Spots")
    plt.plot(data["year"][:200], Tsu, label="Predicted")
    plt.legend()
    plt.title("With training data")
    plt.show()

    Pu2 = P[200:400]
    Tu2 = T[200:400]

    Tsu2 = linearModel.predict(Pu2)
    plt.plot(data["year"][200:200+len(Tu2)], Tu2, label="Real")
    plt.xlabel("Year")
    plt.ylabel("Spots")
    plt.plot(data["year"][200:200+len(Tu2)], Tsu2, label="Predicted")
    plt.legend()
    plt.title("With unseen data")
    plt.show()

    e = Tu-Tsu
    plt.plot(data["year"][:len(e)], e)
    plt.xlabel("Year")
    plt.ylabel("Error")
    plt.title("e")
    plt.show()

    plt.hist(e)
    plt.show()

    MSE = (1/len(e)) * sum(map(lambda x: x**2, e))
    print(MSE)

    MAD = median(abs(e))
    print(MAD)
