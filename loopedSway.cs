using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loopedSway : MonoBehaviour
{
    [SerializeField] private float start = -100, end = 100, direction = 1;
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
    private Vector3 start_pos, end_pos;
    public bool enableSway = false;
    void Start()
    {
        start_pos = new Vector3(0, 0, start);
        end_pos = new Vector3(0, 0, end);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z * direction > end * direction ){
            transform.position = start_pos;
        }
        if (transform.position.z * direction < start * direction ){
            transform.position = end_pos;
        }
        Sway();
    }
    void Sway(){
        float displacement;
        
        // sum readings of front 2 sensors
        double top_sensor = bbs.rwTopLeft + bbs.rwTopRight;
        // sum readings of back 2 sensors
        double btm_sensor = bbs.rwBottomLeft + bbs.rwBottomRight;
        //calculate total weight of person 
        double weight = top_sensor + btm_sensor;
        // measure difference between front and back sensors - this should correlate to the user leaning forward vs back
        //displacement is normalised by total weight
        displacement =(float) ((top_sensor - btm_sensor)/bbs.weight);
        Debug.Log(displacement + " , weight: " + bbs.weight);
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
        //apply max movement constraint
        displacement =Mathf.Min(displacement, maxSpeed);
        
        //reduce to per frame speed
         displacement *= Time.deltaTime;

        //moce transform position
        transform.position += new Vector3(0,0,displacement);

    }
    
}
