using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandAnim : MonoBehaviour
{

    public SteamVR_Action_Boolean m_ActivatePress = null;
    private Animator anim = null;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }



    // Update is called once per frame
    void Update()
    {
        if (m_ActivatePress.state)
        {
            anim.Play("SphereGrab");
        }
    }
}
