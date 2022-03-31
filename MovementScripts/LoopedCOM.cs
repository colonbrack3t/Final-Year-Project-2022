using UnityEngine;
using System;

public class LoopedCOM : LoopedObjects
{
    // Headset transform
    [SerializeField] protected private Transform head;
    //overall sensitivity of sway
    [SerializeField] private float sensitivity = 1;
    //individual sensitivity of head and board
    [SerializeField] private float head_modifier = 20f, board_modifier = 0.5f, LPF_Threshold = 0.5f;
    // location of user's ankle
    [SerializeField] private Transform ankleJoint;
    //Balance board sensor object, has realtime sensor values
    [SerializeField] private BalanceBoardSensor bbs;
    public bool enableSway = false;
    Vector2 last_board_centre = new Vector2(0, 0);
    Vector3 last_head_movement = new Vector3(0, 0, 0);
    protected Vector2 BoardReading(){

        if (!enableSway ) return new Vector2(0,0);
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
        if (f1 + f2 + b1 + b2 < 20) return new Vector2(0,0);
        
        /*
        f1 /= bbs.weight;
        f2 /= bbs.weight;
        b1 /= bbs.weight;
        b2 /= bbs.weight;
        */
        double topright_backright = f1 / (f1 + b1);
        double topleft_backleft = f2 / (f2 + b2);
        double topright_topleft = f1 / (f1 + f2);
        double backright_backleft = b1 / (b1 + b2);

        double centre_x = (topright_topleft + backright_backleft)/2;
        double centre_z = (topright_backright + topleft_backleft)/2;
        if (Double.IsNaN(centre_x)) centre_x = 0;
        if (Double.IsNaN(centre_z)) centre_z = 0;
        
        Vector2 COM = new Vector2((float)-centre_x , (float)centre_z);
        
        Debug.Log(COM);
        COM *= board_modifier * sensitivity;
        
        Vector2 COM_displacement = COM - last_board_centre;
        last_board_centre = COM;
        Debug.Log(COM_displacement);
        COM_displacement = LPF_Vector2(COM_displacement);
        return COM_displacement;
    }
    // LPF based on total vector mag
    Vector2 LPF_Vector2(Vector2 c)
    {
        if (c.magnitude > LPF_Threshold * LPF_Threshold)
        {   
            /*Vector3 unit = c / c.magnitude;
            Vector3 excess = (unit * LPF_Threshold);
            return (unit * LPF_Threshold) + (excess * 0.4f );*/
            return (c / c.magnitude) * LPF_Threshold;
        }
        return c;
    }
    protected Vector3 HeadReading()
    {
        Vector3 change_in_head = head.position - last_head_movement;
        last_head_movement = head.position;
        if (!enableSway ) return new Vector3(0,0,0);
        Vector3 modchange = change_in_head * head_modifier * sensitivity;
        
        return modchange;

    }

}