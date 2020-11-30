import numpy as np
import matplotlib.pyplot as plt
from mpl_toolkits import mplot3d
import math


def LF(x):
    s = np.array([(x[0]**2+2*x[1]**2)/2 - 4*math.cos(x[0])-4*math.cos(x[1])-16,
                  -x[0]**2*x[1]**2+8])
    s.shape = (len(s), 1)
    s = np.matrix(s)
    return s


def LF2(x):
    s = np.array([4*x[0] + 2*x[1] + x[2] + 2*x[3] - 14,
                  2*x[3]**2 + 4*x[1]*x[2] + 22,
                  -4*x[0]**2 + 5*x[2]**3 - 3*x[1]*x[3] + 67,
                  2*x[0] - 6*x[1] + 3*x[2] - x[3] + 17])
    s.shape = (len(s), 1)
    s = np.matrix(s)
    return s


def jacobian(x, fun, dx):
    n = len(x)
    A = np.matrix(np.zeros(shape=(n, n)))
    for i in range(0, n):
        x1 = np.matrix(x)
        x1[i] += dx
        A[:, i] = (fun(x1) - fun(x))/dx
    return A


def accuracy(fun):
    accu = 0
    for i in range(0, len(fun)):
        accu += fun.item(i)**2
    return accu


def Newton(x, fun, alpha, dx, maxiter, eps):
    ff = fun(x)
    print("********\nInitial coords = \n%s" % x)
    for i in range(0, maxiter):
        dff = jacobian(x, fun, dx)

        deltax = -np.linalg.solve(dff, ff)
        x1 = np.matrix(x+alpha*deltax)
        ff1 = fun(x1)

        accu = accuracy(fun(x))
        if accu < eps:
            print("Iterations %d: \ncoords = \n%s,  accuracy = %.2e,  eps = %.0e\n********\n" %
                  (i, x, accu, eps))
            return

        ff = ff1
        x = x1
    print("Max iterations reached")
