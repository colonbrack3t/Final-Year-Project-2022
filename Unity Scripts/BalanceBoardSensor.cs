using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
public class BalanceBoardSensor : MonoBehaviour
{
    
    public float adjustedTL = 0f;
    public float adjustedTR = 0f;
    public float adjustedBL = 0f;
    public float adjustedBR = 0f;
    public float rwTopLeft = 0f;
    public float rwTopRight = 0f;
    public float rwBottomLeft = 0f;
    public float rwBottomRight = 0f;
   
    UDPSocketUnity server = new UDPSocketUnity();

    // Start is called before the first frame update
    void Start()
    {
        server.Server("127.0.0.1", 27002, this);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

}