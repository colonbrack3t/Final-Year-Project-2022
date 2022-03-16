from site import venv
import matplotlib.pyplot as plt
import numpy as np
import os


def get_data(d):
        return list(map(float, d.split(",")[1:]))

def plot_head_xz(header, tl, tr, bl, br, headx, headz, t, label, color = "black"):
    plot_sensitivity_change(header, label,color)
    plt.plot(t , headx, label=f"Head left/right {label}",color = color)
    plt.plot(t , headz, label=f"Head back/forth {label}",color = color)

def plot_centre_of_mass(header, tl, tr, bl, br, headx, headz, t, label, color = "black"):
    plt.scatter(headx, headz, label = f"Trial {label}", color = color)



def plot_sensitivity_change(header, label, color):
    def get_data_from_header(d):
        return float (d.split(": ")[1])
    control_time = get_data_from_header(header[3])
    test_time = get_data_from_header(header[4])
    plt.axvline(control_time, label = f"Apply sway {label}", color = color)
    plt.axvline(control_time + test_time, label = f"End sway {label}", color = color) 

def split_data(data):
    header = data[0].split(",")
    tl = get_data(data[1])
    tr = get_data(data[2])
    bl = get_data(data[3])
    br = get_data(data[4])
    t = get_data(data[6])
    head = data[5].replace(")","").replace("(","").split(",")[1:]
    headx = list(map( lambda vector : float(vector.split(";")[0]), head ))
    headz = list(map( lambda vector : float(vector.split(";")[2]), head ))

    return header, tl, tr, bl, br, headx, headz, t

def gen_sensor_plot(header, tl, tr, bl, br, headx, headz, t, label, color = "black"):
    plot_sensitivity_change(header, label,color)
    plt.plot(t, tl, label = f"Front left - Trial {label}", color = color)
    plt.plot(t, tr, label = f"Front right - Trial {label}",color = color)
    plt.plot(t, bl, label = f"Bottom left - Trial {label}",color = color)
    plt.plot(t, br, label = f"Bottom right - Trial {label}",color = color)


def centre_of_pressure(header, tl, tr, bl, br, headx, headz, t,label, color = "black"):

    comx = []
    comy = []

    for i in range(len(tl)):
        # todo - do better
        if tl[i] < 0 or tr[i] < 0 or bl[i] < 0 or br[i] < 0 :
                continue
            
        top = tl[i] / (tl[i] + tr[i])
        bot = bl[i] / (bl[i] + br[i])
        left = tl[i] / (tl[i] + bl[i])
        right = tr[i] / (tr[i] + br[i]) 
        x = (top + bot) / 2

        y = (left + right) / 2

        if x > 1:
            print(top,bot, tl[i],tl[i] + tr[i],bl[i] , (bl[i] + br[i] ))
        comx.append(x)
        comy.append(y)
    plt.scatter(comx, comy, label = f"Centre of mass - Trial {label}",color = color)




    
path = input("Input path of dir: ")
files = os.listdir(path)
valid_files = []
for file in files:
    if file.endswith(".csv"):
        valid_files.append(file)
enum_files = list(enumerate(valid_files))
for e in enum_files:
    print(e)
indices = list(map (int , input("Enter corresponding number for desired files (space seperated)").split(" ")))
files_data = []

    
for i in indices:
    f = enum_files[i][1]
    data_set = open(f , 'r').read().split('\n')
    files_data.append(split_data(data_set))
colors = ["red", "green", "blue", "yellow", "purple", "black", "grey"]
def plot_multiple_files_with_func(fs, plot_func):
    for i in range(len(fs)):
        header, tl, tr, bl, br, headx, headz, t = files_data[i]
        plot_func(header, tl, tr, bl, br, headx, headz, t, header[1], colors[i])
    plt.legend()
    plt.show()
plot_multiple_files_with_func(files_data, plot_head_xz)
plot_multiple_files_with_func(files_data, gen_sensor_plot)
