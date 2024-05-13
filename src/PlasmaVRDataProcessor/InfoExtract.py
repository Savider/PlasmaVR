import h5py
import numpy as np
import os

dataset_folder = "electrons"

def writeInfo(f, maxFrame, title, info):
    f.write(maxFrame.__str__() + ";")
    f.write(title + ";")
    f.write(info + "\n")

def writeAxis(f, number, name, sep, min, max, unit):
    f.write(name + ";")
    f.write(sep.__str__()+";")
    f.write(min.__str__() + ";")
    f.write(max.__str__()+";")
    f.write(unit.__str__()+"\n")

files = os.listdir(dataset_folder)
maxFrame = len(files)-1
info = "simulation"

with h5py.File(dataset_folder + "/" + files[0], "r") as f:
    # List all groups
    print("Keys: %s" % f.keys())
    a_group_key = list(f.keys())[0]
    print(f['SIMULATION'].attrs.keys())

    ndims = f['SIMULATION'].attrs['NDIMS'][0]

    files = os.listdir(dataset_folder)
    maxFrame = len(files)-1
    title = "title"
    info = "description"

    ndims = f['SIMULATION'].attrs['NDIMS'][0]

    file = open("info.txt", "w")

    writeInfo(file, maxFrame,title, info)
    for i in range(ndims):
        number = i+1
        name = ""
        if i == 0:
            name = "x"
        if i == 1:
            name = "y"
        if i == 2:
            name = "z"
        sep = f['SIMULATION'].attrs['NX'][i]
        min = f['SIMULATION'].attrs['XMIN'][i]
        max = f['SIMULATION'].attrs['XMAX'][i]
        unit = "unit"
        writeAxis(file, number, name, sep, min, max, unit)

    file.close()