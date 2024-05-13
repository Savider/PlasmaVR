using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class InformationManager : MonoBehaviour
{
    Dictionary<string, List<float>> information = null;

    List<float> densityX;
    List<float> densityY;
    List<float> densityZ;

    public TextMesh Xdens = null;
    public TextMesh Ydens = null;
    public TextMesh Zdens = null;

    public Text avgPart = null;


    // Start is called before the first frame update
    void Start()
    {
        information = new Dictionary<string, List<float>>();

        addInformation("particleAverage", importFloatFile("Assets/Project/Data/Values/average_energy.raw"));

        loadDensities(38);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateInfo(int frame)
    {
        updateAvgPart(frame);
    }


    void updateAvgPart(int frame)
    {
        avgPart.text = "Avg. particle energy: " + getInfoFrame(frame, "particleAverage").ToString("0.000");
    }

    double getInfoFrame(int frame, string key)
    {
        List<float> list = information[key];
        float thing = list[frame];
        return thing;
    }

    void addInformation(string key, List<float> info)
    {
        information.Add(key, info);
    }

    public List<float> getInfo(string key)
    {
        return information[key];
    }

    List<float> importFloatFile(string filePath)
    {
        if (!System.IO.File.Exists(filePath))
        {
            Debug.LogError("The file does not exist: " + filePath);
            return null;
        }

        List<float> floats = new List<float>();

        System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open);
        System.IO.BinaryReader reader = new System.IO.BinaryReader(fs);
        int count = 0;

        try
        {
            while (true)
            {
                floats.Add(reader.ReadSingle());
                count++;
            }
        } catch(EndOfStreamException e)
        {
            Debug.Log(e.Message);
        }
        return floats;
    }

    void loadDensities(int frame)
    {
        string filePathX = "Assets/Project/Data/Slices/x/" + frame.ToString();
        string filePathY = "Assets/Project/Data/Slices/y/" + frame.ToString();
        string filePathZ = "Assets/Project/Data/Slices/z/" + frame.ToString();

        if (!System.IO.File.Exists(filePathX) || !System.IO.File.Exists(filePathY) || !System.IO.File.Exists(filePathZ))
        {
            Debug.LogError("A file does not exist: " + filePathX);
            return;
        }

        densityX = importFloatFile(filePathX);
        densityY = importFloatFile(filePathY);
        densityZ = importFloatFile(filePathZ);
    }

    void displayDensity(bool activation)
    {
        
    }

    public void updateXDensity(float value)
    {
        if((int)value < densityX.Count)
        {
            Xdens.text = "X1: " + densityX[(int)value].ToString("0.000");
        }
    }

    public void updateYDensity(float value)
    {
        if ((int)value < densityY.Count)
        {
            Ydens.text = "X2: " + densityY[(int)value].ToString("0.000");
        }
    }

    public void updateZDensity(float value)
    {
        if ((int)value < densityZ.Count)
        {
            Zdens.text = "X3: " + densityZ[(int)value].ToString("0.000");
        }
    }


}
