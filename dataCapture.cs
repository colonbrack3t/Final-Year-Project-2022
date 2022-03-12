using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using System.IO;

public class dataCapture : MonoBehaviour
{
    [SerializeField] Transform Head;
    [SerializeField] BalanceBoardSensor bbs;
    [SerializeField] COMSway COMSway;
    bool recording = false;
    double starttime;
    List<double[]> board_sensors = new List<double[]>();

    List<float> sinwave_vals = new List<float>(), time_stamp = new List<float>();
    List<Vector3> head_positions = new List<Vector3>();

    // Start is called before the first frame update
    void ToggleRecordData(){
        if(recording){
            recording = false;
            
            // write all stored data
        }else{ starttime = DateTime.Now.TimeOfDay.TotalMilliseconds;recording = true;}
    }

    // Update is called once per frame
    void Update()
    {
        if(recording){
            // record bbs
            board_sensors.Add(new double[]{bbs.rwTopLeft, bbs.rwBottomRight, bbs.rwBottomLeft, bbs.rwBottomRight});
            //record sinwave
            sinwave_vals.Add(COMSway.sinwave_position);
            //record head
            head_positions.Add(Head.position);

            //record timestamp
            //time_stamp.Add(DateTime.Now.TimeOfDay.TotalMilliseconds- starttime);
        }
    }
}
