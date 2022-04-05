using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
public class Trial : MonoBehaviour
{
    // text that displays time left
    [SerializeField] Text text;

    //head location
    [SerializeField] Transform Head, rig;

    //balance board
    [SerializeField] BalanceBoardSensor bbs;

    //pendulum script to set sensitivity
    [SerializeField] SwayBaseClass pendulum;

    //duration of each phase
    [SerializeField] public float control_duration, test_duration, aftermath_duration, pause_duration;
    [SerializeField] Text control_display_time, test_display_time, aftermath_display_time, pause_display_time;
    //list of sensitivites for each trial
    [SerializeField] List<float> test_sensitivity = new List<float>();

    // trial counter
    int test_num;
    [SerializeField] int curr_test;

    // flags
    bool recording = false, trial_running = false;

    //timer
    float t;

    //data storage
    List<double>[] board_sensors = new List<double>[4];
    List<Vector3> head_positions = new List<Vector3>();
    List<float>   t_time = new List<float>();

    //keep track of stage in trial
    public Trial_Stages stage;

    //enum for each stage
    public enum Trial_Stages { Control_Stage, Test_Stage, Aftermath_Stage, Pause_Stage }

    //enum to ensure consistency between recording and writing sensor readings
    enum sensor { topleft, topright, bottomleft, bottomright }

    //initiate variables
    async void Start()
    {
        test_num = test_sensitivity.Count;
        curr_test = -1;
        Reset_storage();
    }
    void Reset_storage(){
        for(int i = 0 ; i < 4 ; i++)
            board_sensors[i] = new List<double>();
        head_positions = new List<Vector3>();
        t_time = new List<float>();
    }

    public void Recentre(){
        rig.position = new Vector3(0,0,0);
    } 
    public void Reset_Test(){
        Reset_storage();
        recording = false; 
        trial_running = false;
        curr_test = -1;
        test_num = test_sensitivity.Count;
        text.text = "";
        t = 0;
    }
    //intiate variables for new trial
    public void BeginTrial()
    {
        
        stage = Trial_Stages.Control_Stage;
        pendulum.sensitivity = 0;
        curr_test++;
        
        if(test_sensitivity.Count <= curr_test)
            {Reset_Test();
            return;}
        recording = true;
        trial_running = true;
        text.text = "";
        t = 0;
    }
    // Update is called once per frame. Fixed ensures predictable time between frames. Runs trial

    void Record()
    {
        // record bbs
        board_sensors[(int)sensor.topleft].Add(bbs.rwTopLeft);
        board_sensors[(int)sensor.topright].Add(bbs.rwTopRight);
        board_sensors[(int)sensor.bottomleft].Add(bbs.rwBottomLeft);
        board_sensors[(int)sensor.bottomright].Add(bbs.rwBottomRight);
        //record head
        head_positions.Add(Head.position);
        t_time.Add(t);

    }
    async void FixedUpdate()
    {
        if (trial_running)
        {
            //increment time frame
            t += Time.deltaTime;
            if (recording)
            {
                Record();
            }
            //handle stage and display time
            StageSwitcher();

        }

    }

    //handles stage progression
    void StageSwitcher()
    {
        switch (stage)
        {
            case Trial_Stages.Control_Stage:

                if (t >= control_duration)
                {
                    stage = Trial_Stages.Test_Stage;
                    pendulum.sensitivity = test_sensitivity[curr_test];
                }
                else
                {
                    display_time(control_duration);
                }

                break;

            case Trial_Stages.Test_Stage:

                if (t >= control_duration + test_duration)
                {
                    stage = Trial_Stages.Aftermath_Stage;
                    pendulum.sensitivity = 0;
                }
                else
                {
                    display_time(control_duration + test_duration);
                }

                break;

            case Trial_Stages.Aftermath_Stage:

                if (t >= control_duration + test_duration + aftermath_duration)
                {
                    Debug.Log("Fin");
                    recording = false;
                    stage = Trial_Stages.Pause_Stage;
                    write_data();
                    Reset_storage();
                }
                else
                {
                    display_time(control_duration + test_duration + aftermath_duration);
                }
                break;
            case Trial_Stages.Pause_Stage:
                if (t > control_duration + test_duration + aftermath_duration + pause_duration)
                {
                    trial_running = false; t = 0;
                }
                else
                    display_time(control_duration + test_duration + aftermath_duration + pause_duration);
                break;

        }
    }

