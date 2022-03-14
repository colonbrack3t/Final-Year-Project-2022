using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;
public class PendulumMovement : MonoBehaviour
{

    // headset camera
    [SerializeField] Transform camera;
    //tracking of previous position
    Vector3 prev_cam_pos = new Vector3(0,0,0);
    //location of pivot 
    [SerializeField] Vector3 pivot = new Vector3(0,0,0);


    //sensitivity - 1 = 100% extra rotation (ie doubling percieved motion)
    [SerializeField] public float sensitivity = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        //instantiate previous camera position
        prev_cam_pos = camera.position;
    }
    
    void FixedUpdate()
    {  
        //Construct vectors for new and last position
        Vector3 head_v = pivot - camera.position, prev_v = pivot - prev_cam_pos;
        
        //find perpendicular axis
        Vector3 axis = Vector3.Cross(head_v, prev_v);
        //find amount of rotation relative to axis
        float rotation = Vector3.SignedAngle(prev_v, head_v, axis);
        //rotate object around axis, relative to head rotation and sensitivity
        transform.RotateAround(transform.position, axis, rotation * sensitivity);
        //assign previous head position to the new rotated head position
        prev_cam_pos = camera.position;
    }
}
