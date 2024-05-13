using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScalarAnalyser : MonoBehaviour
{

    Gradient colormap;
    GradientAlphaKey[] ak;
    GradientColorKey[] ck;

    float[,,] loadedFrame;

    public SimulationController sim = null;

    public Texture2D xSlice = null;
    public Texture2D ySlice = null;
    public Texture2D zSlice = null;

    public Slider sliderX = null;
    public Slider sliderY = null;
    public Slider sliderZ = null;

    public RawImage xImage = null;
    public RawImage yImage = null;
    public RawImage zImage = null;

    public RawImage gradientImage = null;

    public TextMesh minValue = null;
    public TextMesh maxValue = null;

    public TextMesh xText = null;
    public TextMesh yText = null;
    public TextMesh zText = null;

    float min = float.MaxValue;
    float max = float.MinValue;

    public string basePath = "";
    public Vector3Int fieldSize = new Vector3Int();

    // Start is called before the first frame update
    void Start()
    {
        setup("D://Sims/TestSim", new Vector3Int(288, 288, 288));
        genColormap();
    }

    public void setup(string newbasePath, Vector3Int newfieldSize)
    {
        basePath = newbasePath;
        fieldSize = newfieldSize;
    }

    public void refresh()
    {
        loadFrame(sim.currFrame);
    }

    public void loadFrame(int frame)
    {
        loadedFrame = new float[fieldSize.x, fieldSize.y, fieldSize.z];
        min = float.MaxValue;
        max = float.MinValue;

        string filePath = basePath + "/ScalarsRaw/" + frame.ToString() + ".raw";

        if (!System.IO.File.Exists(filePath))
        {
            Debug.LogError("The file does not exist: " + filePath);
            return;
        }

        System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open);
        System.IO.BinaryReader reader = new System.IO.BinaryReader(fs);

        for (int x = 0; x < fieldSize.x; x++)
        {
            for (int y = 0; y < fieldSize.y; y++)
            {
                for (int z = 0; z < fieldSize.z; z++)
                {
                    loadedFrame[x, y, z] = reader.ReadSingle();
                    if (loadedFrame[x, y, z] < min) min = loadedFrame[x, y, z];
                    if (loadedFrame[x, y, z] > max) max = loadedFrame[x, y, z];
                }
            }
        }

        minValue.text = min.ToString("0.000");
        maxValue.text = max.ToString("0.000");

        getXslice(0);
        getYslice(0);
        getZslice(0);

        updateGradImage();
    }

    public void getXslice(float fslice)
    {
        int slice = (int)fslice;
        Texture2D currSlice = new Texture2D(fieldSize.y, fieldSize.z);

        for(int y = 0; y < fieldSize.y; y++)
        {
            for (int z = 0; z < fieldSize.z; z++)
            {
                //currSlice.SetPixel(y, z, Color.Lerp(Color.blue, Color.red, (loadedFrame[slice, y, z] - min) / (max - min)));
                currSlice.SetPixel(y, z, colormap.Evaluate((loadedFrame[slice, y, z] - min) / (max - min)));
            }
        }

        xSlice = currSlice;
        xText.text = "x=" + fslice.ToString();
        xImage.texture = xSlice;
        xSlice.Apply();
    }

    public void getYslice(float fslice)
    {
        int slice = (int)fslice;
        Texture2D currSlice = new Texture2D(fieldSize.x, fieldSize.z);

        for (int x = 0; x < fieldSize.x; x++)
        {
            for (int z = 0; z < fieldSize.z; z++)
            {
                currSlice.SetPixel(x, z, colormap.Evaluate((loadedFrame[x, slice, z] - min) / (max - min)));
            }
        }

        ySlice = currSlice;
        yText.text = "y=" + fslice.ToString();
        yImage.texture = ySlice;
        ySlice.Apply();
    }

    public void getZslice(float fslice)
    {
        int slice = (int)fslice;
        Texture2D currSlice = new Texture2D(fieldSize.x, fieldSize.y);

        for (int x = 0; x < fieldSize.x; x++)
        {
            for (int y = 0; y < fieldSize.y; y++)
            {
                currSlice.SetPixel(x, y, colormap.Evaluate((loadedFrame[x, y, slice] - min) / (max - min)));
            }
        }

        zSlice = currSlice;
        zText.text = "z=" + fslice.ToString();
        zImage.texture = zSlice;
        zSlice.Apply();
    }

    void genColormap()
    {

        colormap = new Gradient();

        ck = new GradientColorKey[3];
        ck[0].color = Color.blue;
        ck[0].time = 0.0f;
        ck[1].color = Color.yellow;
        ck[1].time = 0.5f;
        ck[2].color = Color.red;
        ck[2].time = 1.0f;

        ak = new GradientAlphaKey[3];
        ak[0].alpha = 1.0f;
        ak[0].time = 0.0f;
        ak[1].alpha = 1.0f;
        ak[1].time = 0.5f;
        ak[2].alpha = 1.0f;
        ak[2].time = 1.0f;

        colormap.SetKeys(ck, ak);
    }

    void updateGradImage()
    {
        Texture2D grad = new Texture2D(100, 1);
        for(int i = 0; i<100; i++)
        {
            grad.SetPixel(i, 0, colormap.Evaluate(i / 100f));
        }
        grad.Apply();
        gradientImage.texture = grad;
    }

}
