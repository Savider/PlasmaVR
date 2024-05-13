using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    // Start is called before the first frame update

    public Material mat = null;

    public void Start()
    {
        mat.color = new Color(1f, 1f, 1f);
    }

    public void colorSwap(bool state)
    {
        if (state)
        {
            //mat.color = new Color(1f, 1f, 1f);
            StartCoroutine(GoLight());
        }
        else
        {
            //mat.color = new Color(0f, 0f, 0f);
            StartCoroutine(GoDark());
        }
    }

    IEnumerator GoDark()
    {
        Color c = new Color(1f, 1f, 1f);
        for (float alpha = 1f; alpha >= 0; alpha -= 0.01f)
        {
            c.r = alpha;
            c.g = alpha;
            c.b = alpha;
            mat.color = c;
            yield return new WaitForSeconds(.01f);
        }
    }

    IEnumerator GoLight()
    {
        Color c = new Color(0f, 0f, 0f);
        for (float alpha = 0f; alpha <= 1; alpha += 0.01f)
        {
            c.r = alpha;
            c.g = alpha;
            c.b = alpha;
            mat.color = c;
            yield return new WaitForSeconds(.01f);
        }
    }
}
