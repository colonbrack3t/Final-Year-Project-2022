# thrown together python code to plot input data

import matplotlib.pyplot as plt
import numpy as np
f = open(input(), 'r').read()
data_sets = f.split('\n')
x = data_sets[0]
xlabel = x[0]
x = list(map(float, x.split(',')[1:]))
ylabel = ""

# plots each subsequent row as y against first row as x
for i in range(1, len(data_sets) -2):
    y = data_sets[i]
    y = list(map(float, y.split(',')[1:]))
    line, = plt.plot(x, y[:len(x)]) if len(x) < len(y) else plt.plot(x[:len(y)], y)
    line.set_label(y[0])


a = data_sets[len(data_sets) - 2]
print(a)
plt.xlabel = xlabel
plt.legend()
plt.axhline(y=float(a), color = 'r', linestyle = '-')
plt.xticks(np.arange(min(x), max(x)+1, int((max(x)+1)/10)))
plt.show()
