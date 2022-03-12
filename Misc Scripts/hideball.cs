using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hideball : MonoBehaviour
{
    //location of camera
    public Transform camera;
    [SerializeField] float n;

    // Update is called once per frame
    void Update()
    {
        // set visibility to false if near camera, else set to true
        GetComponent<Renderer>().enabled =  Vector3.Distance(camera.position, transform.position) > n;
    }
}
