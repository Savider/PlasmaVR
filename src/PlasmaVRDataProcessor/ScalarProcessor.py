import h5py
import numpy as np
import pyvista as pv
import os
import time
import datetime

def isosurfaceSequence(dataset_folder, group_key, isovalue, results_folder, reduction=0.99, raws=False):
    os.mkdir(results_folder)
    if raws:
        os.mkdir(results_folder + "_raws")
    list = os.listdir(dataset_folder)
    seq = 0
    for name in list:
        start = time.time()
        print("Progress: " + seq.__str__() + "/" + (len(list)-1).__str__())
        createIsosurfaceOBJ(dataset_folder + "/" + name, group_key, isovalue, results_folder + "/" + seq.__str__(), reduction)
        if(raws):
            h5toraw(dataset_folder + "/" + name, results_folder + "_raws/" + seq.__str__() + ".raw", group_key)
        seq = seq+1
        end = time.time()
        elapsed = end - start
        est_time = ((len(list) - 1) - seq) * elapsed
        print("Estimated time remaining: " + str(datetime.timedelta(seconds=est_time)))

    files = os.listdir(results_folder)
    for name in files:
        if name.endswith(".mtl"):
            os.remove(results_folder + "/" + name)

def getKeys(dataset_folder):
    things = os.listdir(dataset_folder)
    try:
        with h5py.File(dataset_folder + '/' + things[0], "r") as f:
            keyList = list(f.keys())
            return keyList
    except:
        #Exception ocurred
        return False

def createIsosurfaceOBJ(dataset, group_key, isovalue, target_path, reduction=0.99):
    with h5py.File(dataset, "r") as f:
        # Get the data
        density = np.array(f[group_key])

        values = density

        # Create the spatial reference
        grid = pv.UniformGrid()

        # Set the grid dimensions: shape + 1 because we want to inject our values on
        #   the CELL data
        grid.dimensions = np.array(values.shape)

        # Edit the spatial reference
        grid.origin = (0, 0, 0)  # The bottom left corner of the data set
        grid.spacing = (1, 1, 1)  # These are the cell sizes along each axis

        isos = (isovalue,)

        # Add the data values to the cell data
        grid.point_data["values"] = values.flatten(order="F")  # Flatten the array!

        contour = grid.contour(isosurfaces=isos, method='flying_edges')
        contour = contour.decimate(target_reduction=reduction, progress_bar=True)
        #contour = contour.decimate(reduction=0.95, progress_bar=True, preserve_topology=False)
        #print(contour.GetNumberOfPolys())
        # Now plot the grid!
        #grid.plot(show_edges=False)
        #contour.plot()
        if contour.GetNumberOfPolys()!=0:
            pl = pv.Plotter()
            pl.add_mesh(contour)
            pl.export_obj(target_path)

def h5toraw(curr_path, res_path, key):
    with h5py.File(curr_path, "r") as f:
        # Get the data
        vec = np.array(f[key])
        file = open(res_path, "wb")
        file.write(vec)
        file.close()

#createIsosurfaceOBJ("p1p2p3-electrons-000263.h5", 'p1p2p3', -0.0003, "", 0)
#isosurfaceSequence("charge", 'charge', -2.0, "results", 0.99)
#createIsosurfaceOBJ("charge/charge-electrons-000033.h5", 'charge', -2.0, "")