using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveGraphRender : MonoBehaviour
{
    [SerializeField] BalanceBoardSensor bbs;
    [SerializeField] bool start;
    [SerializeField] Sensor s;
    [SerializeField] LineRenderer lr;
    float time = 0f;


    // Update is called once per frame
    void Update()
    {
        //Simple script that displays balance board readings using a line renderer - useful for debugging
        if (start)
        {
            lr.positionCount += 1;
            time += Time.deltaTime;
            float z =0;
            switch (s)
            {
                case Sensor.TopLeft:
                    z = (float)bbs.rwTopLeft;
                    break;
                case Sensor.TopRight:
                    z = (float)bbs.rwTopRight;
                    break;
                case Sensor.BottomLeft:
                    z = (float)bbs.rwBottomLeft;
                    break;
                case Sensor.BottomRight:
                    z = (float)bbs.rwBottomRight;
                    break;
            }
            lr.SetPosition(lr.positionCount - 1, new Vector3(time, -10, z));
        }
    }
    enum Sensor { TopLeft, TopRight, BottomLeft, BottomRight }
}
