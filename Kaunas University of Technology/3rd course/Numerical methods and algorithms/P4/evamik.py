import matplotlib.pyplot as plt
import math

T1 = 250
TA1 = 320
ts = 15
TA2 = 250
tmax = 100
T2 = 400


def DT(k, T, TA):
    return k(T) * (T-TA)


def TA(t):
    if t < ts:
        return TA1
    elif t <= ts+15:
        return TA1 + (TA2-TA1)/2*(1-math.cos(math.pi/15*(t-ts)))
    else:
        return TA2


def k(T):
    return -0.3+0.22*((T-273)/100) - 0.05*((T-273)/100)**2


def Euler(n, X, Y, dt):
    X.append(X[n-1] + dt)
    Y.append(Y[n-1]+dt*DT(k, Y[n-1], TA(X[n])))


def RK(n, X, Y, dt):
    X.append(X[n-1] + dt)
    Ys = Y[n-1] + dt/2*DT(k, Y[n-1], TA(X[n]))
    Yss = Y[n-1] + dt/2*DT(k, Ys, TA(X[n]+dt/2))
    Ysss = Y[n-1] + dt*DT(k, Yss, TA(X[n]+dt/2))
    Yn = Y[n-1] + dt/6*(DT(k, Y[n-1], TA(X[n]))
                        + 2*DT(k, Ys, TA(X[n]+dt/2))
                        + 2*DT(k, Yss, TA(X[n]+dt/2))
                        + DT(k, Ysss, TA(X[n]+dt)))
    Y.append(Yn)


def Solve(fun, T1, dt, eps):
    X = [0]
    Y = [T1]
    Y2 = [TA(0)]
    n = 1
    prints = 0
    while X[n-1] < tmax:
        fun(n, X, Y, dt)
        Y2.append(TA(X[n]))
        if abs(Y[n] - Y2[n]) < eps and prints < 2:
            print(X[n], Y[n], Y2[n])
            prints += 1
        n += 1
    #print("t =", X[-2], "T =", Y[-2])
    plt.plot(X, Y, label="dt="+str(dt))
    # plt.plot(X, Y2, label="TA, dt="+str(dt))


# Solve(Euler, T1, 2, 0.5e-0)
# Solve(Euler, T1, 1, 5e-1)
# Solve(Euler, T1, 0.25, 2.5e-1)
# Solve(Euler, T1, 0.01, 1e-2)

# Solve(Euler, T2, 2, 1e-1)
# Solve(Euler, T2, 1, 1e-1)
# Solve(Euler, T2, 0.25, 1e-2)
# Solve(Euler, T2, 0.01, 1e-2)

# Solve(RK, T1, 2, 1e-0)
# Solve(RK, T1, 1, 1e-0)
# Solve(RK, T1, 0.25, 1e-1)
# Solve(RK, T1, 0.01, 1e-2)

# Solve(RK, T2, 2, 1e-1)
# Solve(RK, T2, 1, 1e-1)
# Solve(RK, T2, 0.25, 1e-2)
# Solve(RK, T2, 0.01, 1e-2)

Solve(Euler, T1, 2, 0.5e-0)
Solve(Euler, T1, 0.01, 1e-2)
Solve(RK, T1, 2, 1e-0)
Solve(RK, T1, 0.01, 1e-2)

plt.legend(loc='best')
plt.xlabel("t")
plt.ylabel("T", rotation=0)
plt.grid()
plt.show()
