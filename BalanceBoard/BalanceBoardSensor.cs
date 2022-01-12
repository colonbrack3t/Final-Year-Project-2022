using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using System.IO;
public class BalanceBoardSensor : MonoBehaviour
{

    public double adjustedTL = 0f;
    public double adjustedTR = 0f;
    public double adjustedBL = 0f;
    public double adjustedBR = 0f;
    public double rwTopLeft = 0f;
    public double rwTopRight = 0f;
    public double rwBottomLeft = 0f;
    public double rwBottomRight = 0f;
    public double latency = 0f;
    public double avglatency = 0f;
    public double minlatency = float.MaxValue;
    public double maxlatency = 0f;
    public int latency_count = 0;
    public Button Save_latency;
    public Button Record_headset_board;
    public Button Save_headset_board;
    public Button mouse_record_button;
    public Button save_mouse_board_data;
    public Button record_board_button;
    public double avg_ms_per_reading = 0;
    public double prev_ms = 0;
    public double reading_diff = 0f;
    public Text UItext;
    public Transform camera;
    public double camera_height;
    public bool record_headset = false, record_board_collision = false;

    public List<double[]>[] board_recording = new List<double[]>[]{new List<double[]>(),new List<double[]>(),new List<double[]>(),new List<double[]>()};
    double record_start_time = 0;
    public List<double[]> Headset_record = new List<double[]>();
    public List<double> mouse_click_record = new List<double>();
    
    UDPSocketUnity server = new UDPSocketUnity();
    private List<double> latencies = new List<double>();
    // Start is called before the first frame update
    void Start()
    {
        Save_latency.onClick.AddListener(StoreLatencyData);
        Save_headset_board.onClick.AddListener(StoreVRvsBoardReadings);
        Record_headset_board.onClick.AddListener(ToggleRecordHeadsetBoard);

        server.Server("127.0.0.1", 27334, this);
        mouse_record_button.onClick.AddListener(MouseClickRecord);

        record_board_button.onClick.AddListener(ToggleRecordBoard);
        save_mouse_board_data.onClick.AddListener(SaveMouseBoardData);
    }
    void MouseClickRecord(){
        mouse_click_record.Add(DateTime.Now.TimeOfDay.TotalMilliseconds - record_start_time);
    }
    void Update()
    {
        UItext.text = camera.position.y.ToString();
        
        if(record_headset)
             RecordHeadsetVertical();
    }
    public void display(string s)
    {
        Debug.Log(s);
    }
    public void AddLatency(double latency)
    {
        latencies.Add(latency);
    }
    public void ToggleRecordHeadsetBoard()
    {
        record_board_collision = !record_board_collision;
        record_headset = !record_headset;
        record_start_time = DateTime.Now.TimeOfDay.TotalMilliseconds;
    }
    public void ToggleRecordBoard(){
        record_board_collision = !record_board_collision;
        record_start_time = DateTime.Now.TimeOfDay.TotalMilliseconds;
    }
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
    public void SaveMouseBoardData(){
        List<double[]>[] raw_data = {
                                        new List<double[]>(board_recording[0]),
                                        new List<double[]>(board_recording[1]),
                                        new List<double[]>(board_recording[2]),
                                        new List<double[]>(board_recording[3])
                                        };
        List<double> mosue_data = new List<double>(mouse_click_record);
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
        foreach (double timestamp in mosue_data)
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
    public void RecordBoard(string msg_parts)
    {
     
        Record_Raw(msg_parts);
        
    }
    public void RecordHeadsetVertical()
    {   
        try
        {
             Headset_record.Add(new double[] { (double) camera.position.y, DateTime.Now.TimeOfDay.TotalMilliseconds-record_start_time });
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            throw;
        }
        
        
    }
    public void Record_Raw(string msg_parts)
    {
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
        
        board_recording[i].Add(new double[] { (double)value, DateTime.Now.TimeOfDay.TotalMilliseconds-record_start_time });
       
    }
}