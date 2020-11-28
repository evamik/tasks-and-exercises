import numpy as np
import matplotlib.pyplot as plt

def Hermite(X, j, x):
    L=Lagrange(X,j,x)
    DL=D_Lagrange(X,j,X[j])
    U = (1-2*DL*(x-X[j]))*np.power(L, 2)
    V = (x-X[j])*np.power(L, 2)
    return [U, V]

def Lagrange(X, j, x):
    n=len(X)
    L=1
    for k in range(n):
        if k != j:
            L=L*(x-X[k])/(X[j]-X[k])
    return L

def D_Lagrange(X,j,x):
    n=len(X)
    DL=0
    for i in range(n):
        if i==j: continue
        Lds = 1
        for k in range(n): 
            if k!=j and k!=i: 
                Lds = Lds*(x-X[k])
        DL = DL+Lds
    Ldv = 1
    for k in range(n):
        if k!=j:
            Ldv=Ldv*(X[j]-X[k])
    DL=DL/Ldv
    return DL

def f(x,xi,xim1,xip1,yi,yim1,yip1):
    return (2*x-xi-xip1)/((xim1-xi)*(xim1-xip1))*yim1+(2*x-xim1-xip1)/((xi-xim1)*(xi-xip1))*yi+(2*x-xim1-xi)/((xip1-xim1)*(xip1-xi))*yip1

def Akima(X,Y):
    n=len(X)
    DY = np.arange(n).tolist()
    for i in range(n-1):
        if i==0:
            xim1=X[0];xi=X[1];xip1=X[2];yim1=Y[0];yi=Y[1];yip1=Y[2]
            DY[i]=f(xim1,xi,xim1,xip1,yi,yim1,yip1)
        elif i == n:
            xim1=X[n-2];xi=X[n-1];xip1=X[n]; yim1=Y[n-2];yi=Y[n-1];yip1=Y[n]
            DY[n]=f(xip1,xi,xim1,xip1,yi,yim1,yip1)
        else:
            xim1=X[i-1];xi=X[i];xip1=X[i+1]; yim1=Y[i-1];yi=Y[i];yip1=Y[i+1]
            DY[i]=f(xi,xi,xim1,xip1,yi,yim1,yip1)
    return DY


Y = [24.9894,24.9495,25.3771,26.5083,26.2281,25.5893,26.2575,25.9506,26.0383,25.5238,25.1867,25.649,25.649]
X = np.arange(1,14,1).tolist()
plt.plot(X[:12],Y[:12],"ro")
DY = Akima(X,Y)

for iii in range(1,len(X)-1):
    nnn=100
    xxx=np.arange(X[iii], X[iii+1], (X[iii+1]-X[iii])/nnn)
    fff=0
    for j in range(2):
        [U, V] = Hermite(X[iii : iii+2],j,xxx)
        fff=fff+U*Y[iii+j-1] + V*DY[iii+j-1]
    plt.plot(X[iii-1:iii+1], Y[iii-1:iii+1], "b-", linewidth = 3)
    plt.plot(xxx-1, fff, "g-", linewidth = 2)

plt.plot(X[0],Y[0], "b-", linewidth = 3, label = "Pagrindinė")
plt.plot(X[0],Y[0], "g-", linewidth = 2, label = "Spline")
plt.legend(loc='best')
plt.show()