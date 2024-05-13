using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineController : MonoBehaviour
{

    public SimulationController simulationController = null;
    public Slider slider = null;
    public Text text = null;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponentInChildren<Slider>();
        text = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        updateSlider();
        updateText();
    }

    void updateSlider()
    {
        if(simulationController.sims.Count > 0)
        {
            slider.value = simulationController.currFrame;
        }
    }

    void updateText()
    {
        if (simulationController.sims.Count > 0)
        {
            text.text = simulationController.currFrame.ToString("0.0") + "/" + simulationController.maxFrame.ToString("0.0") + " 1/ωp";
        }
    }
}
