using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public MeshRenderer mesh = null;

    Material mat = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mat = mesh.material;
        mat.SetColor("_EmissionColor", Color.red);
        mesh.material = mat;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mat = mesh.material;
        mat.SetColor("_EmissionColor", Color.white);
        mesh.material = mat;
    }
}
