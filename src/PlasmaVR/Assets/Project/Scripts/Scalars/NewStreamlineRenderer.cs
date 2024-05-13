using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class NewStreamlineRenderer : Simulation
{
    public string basePath = "";
    public int maxFrame = 0;

    List<Mesh> meshes;

    MeshFilter filter = null;

    override protected void loadSimulation()
    {
        //Now, for each frame create mesh
        for (int i = 0; i < maxFrame; i++)
        {
            loadFrame(basePath + "/" + i.ToString());
        }

    }

    override public void displayFrame(int frame)
    {
        filter.mesh = meshes[frame];
    }

    void Start()
    {
    }

    public override bool startSim(int numFrames, Vector3 dims, string simPath)
    {
        try
        {
            meshes = new List<Mesh>();
            filter = this.GetComponent<MeshFilter>();

            //Transform to normal coordinates and scale
            this.transform.localPosition = new Vector3(-dims.x, -dims.y, -dims.z);
            this.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

            maxFrame = numFrames;
            basePath = simPath + "/Vectors";

            loadSimulation();
        }
        catch
        {
            return false;
        }
        return true;
    }

    private void loadFrame(string path)
    {
        //Get all the line files
        string[] files = System.IO.Directory.GetFiles(path);
        System.Array.Sort(files, System.StringComparer.InvariantCulture);

        //For each file, add line to vertices

        List<Vector3> vertices = new List<Vector3>();
        List<Color> colors = new List<Color>();
        List<int> indices = new List<int>();
        bool newline = true;

        int vertcount = 0;

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].EndsWith(".meta"))
            {
                continue;
            }

            string linepath = files[i];
            //File in format x, y, z, magnitude
            FileStream file = File.Open(linepath, FileMode.Open);
            BinaryReader reader = new BinaryReader(file);
            long streamlength = reader.BaseStream.Length;

            newline = true;
            
            while (true)
            {
                Vector3 vec = new Vector3();
                float magnitude = 0f;
                try
                {
                    vec.x = reader.ReadSingle();
                    vec.y = reader.ReadSingle();
                    vec.z = reader.ReadSingle();
                    magnitude = reader.ReadSingle();
                }catch(EndOfStreamException e)
                {
                    Debug.Log(e.Message);
                    break;
                }
                
                vertices.Add(vec);
                colors.Add(Color.white);
                
                if (!newline)
                {
                    indices.Add(vertcount - 1);
                    indices.Add(vertcount);
                }
                else
                {
                    newline = false;
                }

                vertcount++;
            }

            file.Close();
            
        }

        Mesh m = new Mesh
        {
            vertices = vertices.ToArray(),
            colors = colors.ToArray()
        };

        m.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        m.SetIndices(indices.ToArray(), MeshTopology.Lines, 0, true);

        meshes.Add(m);
    }
}
