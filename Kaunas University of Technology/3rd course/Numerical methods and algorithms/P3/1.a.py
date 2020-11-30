import numpy as np
import matplotlib.pyplot as plt


def funkcija11(x):
    ans = np.exp(-2*x) * np.cos(x**2) * (x - 3) * (x**2 + 3)
    return ans


xmin = -3
xmax = 3

N = 15
dx = (xmax-xmin)/(N-1)
X = np.arange(xmin, xmax, dx)
Y = funkcija11(X)

plt.plot(X, Y, "ro")
n = len(X)
BaseMatrix = np.zeros((n, n))
for i in range(n):
    for j in range(n):
        BaseMatrix[i, j] = X[i]**j

coefs = np.linalg.solve(BaseMatrix, Y)

xxx = np.arange(min(X)-0.15, max(X)+0.15, 0.01)
yyy = np.zeros_like(xxx)

for i in range(n):
    yyy += coefs[i]*xxx**i

plt.plot(xxx, yyy, "b-", linewidth=3, label="Pradinė")

plt.plot(xxx, funkcija11(xxx), "g-", linewidth=2, label="Interpoliacinė")
plt.plot(xxx, funkcija11(xxx)-yyy, "r-", linewidth=1, label="Skirtumas")
plt.ylim(-10000, 30000)
plt.legend(loc='best')

plt.show()
