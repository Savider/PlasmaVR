using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupController : MonoBehaviour
{
    public GameObject group = null;

    private MinimumSlider minSlider = null;
    private MaximumSlider maxSlider = null;

    public Text minText = null;
    public Text maxText = null;

    public int maxFrame = 0;
    // Start is called before the first frame update
    void Start()
    {
        minSlider = GetComponentInChildren(typeof(MinimumSlider)) as MinimumSlider;
        maxSlider = GetComponentInChildren(typeof(MaximumSlider)) as MaximumSlider;

        minSlider.other = maxSlider;
        maxSlider.other = minSlider;

        minSlider.wholeNumbers = true;
        maxSlider.wholeNumbers = true;

        minSlider.maxValue = maxFrame;
        maxSlider.maxValue = maxFrame;

        minSlider.value = 0;
        maxSlider.value = maxFrame;
        updateMinText(minSlider.value);
        updateMaxText(maxSlider.value);
        //this.gameObject.SetActive(false);
    }

    public void updateMinText(float value)
    {
        minText.text = ((int)value).ToString();
    }

    public void updateMaxText(float value)
    {
        maxText.text = ((int)value).ToString();
    }

    public void updateMaxFrame(int value)
    {
        maxFrame = value;
        updateMaxText((float)value);
        minSlider.maxValue = maxFrame;
        maxSlider.maxValue = maxFrame;
    }

    public bool activity(int value)
    {
        if(value>=(int)minSlider.value && value <= (int)maxSlider.value)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
