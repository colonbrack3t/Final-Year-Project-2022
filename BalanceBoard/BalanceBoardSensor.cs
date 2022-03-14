using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using System.IO;
public class BalanceBoardSensor : MonoBehaviour
{

    // sensor weights
    [SerializeField] public double rwTopLeft = 0f;
    [SerializeField] public double rwTopRight = 0f;
    [SerializeField] public double rwBottomLeft = 0f;
    [SerializeField] public double rwBottomRight = 0f;

    //latency- only updated if client is in latency mode
    [SerializeField] public double latency = 0f;
    [SerializeField] public double avglatency = 0f;
    [SerializeField] public double minlatency = float.MaxValue;
    [SerializeField] public double maxlatency = 0f;
    [SerializeField] public int latency_count = 0;
    private List<double> latencies = new List<double>();

    //records average delay between samples
    [SerializeField] public double avg_ms_per_reading = 0;
    [SerializeField] public double prev_ms = 0;
    
    
    //weight average computation attributes
    private bool calculateWeight = false;
    public double weight = 0;
    private int num_weight_readings = 0;
    //Text visual component
        UDPSocketUnity server = new UDPSocketUnity();
    // Start is called before the first frame update
    void Start()
    {
      //add onclick handlers for various UI
 

        //Initiate UDP Server
        server.Server("127.0.0.1", 27335, this);
    }
    //record when mouse was clicked
    
    void FixedUpdate()
    {
      //if record_headset flag is true, record vertical height of headset
      if(record_headset)
             RecordHeadsetVertical();
        if (calculateWeight){
            num_weight_readings++;
            double weight_reading = rwTopRight + rwTopLeft + rwBottomRight + rwBottomLeft;
            weight = (weight + weight_reading)/num_weight_readings;
        }
    }
    
    //public wrapper to add method of latency list
    public void AddLatency(double latency)
    {
        latencies.Add(latency);
    }



}
