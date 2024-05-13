using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SimController : MonoBehaviour
{

    public Transform controller = null;
    public Transform indicator = null;
    public Transform sim = null;
    public Transform rig = null;

    public SteamVR_Action_Boolean pressed = null;
    public SteamVR_Input_Sources source = SteamVR_Input_Sources.RightHand;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (pressed.GetStateDown(SteamVR_Input_Sources.Any))
        {
            //Indicator retains old rotation (indicator - controller)
            //Container gets new rotation (controller)
            Quaternion rot = indicator.rotation;
            this.transform.rotation = controller.rotation;
            indicator.rotation = rot;
        }
        if (pressed.GetState(SteamVR_Input_Sources.Any))
        {
            //Do the thing

            //See rotation change in controller
            this.transform.rotation = controller.rotation;
            //this.transform.position = controller.transform.position;
            //Apply that change to the indicator and rotator
            indicator.position = controller.position + 0.5f * rig.forward - 0.5f * rig.up;
            sim.rotation = indicator.rotation;
        }

        if (pressed.GetStateUp(SteamVR_Input_Sources.Any))
        {
            indicator.localRotation = indicator.rotation;
            this.transform.rotation = new Quaternion();
            indicator.position = new Vector3(-10, -10, -10);
        }
    }

    private void RevolutionControl()
    {
        if (pressed.GetStateDown(SteamVR_Input_Sources.Any))
        {
            //Indicator retains old rotation (indicator - controller)
            //Container gets new rotation (controller)
            Quaternion rot = indicator.rotation;
            this.transform.rotation = controller.rotation;
            indicator.rotation = rot;
        }
        if (pressed.GetState(SteamVR_Input_Sources.Any))
        {
            //Do the thing

            //See rotation change in controller
            this.transform.rotation = controller.rotation;
            //this.transform.position = controller.transform.position;
            //Apply that change to the indicator and rotator
            indicator.position = controller.position + 0.5f * rig.forward - 0.5f * rig.up;
            sim.rotation = indicator.rotation;
        }

        if (pressed.GetStateUp(SteamVR_Input_Sources.Any))
        {
            indicator.localRotation = indicator.rotation;
            this.transform.rotation = new Quaternion();
            indicator.position = new Vector3(-10, -10, -10);
        }
    }
}
