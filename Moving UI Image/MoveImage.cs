using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveImage : MonoBehaviour
{
    public float max, min, direction = 1, speed;
    RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rectTransform.anchoredPosition.y >= max){
            direction = -1;
        }
        if(rectTransform.anchoredPosition.y <= min){
            direction = 1;
        }
        rectTransform.anchoredPosition += new Vector2(0,speed*direction);
    }
}
