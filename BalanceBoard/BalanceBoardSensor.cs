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
    public double rwTopLeft = 0f;
    public double rwTopRight = 0f;
    public double rwBottomLeft = 0f;
    public double rwBottomRight = 0f;

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
        //Initiate UDP Server
        server.Server("127.0.0.1", 27338, this);
    }
    //public wrapper for debug - used in UDP server
    public void display(string s){Debug.Log(s);}
    
    //public wrapper to add method of latency list
    public void AddLatency(double latency)
    {
        latencies.Add(latency);
    }



}
