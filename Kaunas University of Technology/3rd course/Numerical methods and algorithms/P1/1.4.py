from evamik3 import *
from prettytable import PrettyTable

table = PrettyTable()
table.field_names = ["Method", "Interval", "Root", "Precision", "Iterations"]
# f(x) ----------------------------------------------------------
print_method_results(chords_method, f, f_intervals,
                     "Chords method", "f", "x", False, table)
print_method_results(quasi_newton_method, f, f_intervals,
                     "quasi-Newton method", "f", "x", False, table)
print_method_results(scan_method, f, f_intervals,
                     "Scan method", "f", "x", False, table)

# g(x) ---------------------------------------------------------
print_method_results(chords_method, g, g_intervals,
                     "Chords method", "g", "x", False, table)
print_method_results(quasi_newton_method, g, g_intervals,
                     "quasi-Newton method", "g", "x", False, table)
print_method_results(scan_method, g, g_intervals,
                     "Scan method", "g", "x", False, table)

print(table)
