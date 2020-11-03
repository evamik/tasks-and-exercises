import numpy as np

#a = [-24.49, 55.58, -37.83, 10.19, -0.95]
a = [-48, -28, 8, 7, 1]
# a = [-1440, 504, 64, -30, 2] #pasitikrinimui pagal "2 pavyzdys" įverčius


def f(x):
    return a[4]*(x**4) + a[3]*(x**3) + a[2]*(x**2) + a[1]*x + a[0]


def g(x):
    return np.sin(x)**2 * np.log(x) - x/4


T0 = 250
TA = 320
t1 = 15
T1 = 290


def T(k):
    return (T0 - TA)*np.exp(k*t1) + TA - T1


def rough_est():
    a_array = a
    if a[len(a)-1] < 0:
        a_array = np.array(a_array) * -1
    R = 1 + (np.abs(max(a_array[:-1], key=abs))/a_array[len(a_array)-1])
    return R


def kth_root(x, k):
    if k % 2 != 0:
        res = np.power(np.abs(x), 1/k)
        return res*np.sign(x)
    else:
        return np.power(np.abs(x), 1/k)


def accu_est():
    a_array = np.array(a)
    if a[len(a)-1] < 0:
        a_array = a_array * -1
    B = np.abs(min(a_array[:-1]))
    k = len(a_array)-1 - max(np.where(a_array[:-1] < 0)[0])
    r_teig = 1 + kth_root((B/a_array[len(a_array)-1]), k)

    if (len(a_array)-1) % 2 == 1:
        a_array = a_array * -1
    for i in range(len(a_array)):
        if i % 2 == 1:
            a_array[i] = a_array[i] * -1
    r_neig = 0
    if len(np.where(a_array < 0)[0]) != 0:
        B = np.abs(min(a_array[:-1]))
        k = len(a_array)-1 - max(np.where(a_array[:-1] < 0)[0])
        r_neig = 1 + kth_root((B/a_array[len(a_array)-1]), k)
    return [r_neig, r_teig]


def final_est(R, R2):
    return [-min(R, R2[0]), min(R, R2[1])]


final_R = final_est(rough_est(), accu_est())
g_R = [1, 10]


def scan(start, end, fun, h):
    intervals = []
    x0 = start
    while x0 < end:
        x1 = x0+h
        if np.sign(fun(x0)) != np.sign(fun(x1)):
            intervals.append([x0, x1])
        x0 = x1
    return intervals


def chords_method(xn, xn1, fun, isPrinting):
    k = np.abs(fun(xn)/fun(xn1))
    xmid = (xn + k*xn1) / (1 + k)
    iteration = 1
    while (np.abs(fun(xmid)) > 1E-10):
        if isPrinting:
            print("iteration: %d   x: %.10f   f(x): %1.2e" %
                  (iteration, xmid, fun(xmid)))
        if np.sign(fun(xmid)) == np.sign(fun(xn)):
            xn = xmid
        else:
            xn1 = xmid
        k = np.abs(fun(xn)/fun(xn1))
        xmid = (xn + k*xn1) / (1 + k)
        iteration += 1

    return [iteration, xmid]


def quasi_newton_method(xn, xn1, fun, isPrinting):
    h = (xn1-xn)/100
    xi = xn
    iteration = 0
    while (np.abs(fun(xi)) > 1E-10):
        xi1 = xi - ((fun(xi)-fun(xi-h))/h)**(-1) * fun(xi)
        xi = xi1
        iteration += 1
        if isPrinting:
            print("iteration: %d   x: %.10f   f(x): %1.2e" %
                  (iteration, xi, fun(xi)))
    return [iteration, xi]


def scan_method(xn, xn1, fun, isPrinting):
    h = (xn1-xn)/5
    x0 = xn
    iteration = 0
    while (np.abs(fun(x0)) > 1E-10):
        x1 = x0+h
        if np.sign(fun(x0)) != np.sign(fun(x1)):
            x1 = x0-h
            h = h/5
        x0 = x1
        iteration += 1
        if isPrinting:
            print("iteration: %d   x: %.10f   f(x): %1.2e" %
                  (iteration, x0, fun(x0)))
    return [iteration, x0]


def print_method_results(method, fun, intervals, method_name, fun_name, var_name, isPrinting, table):
    i = 0
    while i < len(intervals):
        results = method(
            intervals[i][0], intervals[i][1], fun, isPrinting)
        if table is not None:
            table.add_row(["#%d %s %s(%s)" % (i+1, method_name, fun_name, var_name), "[%.2f, %.2f]" % (intervals[i][0], intervals[i][1]),
                           "%.4f" % results[1], "%.2e" % fun(results[1]), results[0]])
        i += 1
    print("")


f_intervals = scan(final_R[0], final_R[1], f, 0.5)
g_intervals = scan(1, 10, g, 0.5)
