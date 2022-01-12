# thrown together python code to plot input data

import matplotlib.pyplot as plt
f = open('spacebarlotsofsampels.csv', 'r').read()
datasets = f.split('\n')
xdata = datasets[4].split(',')
xlabel = xdata[0]
x = list(map(float, xdata[1:]))
record_start_time = x[0]
x = list(map(lambda a: a-record_start_time,x))
c = ["r", "g", "b", "o"]
# get 4 corners
for i in range(2,3):
    ydata = datasets[i].split(',')
    ylabel = ydata[0]
    y = list(map(float, ydata[1:]))

    i = i + 1


    line, = plt.plot(x,y)
    line.set_label(ylabel)

    if "Raw Bottom Left" in ylabel:
        peaks = []
        threshold= -0.80
        ind = 0
        for j in range(len(y)):
            if y[j] > threshold:
                print(x[j], y[j])
                peaks.append(x[j])


mouse_data = datasets[len(datasets)-2].split(',')
mouse_label = mouse_data[0]
mouse_timestamps = list(map(float, mouse_data[1:]))
mouse_timestamps= list(map(lambda a: a-record_start_time,mouse_timestamps))
plt.axhline(threshold, color = 'r', label = "Threshold")
distances = []
import math
mouse_label_added = False
for m in mouse_timestamps:
    d = 10000
    r_d = 0
    c = 'r'
    for p in peaks:
        _d = math.sqrt((m -p)**2)
        if d > _d:
            d = _d
            r_d = p-m
    if r_d < 200 and r_d > -200: # may have hit board too lightly/spacebar oversensitive
        distances.append(r_d)
        c = 'g'
    if mouse_label_added:
        plt.axvline(m,color = c)
    else:
        plt.axvline(m, label = mouse_label,color = c)
        mouse_label_added = True

plt.legend()
print(distances)
m = sum(distances)/len(distances)
deviations = list(map(lambda x : (x - m) **2, distances))

import numpy  as np
import scipy.stats as stats
std = math.sqrt(sum(deviations)/len(deviations))
print(m, std) # 18.981040741006534 18.60688510568728
#todo: histogram, gaussian fit
print(len(distances))
print(len(mouse_timestamps))
x = np.linspace(m - 3*std, m + 3*std, 100)
plt.show()
plt.plot(x, stats.norm.pdf(x, m, std))

plt.show()
