import numpy as np
import matplotlib.pyplot as plt

Y = [24.9894,24.9495,25.3771,26.5083,26.2281,25.5893,26.2575,25.9506,26.0383,25.5238,25.1867,25.649]
X = np.arange(1,13,1).tolist()

Y.insert(0, 24.9894)
Y.append(25.649)
X.insert(0,0.99)
X.append(12.01)

plt.plot(X,Y,"ro")
n = len(X)
BaseMatrix = np.zeros((n,n))
for i in range(n):
    for j in range(n):
        BaseMatrix[i,j] = X[i]**j

coefs = np.linalg.solve(BaseMatrix, Y)

xxx = np.arange(min(X), max(X)+0.01, 0.01)
yyy = np.zeros_like(xxx)

for i in range(n):
    yyy += coefs[i]*xxx**i

plt.plot(xxx, yyy, "b-", linewidth = 3, label = "Interpoliacinė")
plt.legend(loc='best')
plt.show()