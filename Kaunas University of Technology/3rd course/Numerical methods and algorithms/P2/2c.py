import numpy as np
from evamik import *

xs = np.array([np.matrix('-7; 0.5'),
               np.matrix('-1; 4'),
               np.matrix('1; 4'),
               np.matrix('7; 0.5'),
               np.matrix('7; -0.5'),
               np.matrix('1; -4'),
               np.matrix('-1; -4'),
               np.matrix('-7; -0.5')
               ])
dx = 0.01
for i in range(0, len(xs)):
    Newton(xs[i], LF, 1, dx, 100, 1e-6)
