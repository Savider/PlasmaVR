using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class NavigationController : MonoBehaviour
{
    public GameObject cameraRig = null;
    public GameObject controller = null;
    public GameObject revIndicator = null;
    public GameObject simulation = null;
    public GameObject renderContainer = null;
    public GameObject handContainer = null;

    private Material indMat = null;

    public float m_Sensitivity = 0.1f;
    public float m_MaxSpeed = 1.0f;

    public SteamVR_Action_Boolean m_ActivatePress = null;
    public SteamVR_Action_Vector2 m_MoveValue = null;
    public SteamVR_Action_Boolean m_RotatePress = null;
    public SteamVR_Action_Boolean m_SprintPress = null;

    private float m_Speed = 0.0f;

    private Vector3 initPos;
    private Quaternion initRot;
    private Vector3 initScale;

    public bool revolution = false;
    private SteamVR_RenderModel controllerModel = null;

    private void Start()
    {
        initPos = renderContainer.transform.position;
        initRot = renderContainer.transform.rotation;
        initScale = renderContainer.transform.localScale;

        indMat = revIndicator.GetComponent<MeshRenderer>().material;
        controllerModel = controller.GetComponentInChildren<SteamVR_RenderModel>();
    }

    private void Update()
    {
        if (revolution)
        {
            RevolutionControl();
        }
        FreeMovement();
    }

    private void FreeMovement()
    {
        Vector3 orientationEuler = new Vector3(0, transform.eulerAngles.y, 0);
        Quaternion orientation = Quaternion.Euler(orientationEuler);
        Vector3 movement = Vector3.zero;

        if (m_RotatePress.state)
        {
            cameraRig.transform.RotateAround(cameraRig.transform.position, Vector3.up, Time.deltaTime * 90f * m_MoveValue.axis.x);
            movement = cameraRig.transform.up * Time.deltaTime * m_MoveValue.axis.y;
        }
        else
        {
            m_Speed += m_Sensitivity;
            m_Speed = Mathf.Clamp(m_Speed, -m_MaxSpeed, m_MaxSpeed);

            Vector3 dirVec = Vector3.forward * m_MoveValue.axis.y + Vector3.right * m_MoveValue.axis.x;
            if (m_SprintPress.state)
            {
                movement += (m_Speed* 2 * dirVec) * Time.deltaTime;
            }
            else
            {
                movement += (m_Speed * dirVec) * Time.deltaTime;
            }
        }
        cameraRig.transform.Translate(movement);
    }

    private void RevolutionControl()
    {
        handContainer.transform.rotation = controller.transform.rotation;
        if (m_ActivatePress.GetStateDown(SteamVR_Input_Sources.Any))
        {
            //Indicator retains old rotation (indicator - controller)
            //Container gets new rotation (controller)
            Quaternion rot = revIndicator.transform.rotation;
            this.transform.rotation = controller.transform.rotation;
            revIndicator.transform.rotation = rot;
            indMat.color = new Color(10f, 10f, 10f);
            controllerModel.SetMeshRendererState(false);
        }
        if (m_ActivatePress.GetState(SteamVR_Input_Sources.Any))
        {
            //Do the thing

            //See rotation change in controller
            this.transform.rotation = controller.transform.rotation;
            //this.transform.position = controller.transform.position;
            //Apply that change to the indicator and rotator
            simulation.transform.rotation = revIndicator.transform.rotation;
        }

        if (m_ActivatePress.GetStateUp(SteamVR_Input_Sources.Any))
        {
            revIndicator.transform.localRotation = revIndicator.transform.rotation;
            this.transform.rotation = new Quaternion();
            indMat.color = new Color(0.5f, 0.5f, 0.5f);
        }
        revIndicator.transform.position = controller.transform.position; //+ 0.5f * cameraRig.transform.forward - 0.5f * cameraRig.transform.up;
    }

    private void MovementUpdate()
    {
        renderContainer.transform.position = cameraRig.transform.position + new Vector3(0f, 1.5f, 1f);
        renderContainer.transform.localScale = initScale * 0.2f;
    }

    private void MovementReset()
    {
        renderContainer.transform.position = initPos;
        renderContainer.transform.localScale = initScale;
    }
    
    public void RevolutionActivate(bool activation)
    {
        controllerModel.SetMeshRendererState(!activation);
        revolution = activation;
        revIndicator.SetActive(activation);
    }

    public void MovementActivate(bool activation)
    {
        if (activation)
        {
            MovementUpdate();
        }
        else
        {
            MovementReset();
        }
    }
    /*private void ToggleContainer()
    {
        if (zoomed)
        {
            renderContainer.transform.position = standardRenderPos.position;
            renderContainer.transform.lossyScale = standardRenderPos.lossyScale;
            
        }
        else
        {
            renderContainer.transform.position = cameraRig.transform.position;
            renderContainer.transform.localScale = renderContainer.transform.localScale * 0.1f;
            zoomed = true;
        }
    }*/
}
