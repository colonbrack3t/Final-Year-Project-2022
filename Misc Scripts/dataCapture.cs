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

    [SerializeField] PendulumMovement pendulum;
    [SerializeField] float control_duration, test_duration, test_sensitivity, aftermath_duration;
    bool recording = false;
    float t;
    List<double[]>  board_sensors   = new List<double[]>();
    List<float>     sinwave_vals    = new List<float>();
    List<Vector3>   head_positions  = new List<Vector3>();
    public Trial_Stages stage;

    public enum Trial_Stages { Control_Stage, Test_Stage, Aftermath_Stage }
    void BeginTrial()
    {
        recording = true;
        stage = Trial_Stages.Control_Stage;
        pendulum.sensitivity = 0;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (recording)
        {
            switch (stage)
            {
                case Trial_Stages.Control_Stage:
                    if (t < control_duration)
                    {
                        t += Time.deltaTime;
                    }
                    else
                    {
                        stage = Trial_Stages.Test_Stage;
                        pendulum.sensitivity = test_sensitivity;
                    }
                    break;

                case Trial_Stages.Test_Stage:
                    if (t < control_duration + test_duration)
                    {
                        t += Time.deltaTime;
                    }
                    else
                    {
                        stage = Trial_Stages.Aftermath_Stage;
                        pendulum.sensitivity = 0;
                    }
                    break;

                case Trial_Stages.Aftermath_Stage:
                    if (t < control_duration + test_duration + aftermath_duration)
                    {
                        t += Time.deltaTime;
                    }
                    else
                    {
                        Debug.Log("Fin");
                        recording = false;
                    }
                    break;
            }

            // record bbs
            board_sensors.Add(new double[] { bbs.rwTopLeft, bbs.rwBottomRight, bbs.rwBottomLeft, bbs.rwBottomRight });
            //record head
            head_positions.Add(Head.position);
        }
    }
    void write_data(){
        using (StreamWriter writer = new StreamWriter("Assets/BalanceBoard/mouse.csv"))
        {}
    }
}
