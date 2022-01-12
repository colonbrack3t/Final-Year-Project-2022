# thrown together python code to plot input data
import matplotlib.pyplot as plt
import numpy as np
f = open('headset.csv', 'r').read()
data_sets = f.split('\n')

for i in range(0,len(data_sets)- 1,2):
    x = data_sets[i].split(',')

    xlabel = x[0]
    x = list(map(float, x[1:]))

    i = i + 1
    time = data_sets[i].split(',')
    time = list(map(float, time[1:]))


    line, = plt.plot(time, x)
    line.set_label(xlabel)
    if xlabel == "Headset Height":
        end = False
        ind = 0
        list_of_mins = []
        while(not end):
            try:
                ind = x.index(min(x), ind)
                list_of_mins.append(time[ind])
                ind+=1
            except Exception:
                end = True
        plt.axvline(x=sum(list_of_mins)/len(list_of_mins), color = 'r', linestyle = '-')


plt.ylabel = "Time"
plt.legend()
plt.show()
