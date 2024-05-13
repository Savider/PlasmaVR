using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneSliderSetup : MonoBehaviour
{
    public Slider sliderX = null;
    public Slider sliderY = null;
    public Slider sliderZ = null;

    public RectTransform sliderXrect = null;
    public RectTransform sliderYrect = null;
    public RectTransform sliderZrect = null;

    public RectTransform quadX = null;
    public RectTransform quadY = null;
    public RectTransform quadZ = null;

    public void Start()
    {
        setup(new Vector3(14.4f/20f, 14.4f/20f, 14.4f/20f), new Vector3Int(288, 288, 288));
    }

    public void setup(Vector3 dims, Vector3Int res)
    {
        sliderXrect.anchoredPosition3D = new Vector3(0f, -dims.y, -dims.z);
        sliderYrect.anchoredPosition3D = new Vector3(-dims.x, 0, -dims.z);
        sliderZrect.anchoredPosition3D = new Vector3(-dims.x, -dims.y, 0);

        sliderXrect.sizeDelta = new Vector2(dims.x * 20f, 20f);
        sliderYrect.sizeDelta = new Vector2(dims.y * 20f, 20f);
        sliderZrect.sizeDelta = new Vector2(dims.z * 20f, 20f);        

    }
}
