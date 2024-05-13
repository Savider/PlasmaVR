import h5py
import numpy as np
import pyvista as pv
import os
import random
import sys
import time
import datetime

dataset = "electrons/RAW-electrons-000028.h5"

resolution = 512

def checkMaxParticles(dataset, scalar_key):
    with h5py.File(dataset, "r") as f:
        scalar = np.array(f[scalar_key])
        return scalar.size, scalar.min(), scalar.max()

def getKeys(dataset_folder):
    things = os.listdir(dataset_folder)
    try:
        with h5py.File(dataset_folder + '/' + things[0], "r") as f:
            keyList = list(f.keys())
            return keyList
    except:
        #Exception ocurred
        return False

def createParticleRaw(dataset, result_path, scalar_key, maxValue, minValue, reduction=1):
    with h5py.File(dataset, "r") as f:
        # Get the data
        scalars = np.atleast_1d(f[scalar_key])
        x1 = np.atleast_1d(f['x1'])
        x2 = np.atleast_1d(f['x2'])
        x3 = np.atleast_1d(f['x3'])

        fullarr = np.zeros(scalars.size * 4, dtype=np.float32)

        if scalars.size == 0:
            file = open(result_path, "wb")
            file.close()
            return

        exec = 0
        for index in range(scalars.size):
            if random.random() >= reduction:
                continue
            exec += 1
            if exec >= resolution*resolution:
                break
            fullarr[(exec-1) * 4] = x1[index]
            fullarr[(exec-1) * 4 + 1] = x2[index]
            fullarr[(exec-1) * 4 + 2] = x3[index]
            fullarr[(exec-1) * 4 + 3] = (scalars[index] - minValue) / maxValue

        file = open(result_path, "wb")
        file.write(fullarr[:exec*4])
        file.close()



def particleSequence(dataset_folder, results_folder, group_key):
    os.mkdir(results_folder)
    list = os.listdir(dataset_folder)

    maxParts = 0
    scalarMin= sys.float_info.max
    scalarMax= sys.float_info.min

    for name in list:
        partsNum, min, max = checkMaxParticles(dataset_folder + "/" + name, group_key)
        if partsNum > maxParts:
            maxParts = partsNum
        if min < scalarMin:
            scalarMin = min
        if max > scalarMax:
            scalarMax = max

    reduction = 1
    if maxParts > resolution*resolution:
        reduction = resolution*resolution / maxParts

    num = 0
    for name in list:
        start = time.time()
        print("Progress: " + num.__str__() + "/" + (len(list)-1).__str__())
        createParticleRaw(dataset_folder+"/"+name, results_folder+"/"+num.__str__()+".raw", group_key, scalarMax, scalarMin, reduction)
        num = num+1
        end = time.time()
        elapsed = end - start
        est_time = ((len(list) - 1) - num) * elapsed
        print("Estimated time remaining: " + str(datetime.timedelta(seconds=est_time)))

    return True

def ParticleRaws(datasetPath):
    with h5py.File(dataset, "r") as f:
        # List all groups
        print("Keys: %s" % f.keys())
        a_group_key = list(f.keys())[0]

        # Get the data
        energies = np.array(f['ene'])
        x1 = np.array(f['x1'])
        x2 = np.array(f['x2'])
        x3 = np.array(f['x3'])
        ene_max = energies.max()

        outputSingleRaw(x1, x2, x3, energies, "particle28.raw")

def outputSingleRaw(x1, x2, x3, scalar, scalar_min, scalar_max, result_path):
    fullarr = np.zeros(scalar.size * 4, dtype=np.float32)

    for index in range(scalar.size):
        fullarr[index*4]=x1[index]
        fullarr[index * 4 + 1] = x2[index]
        fullarr[index * 4 + 2] = x3[index]
        fullarr[index * 4 + 3] = (scalar[index]-scalar_min)/scalar_max

    file = open(result_path, "wb")
    file.write(fullarr)
    file.close()

def avgEnergyData(dataset_folder, results_file):
    list = os.listdir(dataset_folder)

    for name in list:
        print("Doing "+ name)
        appendAvgEnergyData(dataset_folder+"/"+name, results_file)

def appendAvgEnergyData(datasetPath, result_path):
    with h5py.File(datasetPath, "r") as f:
        # List all groups
        #print("Keys: %s" % f.keys())
        #a_group_key = list(f.keys())[0]

        # Get the data
        energies = np.array(f['ene'])
        #x1 = np.array(f['x1'])
        #x2 = np.array(f['x2'])
        #x3 = np.array(f['x3'])
        avg = np.average(energies)

        print(avg)
        file = open(result_path, "ab")
        file.write(avg)
        file.close()

#particleSequence("electrons", "particle_results", 'ene')

#avgEnergyData("electrons", "average_energy_test.raw")