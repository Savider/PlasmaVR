using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class UIPlacer : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject controller = null;
    public GameObject UI = null;
    public Vector3 UIOffset = new Vector3(0, 0, 0);

    private bool open = false;

    public SteamVR_Action_Boolean pressed = null;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pressed.GetStateDown(SteamVR_Input_Sources.Any))
        {
            toggleMenu();
        }
        if (open)
        {
            UI.transform.position = controller.transform.position + UI.transform.rotation*UIOffset;
        }
    }

    void toggleMenu()
    {
        open = !open;
        UI.SetActive(open);
    }
}
