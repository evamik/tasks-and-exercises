import numpy as np
from evamik import *

xs = np.array([np.matrix('1.1;1;1;1'),
               np.matrix('1.1;0;0;0')
               ])
dx = 0.01
for i in range(0, len(xs)):
    Newton(xs[i], LF2, 1, dx, 100, 1e-6)
