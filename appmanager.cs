using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class appmanager : MonoBehaviour
{
    [SerializeField] SwayCube[] tunnels;
    [SerializeField] BalanceBoardSensor board;
    [SerializeField] StroopTest stroopTest;
    [SerializeField] Text user_text;
    private double time_for_weight_est = 5f;

    private double time_for_delay = 1f;
    public Stage stage;
    // Start is called before the first frame update
    void Start()
    {
        user_text.text = "Press A on right controller to place board. Press Trigger on right controller to lock board";
    }

    // Update is called once per frame
    void Update()
    {
        switch(stage){
            case Stage.GetAnkleJoint:
                break;
            case Stage.StartGetWeight:
                user_text.text = "Please stay as still as possible on the board, it is now calibrating .."+
                time_for_weight_est;
                board.toggleWeightCalculation();
                stage = Stage.GetWeight;
                break;
            case Stage.GetWeight:
                time_for_weight_est-= Time.deltaTime;
                if (time_for_weight_est < 0)
                    stage = Stage.EndGetWeight;
                user_text.text = "Please stay as still as possible on the board, it is now calibrating .."+
                time_for_weight_est;
                break;
            case Stage.EndGetWeight:
                board.toggleWeightCalculation();
                user_text.text = "Beginning stroop test..";
                stage = Stage.StartStroopTest;
                break;
            case Stage.StartStroopTest:
            //wait 1s then 
            time_for_delay-= Time.deltaTime;
            if (time_for_delay < 0)
                    {
                        stage = Stage.StroopTest;
                        stroopTest.test_running = true;
                        stroopTest.GenerateStroopTest();
                        foreach (var t in tunnels)
                        {
                            t.enableSway = true;
                        }
                        }
            

                break;
            case Stage.StroopTest:
            // enable sway cubes
            // start stroop test

                break;
                
        }
    }

    public enum Stage{
        GetAnkleJoint, StartGetWeight, GetWeight, EndGetWeight, StartStroopTest,StroopTest

    }
}
