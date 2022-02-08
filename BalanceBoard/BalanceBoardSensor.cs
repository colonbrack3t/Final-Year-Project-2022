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
    
    //ref to buttons to add onlclick handlers
    [SerializeField] private Button Save_latency;
    [SerializeField] private Button Record_headset_board;
    [SerializeField] private Button Save_headset_board;
    [SerializeField] private Button mouse_record_button;
    [SerializeField] private Button save_mouse_board_data;
    [SerializeField] private Button record_board_button;

    //weight average computation attributes
    private bool calculateWeight = false;
    public double weight = 0;
    private int num_weight_readings = 0;
    //Text visual component
    [SerializeField] private Text UItext;
    //Headset "Camera" transform - matches where headset is in real world
    [SerializeField] private Transform headsetcamera;

    //record headset/record board sensors flags
    [SerializeField] public bool record_headset = false, record_board_collision = false;

    // Lists to store board/headset/mouse clicks

    //saves
    private List<double[]>[] board_recording = new List<double[]>[]{new List<double[]>(),new List<double[]>(),new List<double[]>(),new List<double[]>()};
    private List<double[]> Headset_record = new List<double[]>();
    private List<double> mouse_click_record = new List<double>();

    private double record_start_time = 0;
    UDPSocketUnity server = new UDPSocketUnity();
    // Start is called before the first frame update
    void Start()
    {
      //add onclick handlers for various UI
        Save_latency.onClick.AddListener(StoreLatencyData);
        Save_headset_board.onClick.AddListener(StoreVRvsBoardReadings);
        Record_headset_board.onClick.AddListener(ToggleRecordHeadsetBoard);

        mouse_record_button.onClick.AddListener(MouseClickRecord);

        record_board_button.onClick.AddListener(ToggleRecordBoard);
        save_mouse_board_data.onClick.AddListener(SaveMouseBoardData);

        //Initiate UDP Server
        server.Server("127.0.0.1", 27334, this);
    }
    //record when mouse was clicked
    void MouseClickRecord(){
        mouse_click_record.Add(DateTime.Now.TimeOfDay.TotalMilliseconds - record_start_time);
    }
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
    //public wrapper for Unity Debug
    public void display(string s)
    {
        Debug.Log(s);
    }
    //public wrapper to add method of latency list
    public void AddLatency(double latency)
    {
        latencies.Add(latency);
    }

    //toggle recording headset height and board sensors
    public void ToggleRecordHeadsetBoard()
    {
        record_board_collision = !record_board_collision;
        record_headset = !record_headset;
        record_start_time = DateTime.Now.TimeOfDay.TotalMilliseconds;
    }
    //toggle recording board sensors
    public void ToggleRecordBoard(){
        record_board_collision = !record_board_collision;
        record_start_time = DateTime.Now.TimeOfDay.TotalMilliseconds;
    }
    //save latency data to data.csv
    public void StoreLatencyData()
    {
        Debug.Log("Created data file");
        List<double> fixed_latencies = new List<double>(latencies);
        string x = "Reading #";
        string y = "Latency (ms)";

        for (int i = 0; i < fixed_latencies.Count; i++)
        {
            x += "," + i;
            y += "," + fixed_latencies[i];
        }

        using (StreamWriter writer = new StreamWriter("Assets/BalanceBoard/data.csv"))
        {
            writer.WriteLine(x);
            writer.WriteLine(y);
            writer.WriteLine(avglatency);
        }
    }
    //save mouse + board sensor data
    public void SaveMouseBoardData(){
        List<double[]>[] raw_data = {
                                        new List<double[]>(board_recording[0]),
                                        new List<double[]>(board_recording[1]),
                                        new List<double[]>(board_recording[2]),
                                        new List<double[]>(board_recording[3])
                                        };
        List<double> mouse_data = new List<double>(mouse_click_record);
        List<string> lines = new List<string>();
        for (int i = 0; i < raw_data.Length; i++)
        {
            string output_line = "";
            string time = "Time";
            switch (i)
            {
                case 0:
                    output_line = "Top Left raw data";
                    break;
                case 1:
                    output_line = "Top Right raw data";
                    break;
                case 2:
                    output_line = "Bottom Left raw data";
                    break;
                case 3:
                    output_line = "Bottom Right raw data";
                    break;



            }
            for (int j = 0; j < raw_data[i].Count; j++)
            {
                output_line += ", " + raw_data[i][j][0];
                time += ", " + raw_data[i][j][1];
            }
            lines.Add(output_line);
            lines.Add(time);
        }
        string mouse_line = "Mouse Click Times";
        foreach (double timestamp in mouse_data)
        {
            mouse_line+= "," + timestamp;

        }
        lines.Add(mouse_line);
        using (StreamWriter writer = new StreamWriter("Assets/BalanceBoard/mouse.csv"))
        {
            foreach (string item in lines)
            {
                writer.WriteLine(item);
            }
        }
Debug.Log("Done");
    }

    //save headset and board data
    public void StoreVRvsBoardReadings()
    {
        Debug.Log("Created data file");

        List<double[]>[] raw_data = {
                                        new List<double[]>(board_recording[0]),
                                        new List<double[]>(board_recording[1]),
                                        new List<double[]>(board_recording[2]),
                                        new List<double[]>(board_recording[3])
                                        };
        List<double[]> headset = new List<double[]>(Headset_record);
        List<string> lines = new List<string>();
        for (int i = 0; i < raw_data.Length; i++)
        {
            string output_line = "";
            string time = "Time";
            switch (i)
            {
                case 0:
                    output_line = "Top Left raw data";
                    break;
                case 1:
                    output_line = "Top Right raw data";
                    break;
                case 2:
                    output_line = "Bottom Left raw data";
                    break;
                case 3:
                    output_line = "Bottom Right raw data";
                    break;



            }
            for (int j = 0; j < raw_data[i].Count; j++)
            {
                output_line += ", " + raw_data[i][j][0];
                time += ", " + raw_data[i][j][1];
            }
            lines.Add(output_line);
            lines.Add(time);
        }
        string headset_height = "Headset Height";
        string headset_time = "Time";
        for (int i = 0; i < headset.Count; i++)
        {
            headset_height += ", " + Headset_record[i][0];
            headset_time += ", " + Headset_record[i][1];
        }
        lines.Add(headset_height);
        lines.Add(headset_time);
        using (StreamWriter writer = new StreamWriter("Assets/BalanceBoard/headset.csv"))
        {
            foreach (string item in lines)
            {
                writer.WriteLine(item);
            }
        }
Debug.Log("Done");

    }


    //Record headset vertical height
    public void RecordHeadsetVertical()
    {
        try
        {
          //take difference from starttime and current time
             Headset_record.Add(new double[] { (double) headsetcamera.position.y, DateTime.Now.TimeOfDay.TotalMilliseconds-record_start_time });
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            throw;
        }


    }
    public void toggleWeightCalculation(){
        calculateWeight = !calculateWeight;
    }
    //record raw data from balance board
    public void Record_Raw(string msg_parts)
    {
      //get corresponding index of sensor to list of sensor readings, and get current value of that sensor
        int i;
        double value;
        switch (msg_parts)
        {

            case "rTL":
                value = rwTopLeft;
                i = 0;
                break;
            case "rTR":
                value = rwTopRight;
                i = 1;
                break;
            case "rBL":
                value = rwBottomLeft;
                i = 2;
                break;
            case "rBR":
                value = rwBottomRight;
                i = 3;
                break;
            default:
                return;

        }
        //add reading to list, along with timestamp of data collection
        board_recording[i].Add(new double[] { (double)value, DateTime.Now.TimeOfDay.TotalMilliseconds-record_start_time });

    }
}
