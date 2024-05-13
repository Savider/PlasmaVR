using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCam : MonoBehaviour
{
    public Transform cam;
    public bool stayVertical = false;

    private Vector3 pos = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        pos = cam.position;
    }

    // Update is called once per frame
    void Update()
    {
        pos = cam.position;
        if (stayVertical)
        {
            pos.y = this.transform.position.y; 
        }
        this.transform.LookAt(this.transform.position - (pos - this.transform.position));
    }
}
