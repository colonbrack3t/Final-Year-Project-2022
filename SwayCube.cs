using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SwayCube : MonoBehaviour
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

    //Starting location of object
    private Vector3 startingPosition;

    public bool enableSway = false;
    // Start is called before the first frame update
    void Start()
    {
        // get initial position
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {if(enableSway){Sway();}
        
    }
    void Sway(){
        // horizontal 2d distance between headset and feet
        float head_dist = (head.position.z - ankleJoint.position.z);
        // sum readings of front 2 sensors
        double top_sensor = bbs.rwTopLeft + bbs.rwTopRight;
        // sum readings of back 2 sensors
        double btm_sensor = bbs.rwBottomLeft + bbs.rwBottomRight;
        //calculate total weight of person 
        double weight = top_sensor + btm_sensor;
        // measure difference between front and back sensors - this should correlate to the user leaning forward vs back
        //displacement is normalised by total weight
        double displacement = (top_sensor - btm_sensor)/bbs.weight;

        // multiply measures by weights
        head_dist *= head_modifier;
        displacement *= board_modifier;
        // sum weighted measures and apply overall sensitivity parameter
        float coordinate_modifier = sensitivity * (float)(head_dist + displacement);
        //moce transform position
        transform.position = startingPosition +  new Vector3(0,0,coordinate_modifier);

    }
}
//TODO: Factor in varying weights
// for weight just divide by users weight
