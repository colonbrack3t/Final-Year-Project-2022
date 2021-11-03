using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
public class BalanceBoardSensor : MonoBehaviour
{
    
    public float oaTopLeft = 0f;
    public float oaTopRight = 0f;
    public float oaBottomLeft = 0f;
    public float oaBottomRight = 0f;
    public float rwWeight;
    UDPSocket server = new UDPSocket();
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