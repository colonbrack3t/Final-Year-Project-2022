using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class accentuate_camera_movements : MonoBehaviour
{   public Transform camera;
    public float accentuation_factor;
    Vector3 prev = new Vector3(0,0,0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        this.transform.eulerAngles = camera.localEulerAngles*accentuation_factor;
    }
}
