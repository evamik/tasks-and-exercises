from evamik3 import *
import numpy as np
import matplotlib.pyplot as plt
from prettytable import PrettyTable

intervals = scan(-20, 20, T, 0.3)
table = PrettyTable()
table.field_names = ["Method", "ki", "Root",
                     "Precision", "Iterations", "T(t1)"]
i = 0
while i < len(intervals):
    results = quasi_newton_method(
        intervals[i][0], intervals[i][1], T, False)
    if table is not None:
        table.add_row(["#%d quasi-Newton method T(k)" % (i+1), "%.2f" % intervals[i][0],
                       "%.4f" % results[1], "%.2e" % T(results[1]), "%d" % results[0], "%.2f" % (T(results[1])+T1)])
    i += 1
print(table)

x = np.linspace(intervals[0][0], intervals[len(intervals)-1][1], 1000000)
plt.axhline(y=0, color="black", label="x axis")
plt.plot(x, T(x), label="T(k)")
plt.legend(loc='lower left')
plt.grid()
plt.show()
