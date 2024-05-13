import numpy as np
import h5py
import pyvista as pv
import time
import datetime

import os
import shutil
from pyvista import examples

"""
b1 = "vectors/b1/b1-000038.h5"
b2 = "vectors/b2/b2-000038.h5"
b3 = "vectors/b3/b3-000038.h5"

f = h5py.File(b1, "r")
b1vec = np.array(f['b1'])
f.close()

f = h5py.File(b2, "r")
b2vec = np.array(f['b2'])
f.close()

f = h5py.File(b3, "r")
b3vec = np.array(f['b3'])
f.close()

nx = b1vec.shape[0]
ny = b1vec.shape[1]
nz = b1vec.shape[2]

origin = 0, 0, 0

mesh = pv.UniformGrid(dims=(nx, ny, nz), spacing=(1, 1, 1), origin=origin)

x = mesh.points[:, 0]
y = mesh.points[:, 1]
z = mesh.points[:, 2]

vectors = np.empty((nx*ny*nz, 3))

vectors[:, 0] = b1vec.flatten()
vectors[:, 1] = b2vec.flatten()
vectors[:, 2] = b3vec.flatten()

print(vectors.shape)

mesh['vectors'] = vectors
stream, src = mesh.streamlines(
    'vectors', return_source=True, terminal_speed=0.0, n_points=100
)

stream.tube(radius=0.2).plot()

print(stream.n_cells)
print(stream.cell_points(98))
print(stream.point_data)
"""

def writeLineFile(point_list, file_path):#, b1, b2, b3, min, max):
    num_points = point_list.shape[0]
    fullarr = np.zeros(num_points * 4, dtype=np.float32)
    for index in range(num_points):
        fullarr[index * 4] = point_list[index][0]
        fullarr[index * 4 + 1] = point_list[index][1]
        fullarr[index * 4 + 2] = point_list[index][2]
        fullarr[index * 4 + 3] = float(1)
        '''
        i1 = point_list[index][0].astype(int)
        i2 = point_list[index][1].astype(int)
        i3 = point_list[index][2].astype(int)

        mag = b1[i1][i2][i3]**2 + b2[i1][i2][i3]**2 + b3[i1][i2][i3]**2
        mag = (mag-min)/max
        fullarr[index * 4 + 3] = mag
        '''


    file = open(file_path + ".raw", "wb")
    file.write(fullarr)
    file.close()

def minmaxMag(b1, b2, b3):
    min = np.finfo(dtype=np.float32).max
    max = np.finfo(dtype=np.float32).min

    b1_flat = b1.flatten()
    b2_flat = b2.flatten()
    b3_flat = b3.flatten()

    for i in range(len(b1_flat)):
        val = b1_flat[i]**2 + b2_flat[i]**2 + b3_flat[i]**2
        if val < min:
            min = val
        if val > max:
            max = val

    return min, max

def writeFrame(path_b1, path_b2, path_b3, vector_path, frame_number):
    try:
        shutil.rmtree(vector_path+"/"+frame_number.__str__(), ignore_errors=True)
    finally:
        os.makedirs(vector_path+"/"+frame_number.__str__())

    b1 = path_b1
    b2 = path_b2
    b3 = path_b3

    f = h5py.File(b1, "r")
    b1vec = np.array(f['b1'])
    f.close()

    f = h5py.File(b2, "r")
    b2vec = np.array(f['b2'])
    f.close()

    f = h5py.File(b3, "r")
    b3vec = np.array(f['b3'])
    f.close()

    nx = b1vec.shape[0]
    ny = b1vec.shape[1]
    nz = b1vec.shape[2]

    #min, max = minmaxMag(b1vec, b2vec, b3vec)

    origin = 0, 0, 0

    mesh = pv.UniformGrid(dims=(nx, ny, nz), spacing=(1, 1, 1), origin=origin)

    x = mesh.points[:, 0]
    y = mesh.points[:, 1]
    z = mesh.points[:, 2]

    vectors = np.empty((nx * ny * nz, 3))

    vectors[:, 0] = b1vec.flatten()
    vectors[:, 1] = b2vec.flatten()
    vectors[:, 2] = b3vec.flatten()

    #print(vectors.shape)

    mesh['vectors'] = vectors
    stream, src = mesh.streamlines(
        'vectors', return_source=True, terminal_speed=0.0, n_points=100
    )

    #stream.tube(radius=0.2).plot()

    #print(stream.n_cells)
    #print(stream.cell_points(98))

    for i in range(stream.n_cells):
        writeLineFile(stream.cell_points(i), vector_path + "/" + frame_number.__str__() + "/" + i.__str__())#, b1vec, b2vec, b3vec, min, max)

def writeSequence(vector_path, result_path):

    listb1 = os.listdir(vector_path + "/b1")
    listb2 = os.listdir(vector_path + "/b2")
    listb3 = os.listdir(vector_path + "/b3")

    listb1.sort()
    listb2.sort()
    listb3.sort()

    seq=0
    for i in range(len(listb1)):
        start = time.time()
        print("Progress: " + seq.__str__() + "/" + (len(listb1) - 1).__str__())
        writeFrame(vector_path + "/b1/" + listb1[i], vector_path + "/b2/" + listb2[i], vector_path + "/b3/" + listb3[i], result_path, i)
        seq = seq + 1
        end = time.time()
        elapsed = end - start
        est_time = ((len(listb1)-1)-seq)*elapsed
        print("Estimated time remaining: " + str(datetime.timedelta(seconds = est_time)))


#writeFrame("vectors/b1/b1-000038.h5", "vectors/b2/b2-000038.h5", "vectors/b3/b3-000038.h5", "test_dir", 38)
#writeSequence("vectors", "test_dir")




