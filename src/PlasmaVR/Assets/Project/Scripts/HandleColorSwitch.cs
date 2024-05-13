using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleColorSwitch : MonoBehaviour
{
    Image handle = null;

    // Start is called before the first frame update
    void Start()
    {
        handle = GetComponent<Image>();
        handle.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void huePicker(float hue)
    {
        handle.color = Color.HSVToRGB(hue, 1, 1);
    }
}