    //Creates strings from lists and writes all recorded data to new csv file
    void write_data()
    {
        string header = String.Format("Date: {0}, Trial #: {1}/{7}, Sensitivty: {2}, Control time: {3}, Test time: {4}, Aftermath time: {5}, Pause time : {6}",
                                     DateTime.Now.ToString(), curr_test + 1, test_sensitivity[curr_test], control_duration, test_duration, aftermath_duration, pause_duration, test_num);
        Debug.Log(header);
        string delimiter = ",";
        string topleft = "Top Left," + string.Join(delimiter, board_sensors[(int)sensor.topleft]);
        string topright = "Top Right," + string.Join(delimiter, board_sensors[(int)sensor.topright]);
        string bottomleft = "Bottom Left," + string.Join(delimiter, board_sensors[(int)sensor.bottomleft]);
        string bottomright = "Bottom Right," + string.Join(delimiter, board_sensors[(int)sensor.bottomright]);
        string head_readings = "Head#" + string.Join("#", head_positions.Select(v=>v.ToString("F4")) );
        head_readings = head_readings.Replace(",", ";").Replace("#",delimiter);
        string t_time_s = "Time," + string.Join(delimiter, t_time);
        using (StreamWriter writer = new StreamWriter(String.Format("Assets/Trial_{0}_sensitivity_{1}_{2}.csv", 
                curr_test, test_sensitivity[curr_test], DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss"))))
        {
            writer.WriteLine(header);
            writer.WriteLine(topleft);
            writer.WriteLine(topright);
            writer.WriteLine(bottomleft);
            writer.WriteLine(bottomright);
            writer.WriteLine(head_readings);
            writer.WriteLine(t_time_s);
        }
    }

    async void display_time(float time_diff)
    {
        text.text = "Trail #: " + curr_test + "\n" + Math.Floor(time_diff - t) + " seconds left \n Current stage : " + stage;
    }  
    public void change_control_time(float slider_val){
        change_time(Trial_Stages.Control_Stage, slider_val);
    }
    public void change_test_time(float slider_val){
        change_time(Trial_Stages.Test_Stage, slider_val);
    }
    public void change_aftermath_time(float slider_val){
        change_time(Trial_Stages.Aftermath_Stage, slider_val);
    }
    public void change_pause_time(float slider_val){
        change_time(Trial_Stages.Pause_Stage, slider_val);

    }
    

    
    private void change_time(Trial_Stages stage, float slider_val){
            switch(stage){
                case Trial_Stages.Control_Stage:
                    control_duration = slider_val;
                    control_display_time.text = "Control : " + slider_val;
                    break;
                case Trial_Stages.Test_Stage:
                    test_duration = slider_val;
                    test_display_time.text = "Test : " + slider_val;
                    break;
                case Trial_Stages.Aftermath_Stage:
                    aftermath_duration = slider_val;
                    aftermath_display_time.text = "Aftermath : " + slider_val;
                    break;
                case Trial_Stages.Pause_Stage:
                    pause_duration = slider_val;
                    pause_display_time.text = "Pause : " + slider_val;
                    break;
                default :
                    return;
            }
    }
    public Button sensitivity_btn_copy;
    public Transform sensitivity_btns_container;
    public List<Button> sensitivity_btns;
  
    public Text user_input;
    public void AddSensitivity(){
        if (float.TryParse(user_input.text, out float sensitivity)){
            var btn = Instantiate(sensitivity_btn_copy, sensitivity_btns_container).GetComponent<Button>();
            btn.onClick.AddListener(delegate{DeleteSensitivity(btn);});
            sensitivity_btns.Add(btn);
            test_sensitivity.Add(sensitivity);
            btn.GetComponentInChildren<Text>().text = user_input.text;
        }
    }
    public void DeleteSensitivity(Button btn){
        int index = sensitivity_btns.FindIndex(a=> a == btn);
        test_sensitivity.RemoveAt(index);
        sensitivity_btns.RemoveAt(index);
        Destroy(btn.gameObject);
    }
}
