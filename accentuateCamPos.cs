using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class accentuateCamPos : MonoBehaviour
{
    public Transform camera;
    [SerializeField] float mod = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = mod * new Vector3(camera.localPosition.x*camera.localPosition.x , 0 , camera.localPosition.z*camera.localPosition.z);
    }
}
