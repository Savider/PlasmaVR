using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dummiesman;
using System.IO;

public class IsosurfaceRenderer : Simulation
{
    public string basePath = "";
    public int maxFrame = 0;

    List<Mesh> meshes;

    MeshFilter filter = null;


    public override bool startSim(int numFrames, Vector3 dims, string simPath)
    {
        try
        {
            meshes = new List<Mesh>();
            filter = this.GetComponent<MeshFilter>();
            this.transform.localPosition = new Vector3(-dims.x, -dims.y, -dims.z);
            this.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

            maxFrame = numFrames;
            basePath = simPath + "/Scalars";

            loadSimulation();
        }
        catch
        {
            return false;
        }
        return true;
    }

    void oldloadMesh(string path)
    {
        GameObject meshObject = Resources.Load<GameObject>(path);
        if(meshObject == null)
        {
            meshes.Add(new Mesh());
        }
        else
        {
            meshes.Add(meshObject.GetComponentInChildren<MeshFilter>().sharedMesh);
        }
    }

    public override void displayFrame(int frame)
    {
        filter.mesh = meshes[frame];
    }

    protected override void loadSimulation()
    {
        //basePath = basePath.Replace("Assets/Project/Resources/", "");
        for (int i = 0; i < maxFrame; i++)
        {
            //loadMesh(basePath + "/" + i);
            loadMeshRuntime(basePath + "/" + i + ".obj");
        }
    }

    void loadMeshRuntime(string path)
    {
        //file path
        if (!File.Exists(path))
        {
            Debug.Log("File doesn't exist.");
            meshes.Add(new Mesh());
        }
        else
        {
            GameObject loadedObject = null;
            loadedObject = new OBJLoader().Load(path);
            MeshFilter mf = loadedObject.GetComponentInChildren(typeof(MeshFilter)) as MeshFilter;
            meshes.Add(mf.sharedMesh);
            Destroy(loadedObject);
        }
    }
}
