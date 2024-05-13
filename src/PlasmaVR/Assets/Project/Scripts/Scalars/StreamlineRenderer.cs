using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StreamlineRenderer : Simulation
{
    private const int fieldSize = 288;
    private float stepSize = 0.1f;
    private int steps = 1000;

    Vector3[,,] field = new Vector3[fieldSize, fieldSize, fieldSize];

    private string basePath = "Assets/Project/Data/Vectors";
    private int numberOfSlices = 50;

    List<Mesh> meshes;

    MeshFilter filter = null;

    public override bool startSim(int numFrames, Vector3 dims, string basePath)
    {
        return false;
    }

    override protected void loadSimulation()
    {
    }

    override public void displayFrame(int frame)
    {
    }


    /*
     * To avoid build errors, this class stays commented
     * 
    public override bool startSim(int numFrames, Vector3 dims, string basePath)
    {
        meshes = new List<Mesh>();
        filter = this.GetComponent<MeshFilter>();

        //Transform to normal coordinates and scale
        this.transform.localPosition = new Vector3(-dims.x, -dims.y, -dims.z);
        this.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        loadSimulation();

        return true;
    }

    override protected void loadSimulation()
    {
        for(int i = 0; i<= numberOfSlices; i++)
        {
            loadFrame(i);
        }
    }

    override public void displayFrame(int frame)
    {
        filter.mesh = meshes[frame];
    }

    void importFieldXDim(string filePath)
    {

        if (!System.IO.File.Exists(filePath))
        {
            Debug.LogError("The file does not exist: " + filePath);
            return;
        }

        System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open);
        System.IO.BinaryReader reader = new System.IO.BinaryReader(fs);

        for(int x = 0; x < fieldSize; x++)
        {
            for (int y = 0; y < fieldSize; y++)
            {
                for (int z = 0; z < fieldSize; z++)
                {
                    field[x, y, z].x = reader.ReadSingle();
                }
            }
        }
    }

    void importFieldYDim(string filePath)
    {

        if (!System.IO.File.Exists(filePath))
        {
            Debug.LogError("The file does not exist: " + filePath);
            return;
        }

        System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open);
        System.IO.BinaryReader reader = new System.IO.BinaryReader(fs);

        for (int x = 0; x < fieldSize; x++)
        {
            for (int y = 0; y < fieldSize; y++)
            {
                for (int z = 0; z < fieldSize; z++)
                {
                    field[x, y, z].y = reader.ReadSingle();
                }
            }
        }
    }

    void importFieldZDim(string filePath)
    {

        if (!System.IO.File.Exists(filePath))
        {
            Debug.LogError("The file does not exist: " + filePath);
            return;
        }

        System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open);
        System.IO.BinaryReader reader = new System.IO.BinaryReader(fs);

        for (int x = 0; x < fieldSize; x++)
        {
            for (int y = 0; y < fieldSize; y++)
            {
                for (int z = 0; z < fieldSize; z++)
                {
                    field[x, y, z].z = reader.ReadSingle();
                }
            }
        }
    }

    void importField(string xDim, string yDim, string zDim)
    {
        importFieldXDim(xDim);
        importFieldYDim(yDim);
        importFieldZDim(zDim);
    }

    Vector3 fieldFunction(int x, int y, int z)
    {
        return field[x, y, z];
    }

    void eulerIntegration(float stepSize, int numSteps, Vector3 start)
    {
        GameObject line = new GameObject();
        line.transform.SetParent(transform);
        line.AddComponent<MeshFilter>();
        line.AddComponent<MeshRenderer>();

        line.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Sprites/Default"));

        Vector3[] verts = new Vector3[numSteps];
        Color[] colors = new Color[numSteps];
        int[] indices = new int[numSteps];
        bool breakage = false;
        Vector3 currpos = start;
        for(int i = 0; i<numSteps; i++)
        {
            int xFloor = Mathf.FloorToInt(currpos.x);
            int yFloor = Mathf.FloorToInt(currpos.y);
            int zFloor = Mathf.FloorToInt(currpos.z);
            

            for(int j=0; j<2; j++)
            {
                if (xFloor <0 || xFloor >= fieldSize-1)
                {
                    breakage= true;
                }
                if (yFloor < 0 || yFloor >= fieldSize - 1)
                {
                    breakage= true;
                }
                if (zFloor < 0 || zFloor >= fieldSize - 1)
                {
                    breakage= true;
                }
            }

            if (breakage) {
                indices[i] = i;
                // Vertex colors
                colors[i] = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                // Vertex positions
                verts[i] = currpos;
                continue;
            }

            float xWeight = currpos.x - xFloor;
            float yWeight = currpos.y - yFloor;
            float zWeight = currpos.z - zFloor;

            Vector3 c00 = fieldFunction(xFloor, yFloor, zFloor).normalized * (1 - xWeight) + fieldFunction(xFloor + 1, yFloor, zFloor).normalized * xWeight;
            Vector3 c10 = fieldFunction(xFloor, yFloor+1, zFloor).normalized * (1 - xWeight) + fieldFunction(xFloor + 1, yFloor+1, zFloor).normalized * xWeight;
            Vector3 c01 = fieldFunction(xFloor, yFloor, zFloor+1).normalized * (1 - xWeight) + fieldFunction(xFloor + 1, yFloor, zFloor+1).normalized * xWeight;
            Vector3 c11 = fieldFunction(xFloor, yFloor+1, zFloor+1).normalized * (1 - xWeight) + fieldFunction(xFloor + 1, yFloor+1, zFloor+1).normalized * xWeight;

            Vector3 c0 = c00 * (1 - yWeight) + c10 * yWeight;
            Vector3 c1 = c01 * (1 - yWeight) + c11 * yWeight;

            Vector3 resvec = c0 * (1 - zWeight) + c1 * zWeight;



            indices[i] = i;
            // Vertex colors
            colors[i] = new Color(1.0f, 1.0f, 1.0f, fieldFunction(xFloor, yFloor, zFloor).magnitude);
            // Vertex positions
            verts[i] = currpos;

            currpos = currpos + resvec * stepSize;
        }

        Mesh m = new Mesh
        {
            vertices = verts,
            colors = colors
        };

        m.SetIndices(indices, MeshTopology.LineStrip, 0, true);

        line.GetComponent<MeshFilter>().mesh = m;
    }

    Mesh eulerToMesh(float stepSize, int numSteps, int spacing)
    {
        Vector3[] verts = new Vector3[numSteps*spacing*spacing*spacing];
        Color[] colors = new Color[numSteps*spacing*spacing*spacing];
        int[] indices = new int[(2*numSteps+1) * spacing * spacing * spacing];
        int vertCount = 0;
        int indexCount = 0;

        for (int x = 0; x < spacing; x++)
        {
            for (int y = 0; y < spacing; y++)
            {
                for (int z = 0; z < spacing; z++)
                {
                    Vector3 currpos = new Vector3(fieldSize / spacing * x, fieldSize / spacing * y, fieldSize / spacing * z);
                    bool breakage = false;

                    for (int i = 0; i < numSteps; i++)
                    {
                        int xFloor = Mathf.FloorToInt(currpos.x);
                        int yFloor = Mathf.FloorToInt(currpos.y);
                        int zFloor = Mathf.FloorToInt(currpos.z);

                        if (xFloor < 0 || xFloor >= fieldSize - 1 ||
                                yFloor < 0 || yFloor >= fieldSize - 1 ||
                                zFloor < 0 || zFloor >= fieldSize - 1)
                        {
                            breakage = true;
                        }

                        if (breakage)
                        {
                            indices[indexCount] = vertCount;
                            if (i + 1 == numSteps)
                            {
                                indices[indexCount + 1] = vertCount;
                            }
                            else
                            {
                                indices[indexCount + 1] = vertCount + 1;
                            }
                            // Vertex colors
                            colors[vertCount] = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                            // Vertex positions
                            verts[vertCount] = currpos;
                            vertCount++;
                            indexCount = indexCount + 2;
                            continue;
                        }

                        float xWeight = currpos.x - xFloor;
                        float yWeight = currpos.y - yFloor;
                        float zWeight = currpos.z - zFloor;

                        Vector3 c00 = fieldFunction(xFloor, yFloor, zFloor).normalized * (1 - xWeight) + fieldFunction(xFloor + 1, yFloor, zFloor).normalized * xWeight;
                        Vector3 c10 = fieldFunction(xFloor, yFloor + 1, zFloor).normalized * (1 - xWeight) + fieldFunction(xFloor + 1, yFloor + 1, zFloor).normalized * xWeight;
                        Vector3 c01 = fieldFunction(xFloor, yFloor, zFloor + 1).normalized * (1 - xWeight) + fieldFunction(xFloor + 1, yFloor, zFloor + 1).normalized * xWeight;
                        Vector3 c11 = fieldFunction(xFloor, yFloor + 1, zFloor + 1).normalized * (1 - xWeight) + fieldFunction(xFloor + 1, yFloor + 1, zFloor + 1).normalized * xWeight;

                        Vector3 c0 = c00 * (1 - yWeight) + c10 * yWeight;
                        Vector3 c1 = c01 * (1 - yWeight) + c11 * yWeight;

                        Vector3 resvec = c0 * (1 - zWeight) + c1 * zWeight;



                        indices[indexCount] = vertCount;
                        if (i + 1 == numSteps)
                        {
                            indices[indexCount + 1] = vertCount;
                        }
                        else
                        {
                            indices[indexCount + 1] = vertCount + 1;
                        }
                        // Vertex colors
                        colors[vertCount] = new Color(1.0f, 1.0f, 1.0f, fieldFunction(xFloor, yFloor, zFloor).magnitude);
                        // Vertex positions
                        verts[vertCount] = currpos;

                        currpos = currpos + resvec * stepSize;
                        vertCount++;
                        indexCount = indexCount + 2;
                    }
                }
            }
        }

        Mesh m = new Mesh
        {
            vertices = verts,
            colors = colors
        };

        m.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        m.SetIndices(indices, MeshTopology.Lines, 0, true);

        return m;
    }

    private void saveMesh(string path, Mesh mesh)
    {
        AssetDatabase.CreateAsset(mesh, path);
        AssetDatabase.SaveAssets();
    }

    void loadFrame(int frameNum)
    {
        string num = string.Format("{0,6:D6}", frameNum);

        string[] guids = AssetDatabase.FindAssets(num, new[] { basePath + "/meshes" });

        if(guids.Length == 0)
        {
            //Mesh doesnt exist, create and save.
            string b1 = "/b1/b1-" + num + ".h5.raw";
            string b2 = "/b2/b2-" + num + ".h5.raw";
            string b3 = "/b3/b3-" + num + ".h5.raw";

            importField(basePath + b1, basePath + b2, basePath + b3);
            Mesh m = eulerToMesh(stepSize, steps, 10);
            saveMesh(basePath + "/meshes/frame" + num + ".asset", m);

            meshes.Add(m);
        }
        else
        {
            //Mesh exists, load.
            Mesh m = (Mesh)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[0]), typeof(Mesh));
            
            meshes.Add(m);
        }
    }

    void Start()
    {
        meshes = new List<Mesh>();
        filter = this.GetComponent<MeshFilter>();
        this.transform.localPosition = new Vector3(-1.4f, -1.4f, -1.4f);
        this.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        loadSimulation();
    }



    private void Update()
    {
    }
    */
}
