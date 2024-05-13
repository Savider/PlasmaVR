using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string displayedText = "";
    public GameObject tooltip = null;
    public Vector3 offset = Vector3.zero;
    GameObject spawnedTooltip = null;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (spawnedTooltip != null)
        {
            spawnedTooltip.transform.position = this.transform.position + offset;
            spawnedTooltip.transform.rotation = this.transform.rotation;
        }
        */
    }

    private void OnDisable()
    {
        Destroy(spawnedTooltip);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        spawnedTooltip = Instantiate(tooltip, new Vector3(0, 0, 0), Quaternion.identity);
        TextMesh text = spawnedTooltip.GetComponent<TextMesh>();
        text.text = displayedText;
        spawnedTooltip.transform.position = this.transform.position + offset;
        spawnedTooltip.transform.rotation = this.transform.rotation;

        spawnedTooltip.transform.parent = this.transform;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(spawnedTooltip);
    }


}
