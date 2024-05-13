using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class SimulationController : MonoBehaviour
{
    public List<Simulation> sims;

    public Simulation particleSim = null;
    public Simulation isosurfaceSim = null;
    public Simulation streamlineSim = null;

    public GridCreator grid = null;

    public GameObject partInfo = null;
    public GameObject isoInfo = null;
    public GameObject streamInfo = null;

    public DrawingMaker draw = null;
    public InformationManager info = null;
    public ScalarAnalyser scalarAnalyser = null;
    public Slider timeline = null;

    public SliderSizer sliderSetup;

    public int maxFrame = 0;
    public int currFrame = 0;

    public bool playing = true;
    private bool toUpdate = false;

    protected IEnumerator coroutine;

    //Sim Settings
    private string basePath = "";
    private string title = "";
    private string description = "";

    private List<string> axisNames = new List<string>(); 
    private Vector3 axisMin = new Vector3();
    private Vector3 axisMax = new Vector3();
    private Vector3Int simRes = new Vector3Int();


    // Start is called before the first frame update
    void Start()
    {
        sims = new List<Simulation>();

        //TestSim
        basePath = "Simulation";
        loadInfo();
        //maxFrame = 50;
        Vector3 dims = new Vector3((axisMax.x - axisMin.x)/20f, (axisMax.y - axisMin.y) / 20f, (axisMax.z - axisMin.z) / 20f);

        /*
        //MomentumSim
        maxFrame = 376;
        Vector3 dims = new Vector3(1f, 1f, 1f);
        basePath = "Assets/Project/Resources/MomentumSim";
        */

        sims.Add(particleSim);
        sims.Add(isosurfaceSim);
        sims.Add(streamlineSim);

        List<Simulation> toRemove = new List<Simulation>();
        foreach(Simulation sim in sims)
        {
            if (!sim.startSim(maxFrame, dims, basePath))
            {
                toRemove.Add(sim);
            };
        }
        foreach(Simulation sim in toRemove)
        {
            sims.Remove(sim);
        }
        toRemove.Clear();

        //Setup grid
        grid.upperBound = dims.x;
        grid.lowerBound = -dims.x;
        grid.createMesh();
        grid.createAxisText();

        //Setup sliders
        sliderSetup.setup(dims);

        //Setup scalar analysis
        scalarAnalyser.setup(basePath, simRes);

        info = GetComponent<InformationManager>();

        draw.maxFrame = maxFrame;
        draw.startNewGroup();

        timeline.maxValue = maxFrame;
        //this.coroutine = AnimateSimulation(0.1f);
        //StartCoroutine(coroutine);
    }

    private void OnEnable()
    {
        this.coroutine = AnimateSimulation(0.07f);
        StartCoroutine(coroutine);
    }

    // Update is called once per frame
    void Update()
    {
        if (toUpdate)
        {
            draw.changeFrame(currFrame);
            //info.updateInfo(currFrame);
            foreach(Simulation sim in sims)
            {
                sim.displayFrame(currFrame);
            }
            toUpdate = false;
        }
    }

    private void loadInfo()
    {
        //Get info lines
        string path = basePath + "/info.txt";
        string[] lines = System.IO.File.ReadAllLines(path);

        //Read first line, sim description and title
        string[] startInfo = lines[0].Split(';');
        maxFrame = Int32.Parse(startInfo[0]);
        title = startInfo[1];
        description = startInfo[2];

        List<string> names = new List<string>();
        List<float> mins = new List<float>();
        List<float> maxs = new List<float>();
        List<int> res = new List<int>();
        List<string> units = new List<string>();

        //Read axis info
        if (lines.Length != 4)
        {
            Debug.Log("Less axes than expected");
        }
        else
        {
            for(int i=1; i<lines.Length; i++)
            {
                string[] axisInfo = lines[i].Split(';');
                names.Add(axisInfo[0]);
                res.Add(Int32.Parse(axisInfo[1]));
                mins.Add(float.Parse(axisInfo[2], CultureInfo.InvariantCulture));
                maxs.Add(float.Parse(axisInfo[3], CultureInfo.InvariantCulture));
                units.Add(axisInfo[4]);
            }
        }
        axisNames = names;
        simRes = new Vector3Int(res[0], res[1], res[2]);
        axisMin = new Vector3(mins[0], mins[1], mins[2]);
        axisMax = new Vector3(maxs[0], maxs[1], maxs[2]);

    }

    public void toggleAnimation(bool activation)
    {
        playing = activation;
    }

    public void changeFrame(float num)
    {
        this.currFrame = (int)num;
        toUpdate = true;
    }

    public void activatePart(bool activation)
    {
        if (activation)
        {
            if (!sims.Contains(particleSim))
            {
                sims.Add(particleSim);
                particleSim.gameObject.SetActive(true);
                partInfo.SetActive(true);
            }
        }
        else
        {
            if (sims.Contains(particleSim))
            {
                sims.Remove(particleSim);
                particleSim.gameObject.SetActive(false);
                partInfo.SetActive(false);
            }
        }
    }

    public void activateIso(bool activation)
    {
        if (activation)
        {
            if (!sims.Contains(isosurfaceSim))
            {
                sims.Add(isosurfaceSim);
                isosurfaceSim.gameObject.SetActive(true);
                isoInfo.SetActive(true);
            }
        }
        else
        {
            if (sims.Contains(isosurfaceSim))
            {
                sims.Remove(isosurfaceSim);
                isosurfaceSim.gameObject.SetActive(false);
                isoInfo.SetActive(false);
            }
        }
    }

    public void activateStream(bool activation)
    {
        if (activation)
        {
            if (!sims.Contains(streamlineSim))
            {
                sims.Add(streamlineSim);
                streamlineSim.gameObject.SetActive(true);
                streamInfo.SetActive(true);
            }
        }
        else
        {
            if (sims.Contains(streamlineSim))
            {
                sims.Remove(streamlineSim);
                streamlineSim.gameObject.SetActive(false);
                streamInfo.SetActive(false);
            }
        }
    }

    private void swapFrame()
    {
        currFrame++;
        if (currFrame > maxFrame)
        {
            currFrame = 0;
        }
        toUpdate = true;
    }

    public void nextFrame()
    {
        currFrame++;
        if (currFrame > maxFrame)
        {
            currFrame = 0;
        }
        toUpdate = true;
    }

    public void prevFrame()
    {
        currFrame--;
        if (currFrame < 0)
        {
            currFrame = maxFrame;
        }
        toUpdate = true;
    }


    protected IEnumerator AnimateSimulation(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            if (playing)
            {
                this.swapFrame();
            }
        }
    }
}
