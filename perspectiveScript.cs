using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class perspectiveScript : MonoBehaviour
{
    public float distance;     
    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //rectTransform.anchorMin = new Vector2(, )
        rectTransform.anchoredPosition = Vector3.zero;
    }
}
