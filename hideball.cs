using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hideball : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform camera;
    public float n;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Renderer>().enabled =  Vector3.Distance(camera.position, transform.position) > n;
    }
}
