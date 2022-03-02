using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 public class LoopedBoardReader : LoopedObject
{
    // Headset transform
    [SerializeField] private Transform head;
    //overall sensitivity of sway
    [SerializeField] private float sensitivity = 1, maxSpeed = 2.2f;
    //individual sensitivity of head and board
    [SerializeField] private float head_modifier = 20f, board_modifier = 0.5f;
    // location of user's ankle
    [SerializeField] private Transform ankleJoint;
    //Balance board sensor object, has realtime sensor values
    [SerializeField] private BalanceBoardSensor bbs;
    public bool enableSway = false;
    protected float BoardReading(){
        if (!enableSway) return 0;
        float displacement;
        
        // sum readings of front 2 sensors
        double top_sensor = bbs.rwTopLeft + bbs.rwTopRight;
        // sum readings of back 2 sensors
        double btm_sensor = bbs.rwBottomLeft + bbs.rwBottomRight;
        // measure difference between front and back sensors - this should correlate to the user leaning forward vs back
        //displacement is normalised by total weight
        displacement =(float) ((top_sensor - btm_sensor)/bbs.weight);
        Debug.Log(displacement + " , weight: " + bbs.weight + " , top - btm " + (top_sensor - btm_sensor));
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


        return displacement;
    }
   }