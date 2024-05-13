using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Simulation : MonoBehaviour
{
    protected bool toUpdate = false;


    public abstract void displayFrame(int frame);

    public abstract bool startSim(int numFrames, Vector3 dims, string simPath);

    protected abstract void loadSimulation();

}
