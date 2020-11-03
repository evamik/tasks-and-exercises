import numpy as np
from PyFunkcijos import *

# -------------------iseities duomnenys:
A = np.matrix([[2, 4,  6, -2],
               [1, 3,  1, -3],
               [1, 1,  5,  1],
               [2, 3, -3, -2]]).astype(np.float)        # koeficientu matrica
# laisvuju nariu vektorius-stulpelis
b = (np.matrix([4, -7, 11, -4])).transpose().astype(np.float)
n = (np.shape(A))[0]   # lygciu skaicius nustatomas pagal ivesta matrica A
# laisvuju nariu vektoriu skaicius nustatomas pagal ivesta matrica b
nb = (np.shape(b))[1]

T = ScrollTextBox(100, 50)  # sukurti teksto isvedimo langa
SpausdintiMatrica(A, T, 'A')
SpausdintiMatrica(b, T, 'b')
SpausdintiMatrica(n, T, 'n')
SpausdintiMatrica(nb, T, 'nb')

Q = np.identity(n)
for i in range(0, n-1):
    z = A[i:n, i]
    SpausdintiMatrica(z, T, 'z')
    zp = np.zeros(np.shape(z))
    zp[0] = np.linalg.norm(z)
    omega = z-zp
    omega = omega/np.linalg.norm(omega)
    Qi = np.identity(n-i)-2*omega*omega.transpose()
    SpausdintiMatrica(Qi, T, 'Qi')
    A[i:n, :] = Qi.dot(A[i:n, :])
    SpausdintiMatrica(A, T, 'A')
    Q = Q.dot(
        np.vstack(
            (
                np.hstack((np.identity(i), np.zeros(shape=(i, n-i)))),
                np.hstack((np.zeros(shape=(n-i, i)), Qi))
            )
        )
    )
    SpausdintiMatrica(Q, T, 'Q')

# atgalinis etapas:
b1 = Q.transpose().dot(b)
SpausdintiMatrica(b1, T, 'b1')
x = np.zeros(shape=(n, nb))
SpausdintiMatrica(A, T, 'A')
for i in range(n-1, -1, -1):
    if(A[i, i] == 0):
        x[i, :] = 1
        T.insert(END, "\nYra be galo daug sprendinių, imame, jog x%d = 1" % (i+1))
    else:
        x[i, :] = (b1[i, :]-A[i, i+1:n]*x[i+1:n, :])/A[i, i]

SpausdintiMatrica(x, T, 'x')

str1 = input("Baigti darba? Press Enter \n")
