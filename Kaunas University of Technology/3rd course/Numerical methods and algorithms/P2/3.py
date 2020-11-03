import numpy as np
import random
import matplotlib
from matplotlib.patches import Rectangle
from time import sleep
from evamik import *


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


def gradVec(x, x1, fun, S, lam, n, m, i, dx):
    A = []
    for j in range(0, len(x1)):
        xx = x1[j]
        x[i][j] += dx
        dff = fun(x, n, m, S, lam)
        x[i][j] = xx
        ff = fun(x, n, m, S, lam)
        A.append((dff-ff)/dx)
    return A


def gradientMethod(step, itermax, eps, bounds, fun, x, S, lam, dx, m, n=-1):
    if n == -1:
        n = len(x)
    else:
        for _ in range(0, n):
            x.append([random.uniform(bounds[0][0], bounds[0][1]), random.uniform(
                bounds[1][0], bounds[1][1])])
    for _ in range(0, m):
        x.append([0, 0])

    iteration = 0

    ax1 = plot()
    surfaces(ax1, bounds, fun, x, n, m, S, lam)
    plt.draw()
    plt.pause(2)
    steps = [step]*3
    ffs = [0]*3
    while iteration < itermax:
        for i in range(n, n+m):
            j = i-n
            grad = gradVec(x, x[i], fun, S, lam, n, m, i, dx)
            deltax = grad/np.linalg.norm(grad)*steps[j]
            x[i] = (x[i]-deltax).tolist()
            ff = fun(x, n, m, S, lam)
            ax1.scatter(x[i][0], x[i][1], ff, color="blue")
            if(ffs[j] < ff):
                steps[j] /= 1.5
            ffs[j] = ff
        iteration += 1
        for i in range(0, n):
            ax1.scatter(x[i][0], x[i][1], fun(x, n, m, S, lam), color="black")
        surfaces(ax1, bounds, fun, x, n, m, S, lam)
        accuracy = np.linalg.norm(steps)
        ax1.text2D(1, 0.8, " "*20+"Iteration: %d\n%sTarget: %f\n%sAccuracy: %1.2e" %
                   (iteration, " "*20, np.linalg.norm(ffs), " "*20, accuracy), transform=ax1.transAxes)
        plt.draw()
        plt.pause(0.01)
        if(iteration == 1):
            plt.pause(2)
        if(accuracy < eps):
            plt.pause(100)
            return x
        else:
            ax1.cla()
    return x


def surfaces(ax1, bounds, fun, x, n, m, S, lam):
    xx = np.linspace(bounds[0][0], bounds[0][1], 200)
    yy = np.linspace(bounds[1][0], bounds[1][1], 200)
    X, Y = np.meshgrid(xx, yy)
    x1 = x.copy()
    x1[n+m-1] = [X, Y]
    ax1.plot_surface(
        X, Y, fun(x1, n, m, S, lam), color='khaki', alpha=0.4)
    ax1.plot_wireframe(
        X, Y, fun(x1, n, m, S, lam), color='black', alpha=0.7, linewidth=0.1, antialiased=True)


def plot():
    fig1 = plt.figure(1, figsize=plt.figaspect(0.5))
    ax1 = fig1.add_subplot(1, 2, 1, projection='3d')
    ax1.set_xlabel('x')
    ax1.set_ylabel('y')
    ax1.set_ylabel('z')
    ax1.set_zlim(12, 30)
    matplotlib.interactive(True)
    return ax1


step = 1
itermax = 200
eps = 1e-5
bounds = [[-10, 10], [-10, 10]]
x = []
S = 6
lam = 0.2
dx = 0.01
m = 3
n = 5

result = gradientMethod(step, itermax, eps, bounds,
                        target, x, S, lam, dx, m, n)

print("n:\n", np.matrix(result[0:n]), "\nm:\n", np.matrix(result[n:n+m]))
