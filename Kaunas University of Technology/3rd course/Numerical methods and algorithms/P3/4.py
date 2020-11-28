import numpy as np
import matplotlib.pyplot as plt

def base(m,x):
    G=np.zeros((len(x),m))
    for i in range(m):
        G[:,i]=np.power(x,i)
    return G


Y = [24.9894,24.9495,25.3771,26.5083,26.2281,25.5893,26.2575,25.9506,26.0383,25.5238,25.1867,25.649]
X = np.arange(1,13,1)

for i in range(2, 6+1):
    m = i
    G=base(m, X)
    Gt = np.transpose(G)
    a= np.dot(Gt, G)
    x = np.dot(Gt, Y)
    c=np.linalg.solve(a, x)

    nnn = 200
    xxx = np.arange(min(X), max(X), (max(X)-min(X))/nnn)
    Gv = base(m, xxx)
    fff=np.dot(Gv, c)

    plt.plot(X,Y,"ro")
    plt.plot(xxx,fff,"r-",label=str(m) + "-eilės")
    plt.legend(loc='best')
    plt.show()
