import matplotlib.pyplot as plt
from evamik3 import *

R = rough_est()
R2 = accu_est()
print("Rough estimate:", R)
print("Accurate estimate:", -R2[0], R2[1])
R = final_R
print("Roots interval:", final_R[0], final_R[1])

plt.axhline(y=0, color="black", label="x axis")
plt.plot(R, [0, 0], "|", label="Roots interval", ms=10, mew=2)
plt.legend(loc='upper left')
plt.show()
