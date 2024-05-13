using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    public float upperBound = 1.0f;
    public float lowerBound = -1.0f;
    public GameObject gridText = null;


    private MeshFilter meshFilter = null;
    private MeshRenderer meshRenderer = null;

    
    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void createMesh()
    {
        List<Vector3> verts = new List<Vector3>();
        List<int> indexes = new List<int>();
        List<Color> colors = new List<Color>();

        Vector3 gridOrigin = new Vector3(lowerBound, lowerBound, lowerBound);
        Vector3 xPoint = gridOrigin + new Vector3(-lowerBound + upperBound, 0f, 0f);
        Vector3 yPoint = gridOrigin + new Vector3(0f, -lowerBound + upperBound, 0f);
        Vector3 zPoint = gridOrigin + new Vector3(0f, 0f, -lowerBound + upperBound);

        verts.Add(gridOrigin);
        verts.Add(xPoint);
        verts.Add(yPoint);
        verts.Add(zPoint);

        colors.Add(Color.white);
        colors.Add(Color.white);
        colors.Add(Color.white);
        colors.Add(Color.white);

        indexes.Add(0);
        indexes.Add(1);
        indexes.Add(0);
        indexes.Add(2);
        indexes.Add(0);
        indexes.Add(3);

        Mesh m = new Mesh
        {
            vertices = verts.ToArray(),
            colors = colors.ToArray()
        };

        m.SetIndices(indexes.ToArray(), MeshTopology.Lines, 0);
        meshFilter.mesh = m;
    }

    public void createAndWriteText(Vector3 pos, string text)
    {
        GameObject obj = Instantiate(gridText, this.transform);
        TextMesh textMesh = obj.GetComponent(typeof(TextMesh)) as TextMesh;
        textMesh.text = text;
        obj.transform.localPosition = pos;
        //GameObject cam = GameObject.FindWithTag("myCam");
        //LookCam lookCam = obj.GetComponent(typeof(LookCam)) as LookCam;
        //lookCam.cam = Camera.main.transform;
    }

    public void createAxisText()
    {
        createAndWriteText(new Vector3(upperBound+ 0.1f*upperBound, lowerBound, lowerBound), "x");
        createAndWriteText(new Vector3(lowerBound, upperBound+0.1f * upperBound, lowerBound), "y");
        createAndWriteText(new Vector3(lowerBound, lowerBound, upperBound+0.1f * upperBound), "z");
    }
}
