using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class Pointer : MonoBehaviour
{
    public float defaultLength = 5.0f;
    public GameObject point;

    public Camera Camera { get; private set; } = null;

    public VRInputManager inputManager;

    private LineRenderer lineRenderer = null;

    public SteamVR_Action_Vector2 m_LengthAdjustment = null;

    private void Awake()
    {
        Camera = GetComponent<Camera>();
        Camera.enabled = false;
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        adjustLength();
        UpdateLine();
    }

    private void UpdateLine() {

        PointerEventData data = inputManager.Data;
        float targetLength = data.pointerCurrentRaycast.distance == 0 ? defaultLength : data.pointerCurrentRaycast.distance;
        if (targetLength > defaultLength) targetLength = defaultLength;

        RaycastHit hit = CreateRaycast(targetLength);

        Vector3 endPosition = transform.position + (transform.forward * targetLength);

        if(hit.collider != null)
        {
            endPosition = hit.point;
        }
        point.transform.position = endPosition;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);
    }

    private void adjustLength()
    {
        defaultLength = defaultLength + m_LengthAdjustment.GetAxis(SteamVR_Input_Sources.Any).y * Time.deltaTime * 2f;
        if (defaultLength < 0f)
        {
            defaultLength = 0f;
        }
        if(defaultLength > 5.0f)
        {
            defaultLength = 5.0f;
        }
    }

    private RaycastHit CreateRaycast(float length)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, defaultLength);

        return hit;
    }
}
