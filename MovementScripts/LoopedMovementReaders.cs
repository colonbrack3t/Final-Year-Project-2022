using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
 public class LoopedMovementReaders : LoopedObjects
{
    // Headset transform
    [SerializeField] private Transform head;
    //overall sensitivity of sway
    [SerializeField] private float sensitivity = 1;
    //individual sensitivity of head and board
    [SerializeField] private float head_modifier = 20f, board_modifier = 0.5f;
    // location of user's ankle
    [SerializeField] private Transform ankleJoint;
    //Balance board sensor object, has realtime sensor values
    [SerializeField] private BalanceBoardSensor bbs;
    public bool enableSway = false;
    double last_displacement = 0.5;
    Vector3 last_head_movement = new Vector3(0,0,0); 
    protected float BoardReading(){
        if (!enableSway ) return 0;
        
        double displacement;
        /* old implementation
        // sum readings of front 2 sensors
        double top_sensor = bbs.rwTopLeft + bbs.rwTopRight;
        // sum readings of back 2 sensors
        double btm_sensor = bbs.rwBottomLeft + bbs.rwBottomRight;
        // measure difference between front and back sensors - this should correlate to the user leaning forward vs back
        //displacement is normalised by total weight
        displacement =(float) ((top_sensor - btm_sensor)/bbs.weight);
        */
        double f1 = bbs.rwTopLeft, f2 = bbs.rwTopRight, b1 = bbs.rwBottomLeft, b2 = bbs.rwBottomRight;
        if (f1 + f2 + b1 + b2 < 20) return 0;
  
           double right_ratio = f1 / (f1 + b1);
        double left_ratio = f2 / (f2 + b2);

        displacement = (left_ratio + right_ratio)/2f;
        Debug.Log(displacement + " , weight: " + bbs.weight );
        // multiply measures by weights
        displacement *= board_modifier;
        // horizontal 2d distance between headset and feet
        float head_dist = (head.position.z - ankleJoint.position.z);
        // multiply measures by weights
        head_dist *= head_modifier;



        // apply head dist mod
        displacement += head_dist;
        //apply sensitivity parameter
        displacement *= sensitivity;
        if (Double.IsNaN(displacement)) return 0;
        float change_in_mass_centre = (float) (last_displacement - displacement);
        last_displacement = displacement;
        return change_in_mass_centre;
    }
   }