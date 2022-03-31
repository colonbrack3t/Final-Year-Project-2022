using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMovingImages : MonoBehaviour
{
    public GameObject image, VRCanvas;
    // Start is called before the first frame update
    void Start()
    {
         for(int i = 0; i < 3000; i ++){
            GameObject newImage = Instantiate(image,VRCanvas.transform);
            newImage.transform.position = new Vector3(0,0,0);
            RectTransform rt = newImage.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(Random.Range(-200,200), Random.Range(-200,200));
            rt.localPosition = new Vector3(rt.localPosition.x, rt.localPosition.y,0);
            newImage.GetComponent<MoveImage>().direction = Random.Range(0,2) > 0? 1f: -1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
