using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSizer : MonoBehaviour
{

    public Slider sliderX;
    public Slider sliderY;
    public Slider sliderZ;

    public Transform quadX;
    public Transform quadY;
    public Transform quadZ;

    public Vector3 dims = new Vector3(1.44f, 1.44f, 1.44f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setup(Vector3 newdims)
    {
        dims = newdims;
        positionX();
        positionY();
        positionZ();
    }

    void positionX()
    {
        sliderX.transform.localPosition = new Vector3(0f, -dims.y, -dims.z);
        RectTransform rect = sliderX.gameObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(dims.x * 200f, 20f);

        quadX.localPosition = new Vector3(0f, -dims.y * 100f, -dims.z * 100f);
        Quaternion rot = quadX.localRotation;
        rot.eulerAngles = new Vector3(0.0f, -90f, 0.0f);
        quadX.localRotation = rot;
        quadX.localScale = new Vector3(dims.z, dims.y, 0) * 200f;
    }

    void positionY()
    {
        sliderY.transform.localPosition = new Vector3(-dims.x, 0f, -dims.z);
        RectTransform rect = sliderY.gameObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(dims.y * 200f, 20f);

        quadY.localPosition = new Vector3(0f, -dims.x * 100f, dims.z * 100f);
        Quaternion rot = quadY.localRotation;
        rot.eulerAngles = new Vector3(0.0f, -90f, 0.0f);
        quadY.localRotation = rot;
        quadY.localScale = new Vector3(dims.z, dims.x, 0) * 200f;
    }

    void positionZ()
    {
        sliderZ.transform.localPosition = new Vector3(-dims.x, -dims.y, 0f);
        RectTransform rect = sliderZ.gameObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(dims.z * 200f, 20f);

        quadZ.localPosition = new Vector3(0f, -dims.y * 100f, dims.x * 100f);
        Quaternion rot = quadZ.localRotation;
        rot.eulerAngles = new Vector3(0.0f, -90f, 0.0f);
        quadZ.localRotation = rot;
        quadZ.localScale = new Vector3(dims.x, dims.y, 0) * 200f;
    }
}
