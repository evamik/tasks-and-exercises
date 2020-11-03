import numpy as np
import matplotlib.pyplot as plt
from evamik3 import *

R = final_est(rough_est(), accu_est())
x = np.linspace(R[0], R[1], 10000)
plt.subplot(211)
plt.axhline(y=0, color="black", label="x axis")
plt.plot(x, f(x), label="f(x)")
plt.legend(loc='upper left')
plt.ylim(-2, 4)
plt.xlim(0, 5)
plt.grid()
plt.subplot(212)
plt.axhline(y=0, color="black", label="x axis")
x = np.linspace(g_R[0], g_R[1], 10000)
plt.plot(x, g(x), label="g(x)")
plt.legend(loc='upper left')
plt.ylim(-2, 1.5)
plt.xlim(1, 10)
plt.grid()
plt.show()
