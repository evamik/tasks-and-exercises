import numpy as np
import random
import matplotlib.cm as cm
import matplotlib as matplotlib
import matplotlib.pyplot as plt
from time import sleep
from multiprocessing import Pool
import time


def dist(x1, y1, x2, y2):
    return ((x1-x2)**2 + (y1-y2)**2)**(0.5)


def avg(x):
    avg = 0
    count = 0
    for i in range(0, len(x)):
        for j in range(i+1, len(x)):
            avg += dist(x[i][0], x[i][1], x[j][0],  x[j][1])
            count += 1
    return avg / count


def target(x, n, m, S, lam):
    distance = 0
    average = avg(x)
    for i in range(0, len(x)):
        for j in range(n, n+m):
            if(i == j):
                continue
            distance += abs(average - dist(x[j][0], x[j][1], x[i][0], x[i][1]))
    Sdistance = 0
    for i in range(n, n+m):
        Sdistance += abs(S-dist(0, 0, x[i][0], x[i][1]))
    return (lam * distance) + (1-lam) * Sdistance


def gradVec(i, x, fun, S, lam, n, m, dx):
    x1 = x[i]
    A = []
    for j in range(0, len(x1)):
        xx = x1[j]
        x[i][j] += dx
        dff = fun(x, n, m, S, lam)
        x[i][j] = xx
        ff = fun(x, n, m, S, lam)
        A.append((dff-ff)/dx)
    return A


# gradient methods iteration in respect to i'th vertice
def gradientIteration(i):
    j = i-n
    grad = gradVec(i, x, target, S, lam, n, m, dx)
    deltax = grad/np.linalg.norm(grad)*steps[j]
    x[i] = (x[i]-deltax).tolist()
    ff = target(x, n, m, S, lam)
    xx = x[i]
    step = steps[j]
    if(ffs[j] < ff):
        x[i] = (x[i]+deltax).tolist()
        step = steps[j] / 1.5
    ffs[j] = ff
    return [step, ff, xx]


# global parameters
x = None
x = None
S = None
lam = None
n = None
m = None
dx = None
steps = None
ffs = None

# initialization of global parameters


def init(x2, S2, lam2, n2, m2, dx2, steps2, ffs2):
    global x
    x = x2
    global S
    S = S2
    global lam
    lam = lam2
    global n
    n = n2
    global m
    m = m2
    global dx
    dx = dx2
    global steps
    steps = steps2
    global ffs
    ffs = ffs2


def gradientMethod(step, itermax, eps, bounds, x, S, lam, dx, m, parallel, processes, n=-1):
    if n == -1:
        n = len(x)
    else:
        for _ in range(0, n):
            x.append([random.uniform(bounds[0][0], bounds[0][1]), random.uniform(
                bounds[1][0], bounds[1][1])])
    for _ in range(0, m):
        x.append([random.uniform(bounds[0][0], bounds[0][1]), random.uniform(
            bounds[1][0], bounds[1][1])])

    iteration = 0
    steps = [step]*m
    ffs = [0]*m
    totaltime = 0
    while iteration < itermax:
        result = []
        start = time.time()
        if parallel:
            # initializing the Pool
            p = Pool(processes=processes, initializer=init, initargs=[
                     x, S, lam, n, m, dx, steps, ffs])
            # mapping each index of a vertice to gradientIteration method
            result = [*p.imap(gradientIteration, range(
                n, n+m))]
            p.close()
        else:
            # initializing global parameters
            init(x, S, lam, n, m, dx, steps, ffs)
            for i in range(n, n+m):
                result.append(gradientIteration(i))
        totaltime += time.time() - start
        result = np.transpose(result)
        steps = result[0]
        ffs = result[1]
        x[n:] = result[2]
        iteration += 1
        accuracy = np.linalg.norm(steps)
        if(accuracy < eps):
            # print("Program finished optimization")
            break
    return [x, totaltime/iteration]


step = 1
itermax = 3
eps = 1e-5
bounds = [[-10, 10], [-10, 10]]
x = []
S = 6
lam = 0.2
dx = 0.01
n = 100

colors = ["black", "darkorange", "tan", "darkgoldenrod", "gold", "darkkhaki",
          "yellowgreen", "green", "turquoise", "teal", "blue", "purple"]
mStart = 5
mEnd = 51
mStep = 5

processCounts = [1, 2, 4, 6, 8, 10, 12]
if __name__ == "__main__":
    X = []
    Ys = []
    Y = []
    for i in range(mStart, mEnd, mStep):
        X.append(i)
        Y.append(gradientMethod(step, itermax, eps, bounds,
                                x, S, lam, dx, i, False, 1, n)[1])
        print(f"appending m={i} time results")
    plt.plot(X, Y, colors[0], label="Sequential")
    print("Plotting sequential times")
    ci = 1
    for j in processCounts:
        Y = []
        for i in range(mStart, mEnd, mStep):
            Y.append(gradientMethod(step, itermax, eps, bounds,
                                    x, S, lam, dx, i, True, j, n)[1])
            print(f"appending m={i} time results")
        plt.plot(
            X, Y, colors[ci], label="Multiprocessing (" + str(j) + " number of processes)")

        Ys.append(Y)
        ci += 1
        print(f"Plotting multiprocessing j={j}")
    plt.xlabel("m")
    plt.ylabel("average time of iteration (s)")
    plt.legend(loc='best')
    plt.show()

    Ys = np.transpose(Ys)
    j = 0
    for i in range(mStart, mEnd, mStep):
        plt.plot(processCounts, Ys[j], colors[j], label="m = " + str(i))
        j += 1
    plt.xlabel("Process count")
    plt.ylabel("average time of iteration (s)")
    plt.legend(loc='best')
    plt.show()
