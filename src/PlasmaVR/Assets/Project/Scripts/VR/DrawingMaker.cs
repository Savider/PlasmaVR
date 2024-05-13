using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class DrawingMaker : MonoBehaviour
{
    public GameObject lineControllerPrefab = null;
    public GameObject drawingCanvas = null;
    public SimulationController sim = null;

    GameObject lineObject = null;
    LineRenderer activeLine = null;

    List<GameObject> groups = null;
    List<GroupController> groupControllers = null;

    GameObject activeGroup = null;

    List<Vector3> positions = null;

    Vector3 prevPos = new Vector3();
    double maxMagnitude = 0.01;

    public Transform pointer = null;

    public SteamVR_Action_Boolean m_ActivatePress = null;

    public Color lineColor = Color.red;

    private int numGroups = 0;

    public int maxFrame = 50;

    // Start is called before the first frame update
    void Start()
    {
        groupControllers = new List<GroupController>();
        groups = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_ActivatePress.GetStateDown(SteamVR_Input_Sources.Any))
        {
            //Start line, instantiate a new LineRenderer (function)
            lineObject = new GameObject();
            lineObject.name = "Line";
            lineObject.transform.parent = activeGroup.transform;
            activeLine = lineObject.AddComponent<LineRenderer>() as LineRenderer;
            activeLine.material = new Material(Shader.Find("Sprites/Default"));
            activeLine.widthMultiplier = 0.01f;
            activeLine.useWorldSpace = false;
            activeLine.startColor = lineColor;
            activeLine.endColor = lineColor;

            positions = new List<Vector3>();
            Vector3 newPos = pointer.position;
            positions.Add(newPos);
            activeLine.positionCount = positions.Count;
            activeLine.SetPositions(positions.ToArray());
            prevPos = newPos;
        }

        if (m_ActivatePress.GetState(SteamVR_Input_Sources.Any))
        {
            //While button is held down, create the line accordingly
            if((pointer.position-prevPos).magnitude > maxMagnitude)
            {
                Vector3 newPos = pointer.position;
                positions.Add(newPos);
                activeLine.positionCount = positions.Count;
                activeLine.SetPositions(positions.ToArray());
                prevPos = newPos;
            }
        }

        if (m_ActivatePress.GetStateUp(SteamVR_Input_Sources.Any))
        {
            lineObject = null;
            activeLine = null;
            positions = null;
        }
    }

    public void deleteLines()
    {
        foreach (GameObject o in groups)
        {
            Destroy(o);
        }

        foreach(GroupController g in groupControllers)
        {
            Destroy(g.gameObject);
        }

        numGroups = 0;
        groupControllers = new List<GroupController>();
        groups = new List<GameObject>();
        startNewGroup();
    }

    public void huePicker(float hue)
    {
        lineColor = Color.HSVToRGB(hue, 1, 1);
    }

    public void startNewGroup()
    {
        //Create Group Object
        GameObject groupObject = new GameObject();
        groupObject.name = "Group " + numGroups;
        groupObject.transform.parent = this.transform;
        groups.Add(groupObject);

        //Create Controller Panel for this group
        GameObject lineController = Instantiate(lineControllerPrefab);
        lineController.transform.SetParent(drawingCanvas.transform);
        lineController.transform.localPosition = new Vector3(0f, -0.425f -numGroups * 0.335f, 0f);
        lineController.transform.localScale = new Vector3(1f, 1f, 1f);
        lineController.transform.localRotation = new Quaternion();
        groupControllers.Add(lineController.GetComponent(typeof(GroupController)) as GroupController);
        groupControllers[groupControllers.Count - 1].maxFrame = sim.maxFrame;
        numGroups++;
        activeGroup = groupObject;
    }

    public void changeFrame(float num)
    { 
        for(int i = 0; i<groupControllers.Count; i++)
        {
            if(groups[i]!=null) groups[i].SetActive(groupControllers[i].activity((int)num));
        }
    }
}
