using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public Material mat = null;

    float sizex = 2f;
    float sizey = 1f;

    float minY;
    float maxY;

    public InformationManager info = null;

    private bool created = false;

    MeshFilter lineMeshFilter;
    MeshRenderer lineMeshRender;

    // Start is called before the first frame update
    void Start()
    {
        
        GameObject frame = new GameObject();
        frame.transform.parent = this.transform;
        frame.transform.localPosition = new Vector3();
        frame.transform.localRotation = new Quaternion();
        frame.transform.localScale = new Vector3(1f, 1f, 1f);
        MeshFilter frameMeshFilter = frame.AddComponent(typeof(MeshFilter)) as MeshFilter;
        MeshRenderer frameMeshRender = frame.AddComponent(typeof(MeshRenderer)) as MeshRenderer;

        Mesh frameMesh = new Mesh
        {
            vertices = new Vector3[] {
                new Vector3(0f, 0f, 0f),
                new Vector3(sizex, 0f, 0f),
                new Vector3(sizex, sizey, 0f),
                new Vector3(0f, sizey, 0f),
                new Vector3(0f, 0f, 0f)
            },
            colors = new Color[] {
                new Color(1f, 1f, 1f, 1f),
                new Color(1f, 1f, 1f, 1f),
                new Color(1f, 1f, 1f, 1f),
                new Color(1f, 1f, 1f, 1f),
                new Color(1f, 1f, 1f, 1f)
            }
        };

        frameMesh.SetIndices(new int[] {0, 1, 2, 3, 4}, MeshTopology.LineStrip, 0, true);
        frameMeshFilter.mesh = frameMesh;
        frameMeshRender.material = mat;


        GameObject line = new GameObject();
        line.transform.parent = this.transform;
        line.transform.localPosition = new Vector3();
        line.transform.localRotation = new Quaternion();
        line.transform.localScale = new Vector3(1f, 1f, 1f);
        lineMeshFilter = line.AddComponent(typeof(MeshFilter)) as MeshFilter;
        lineMeshRender = line.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
       
        lineMeshRender.material = mat;

    }

    void buildLine(List<float> values)
    {
        minY = values.Min();
        maxY = values.Max();

        int numVals = values.Count();

        Vector3[] vertexes = new Vector3[numVals];
        Color[] colors = new Color[numVals];
        int[] inds = new int[numVals];

        for(int i=0; i<numVals; i++)
        {
            vertexes[i].x = mapValue(0f, numVals, 0f, sizex, (float)i);
            vertexes[i].y = mapValue(minY, maxY, 0f, sizey, values[i]);
            colors[i] = Color.red;
            inds[i] = i;
        } 

        Mesh lineMesh = new Mesh
        {
            vertices = vertexes,
            colors = colors
        };

        lineMesh.SetIndices(inds, MeshTopology.LineStrip, 0, true);

        lineMeshFilter.mesh = lineMesh;
    }

    void buildLineRenderer(List<float> values)
    {
        GameObject line = new GameObject();
        line.transform.parent = this.transform;
        line.transform.localPosition = new Vector3();
        line.transform.localRotation = new Quaternion();
        line.transform.localScale = new Vector3(1f, 1f, 1f);
        LineRenderer lineRender = line.AddComponent(typeof(LineRenderer)) as LineRenderer;

        minY = values.Min();
        maxY = values.Max();

        int numVals = values.Count();

        Vector3[] vertexes = new Vector3[numVals];

        for (int i = 0; i < numVals; i++)
        {
            vertexes[i].x = mapValue(0f, numVals, 0f, sizex, (float)i);
            vertexes[i].y = mapValue(minY, maxY, 0f, sizey, values[i]);
        }
        lineRender.numCapVertices = 10;
        lineRender.startWidth = 0.01f;
        lineRender.endWidth = 0.01f;
        lineRender.startColor = Color.red;
        lineRender.endColor = Color.green;
        lineRender.useWorldSpace = false;
        lineRender.positionCount = vertexes.Length;
        lineRender.SetPositions(vertexes);
        lineRender.material = mat;
    }

    public float mapValue(float a0, float a1, float b0, float b1, float a)
    {
        return b0 + (b1 - b0) * ((a - a0) / (a1 - a0));
    }

    // Update is called once per frame
    void Update()
    {
        if (!created)
        {
            //buildLine(info.getInfo("particleAverage"));
            buildLineRenderer(info.getInfo("particleAverage"));
            created = true;
        }
    }
}
