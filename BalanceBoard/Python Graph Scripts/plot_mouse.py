# thrown together python code to plot input data

import matplotlib.pyplot as plt
f = open('mouse.csv', 'r').read()
datasets = f.split('\n')
# get 4 corners
for i in range(6,len(datasets)- 2,2):
    ydata = datasets[i].split(',')
    ylabel = ydata[0]
    y = list(map(float, ydata[1:]))

    i = i + 1

    xdata = datasets[i].split(',')
    xlabel = xdata[0]
    x = list(map(float, xdata[1:]))
    line, = plt.plot(x,y, color='g')
    line.set_label(ylabel)

    if "Bottom Right" in ylabel:
        peaks = []
        threshold= -0.7
        ind = 0
        for j in range(len(y)):
            if y[j] > threshold:
                print(x[j], y[j])
                peaks.append(x[j])


mouse_data = datasets[len(datasets)-2].split(',')
mouse_label = mouse_data[0]
mouse_timestamps = list(map(float, mouse_data[1:]))
plt.axhline(threshold, color = 'r', label = "Threshold")
distances = []
import math
mouse_label_added = False
for m in mouse_timestamps:
    d = 10000
    r_d = 0
    for p in peaks:
        _d = math.sqrt((m -p)**2)
        if d > _d:
            d = _d
            r_d = p-m
    distances.append(r_d)
    if mouse_label_added:
        plt.axvline(m)
    else:
        plt.axvline(m, label = mouse_label)
        mouse_label_added = True

plt.legend()
print(distances)
m = sum(distances)/len(distances)
deviations = list(map(lambda x : (x - m) **2, distances))
print(deviations)
std = math.sqrt(sum(deviations)/len(deviations))
print(m, std)#-36.61435555749055 16.73778306287719
plt.show()
