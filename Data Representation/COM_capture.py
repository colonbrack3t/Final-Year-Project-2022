from re import M
from site import venv
import matplotlib.pyplot as plt
import matplotlib.patches as ptc
import numpy as np
import os

from matplotlib.patches import Ellipse
import matplotlib.transforms as transforms

# simple script for formatting data from csv
def get_data(d):
        return list(map(float, d.split(",")[1:]))

#Centres values on average
def centre_on_average(l):
    avg = np.average(l)
    return list(map (lambda x : x - avg, l))

# plots head x and head z change over time
def plot_head_xz(header, tl, tr, bl, br, headx, headz, t, ax,  label):
    plot_sensitivity_change(header, label)
    headx=centre_on_average(headx)
    headz=centre_on_average(headz)
    plt.plot(t , headx, label=f"Head left/right {label}")
    plt.plot(t , headz, label=f"Head back/forth {label}")

#plots centre of mass (head x and z)
def centre_of_mass(header, tl, tr, bl, br, headx, headz, t, ax,  label):
    plt.scatter(headx, headz, label = label)


# plots vertical line when sensitivity was changed / stage change
def plot_sensitivity_change(header, label):
    # header format example : [ "Date: 16/03/2022 13:43:42", "Trial #: 1/6",  "Sensitivty: 1", 
    #                           "Control time: 30",          "Test time: 30", "Aftermath time: 15",
    #                           "Pause time : 120" ]

    def get_data_from_header(d):
        return float (d.split(": ")[1])
    
    control_time = get_data_from_header(header[3])
    test_time = get_data_from_header(header[4])

    #plot vertical lines
    plt.axvline(control_time, label = f"Apply sway {label}", c = "red")
    plt.axvline(control_time + test_time, label = f"End sway {label}", c= "red") 
# parse data 
# data example: [0] | Date: 16/03/2022 13:43:42 ,  Trial #: 1/6 ,     Sensitivty: 1 ,     Control time: 30 ,  Test time: 30 ,     Aftermath time: 15 ,    Pause time : 120 
#               [1] | Top Left ,                  -0.6326048 ,        -0.6326048 ,        -0.6627289 ,        -0.6326048 ,        -0.6024808 ,            -0.6426462 , ...
#               [2] | Top Right ,                 1.055239 ,          1.055239 ,          0.9584283 ,         0.9681093 ,         0.9584283 ,             0.9777904 , ...
#               [3] | Bottom Left ,               -0.7425287 ,        -0.7425287 ,        -0.7816092 ,        -0.8011494 ,        -0.7522988 ,            -0.7816092 , ...
#               [4] | Bottom Right ,              -0.566987 ,         -0.566987 ,         -0.6342567 ,        -0.5958169 ,        -0.5862069 ,            -0.6342567 , ...
#               [5] | Head ,                      (0.1; 1.2; -0.5) ,  (0.1; 1.2; -0.5) ,  (0.1; 1.2; -0.5) ,  (0.1; 1.2; -0.5) ,  (0.1; 1.2; -0.5) ,      (0.1; 1.2; -0.5) , ...
#               [6] | Time ,                      0.02 ,              0.04 ,              0.06 ,              0.08 ,              0.09999999 ,            0.12 , ...                
#              
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

# generate plot of each sensor's readings over time
def gen_sensor_plot(header, tl, tr, bl, br, headx, headz, t, ax, label):
    plot_sensitivity_change(header, label)
   
    plt.plot(t, tl, label = f"Front left {label}")
    plt.plot(t, tr, label = f"Front right {label}")
    plt.plot(t, bl, label = f"Bottom left {label}")
    plt.plot(t, br, label = f"Bottom right {label}")

# calculates ratio of sensor readings to calculate centre of pressure for x and y
def get_centre_of_pressure(tl,tr,bl,br):
    comx ,  comy = [] , []
    for i in range(len(tl)):
        
        
        if tl[i] +  tl[i]  + bl[i] + br[i] < 60:
            continue;    
        top = tl[i] / (tl[i] + tr[i])
        bot = bl[i] / (bl[i] + br[i])
        
        left = tl[i] / (tl[i] + bl[i])
        right = tr[i] / (tr[i] + br[i]) 
        x = (top + bot) / 2

        y = (left + right) / 2
    
        if x > 1:
            print(top,bot, tl[i],tl[i] + tr[i],bl[i] , (bl[i] + br[i] ))
            continue
        comx.append(x)
        comy.append(y)
    return comx , comy
def centre_of_pressure(header, tl, tr, bl, br, headx, headz, t, ax, label):
    #plt.scatter(0.5,0.5)
    comx ,  comy = get_centre_of_pressure(tl,tr,bl,br)
    
    plt.scatter(comx, comy, label = f"Centre of mass - Trial {label}")

def centre_of_mass_PCA(header, tl, tr, bl, br, headx, headz, t, ax, label ):
    plt.scatter(headx,headz, s=1, color = "none")
    confidence_ellipse(headx, headz, ax, n_std =2.0,  linestyle='--', label = label)
def centre_of_pressure_PCA(header, tl, tr, bl, br, headx, headz, t, ax, label):
    #plt.scatter(0.5,0.5)
    comx ,  comy = get_centre_of_pressure(tl,tr,bl,br)
    plt.scatter(comx,comy, s=1, color = "none")
    confidence_ellipse(comx, comy, ax, n_std =2.0,  linestyle='--', label = label)
    
# inspired by https://matplotlib.org/stable/gallery/statistics/confidence_ellipse.html#sphx-glr-gallery-statistics-confidence-ellipse-py
# performs PCA on 2d set and plots eclipse with n_std standard deviation width in both axes
def confidence_ellipse(x, y, ax, n_std=3.0,facecolor="none" ,**kwargs):
    cov = np.cov(x, y)
    pearson = cov[0, 1]/np.sqrt(cov[0, 0] * cov[1, 1])
    # Using a special case to obtain the eigenvalues of this
    # two-dimensionl dataset.
    ell_radius_x = np.sqrt(1 + pearson)
    ell_radius_y = np.sqrt(1 - pearson)
    mean_x = np.mean(x)
    mean_y = np.mean(y)
    ellipse = Ellipse((0, 0), width=ell_radius_x * 2, height=ell_radius_y * 2, edgecolor = np.random.rand(3),
                      facecolor=facecolor,linewidth=3, **kwargs)
    
    # Calculating the stdandard deviation of x from
    # the squareroot of the variance and multiplying
    # with the given number of standard deviations.
    scale_x = np.sqrt(cov[0, 0]) * n_std
    

    # calculating the stdandard deviation of y ...
    scale_y = np.sqrt(cov[1, 1]) * n_std
    

    transf = transforms.Affine2D() \
        .rotate_deg(45) \
        .scale(scale_x, scale_y) \
        .translate(mean_x, mean_y)

    ellipse.set_transform(transf + ax.transData)
    return ax.add_patch(ellipse)


    
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
    f = path + enum_files[i][1]
    data_set = open(f , 'r').read().split('\n')
    files_data.append(split_data(data_set))

def plot_multiple_files_with_func(fs, plot_func, title):
    fig, ax = plt.subplots(figsize=(6, 6))
    
    for i in range(len(fs)):
        header, tl, tr, bl, br, headx, headz, t = files_data[i]
        plot_func(header, tl, tr, bl, br, headx, headz, t, ax, header[1] + " " + header[2])
    ax.set_title(title + "\n" + header[0])
    plt.legend()
    plt.show()
plot_multiple_files_with_func(files_data, centre_of_mass, "Centre of mass using Headset x and z positions")
plot_multiple_files_with_func(files_data, centre_of_mass_PCA,  "Principle Component analysis on Centre of mass using Headset x and z positions")
plot_multiple_files_with_func(files_data, centre_of_pressure, "Data for centre of pressure using Wiiboard sensors")
plot_multiple_files_with_func(files_data, centre_of_pressure_PCA, "Principle Component analysis on centre of pressure using Wiiboard sensors")


plot_multiple_files_with_func(files_data, plot_head_xz, "Head x and z positions over time")
if len(files_data) == 1: 
    plot_multiple_files_with_func(files_data, gen_sensor_plot, "Sensor readings over time")

