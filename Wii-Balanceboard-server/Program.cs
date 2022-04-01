using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using WiimoteLib;
using Timer = System.Timers.Timer;

namespace Wii_Balanceboard_client
{
    internal class WiiServerable
    {
        //Wiimote class from the WiimoteLab library. Handles connection and data extraction
        private static Wiimote wiiDevice;
        //UDP socket client
        private static UDPSocket.UDPSocket c = new UDPSocket.UDPSocket();
        private static 
            bool display_values = false, // flag to show sensor readings in real time
            test_latency = false, // initiate latency tester- sends time stamp over socket 
            record = false, // flag for recording sensor values
            record_rate = false; // flag for recording rate of sensor readings
        //list of times when space bar is pressed - used for recording sensor latency 
        private static List<double> space_bar_time = new List<double>();
        
        //list of sensor values and time when this data was captured
        private static string[] recorded_data = new string[5] { "Raw Top Left", "Raw Top Right", "Raw Bottom Left", "Raw Bottom Right", "Time"};
        //time when last received reading - used for recording rate of sensor readings
        private static List<double> time_of_readings = new List<double>();
        // value used to keep track of delay from next reading to previous reading
        private static double prevTime;
        public static void Main(string[] args)
        {
            
            //establish client
            c.Client("127.0.0.1", 27335);

            //attempt connect wiimote
            var connected = Connect_Device();
            //loop connection
            while (!connected)
            {
                if (!Ask_Yes_No_Question("Do you want to try again?")) return;

                connected = Connect_Device();
            }
            
            Console.WriteLine("Connection successful!");
            Console.ReadKey(true);
            
            
            //When controller is connected, synchronous thread starts which captures data 
            // for more information see threadingTick
            
            //present user options, then start loop for waiting user input
            
            string printmsg =
                "Press d to toggle display. Press S to check avg + std scan rate. Press Z to zero out. Press T to test Latency. Press R to toggle record data to 'data.csv'. Press P to change Port. Press Spacebar to measure latency compared to Spacebar clicks. Press Esc to exit program";
            Console.WriteLine(printmsg);
            bool cont = true;
            while (cont)
            {
                
                var keyPressed = Console.ReadKey(true);
                switch (keyPressed.Key)
                {
                    //toggle display of realtime sensor readings
                    case ConsoleKey.D:
                        if (!display_values)
                        {
                            display_values = true;
                        }
                        else
                        {
                            display_values = false;
                            
                            Console.WriteLine("\n"+printmsg);
                        }

                        break;
                    //if user presses escape, end program
                    case ConsoleKey.Escape:
                        cont = false;
                        break;
                   //if user presses t, start socket latency test
                    case ConsoleKey.T:
                        test_latency = !test_latency;
                        break;
                    // if user presses p they can change the client port 
                    case ConsoleKey.P:
                        Console.WriteLine("Enter Port number");
                        string input = Console.ReadLine();
                        try
                        {
                            int port = int.Parse(input);
                            c.Client("127.0.0.1", port);
                        }    
                        catch (Exception)
                        {
                            Console.WriteLine("Not a valid int");
                            
                        }
                        break;
                    // if spacebar is pressed, append current time to list of spacebar presses
                    case ConsoleKey.Spacebar:
                        space_bar_time.Add(DateTime.Now.TimeOfDay.TotalMilliseconds);
                        break;
                    
                    // if user presses S, start recording sensor read rate
                    case ConsoleKey.S:
                        
                        time_of_readings = new List<double>();
                        prevTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
                        record_rate = !record_rate;
                        break;
                    //if user presses r, record sensor data. When untoggled, saves data to data.csv
                    case ConsoleKey.R:
                        if (record) {
                            record = !record;
                            string s = "Spacebar clicks";
                            foreach (double t in space_bar_time)
                            {
                                s += ", " + t;
                            }
                            using (StreamWriter w = new StreamWriter("data.csv"))
                            {
                                foreach (string item in recorded_data)
                                {
                                    w.WriteLine(item);
                                }
                                w.WriteLine(s);
                            }
                        }
                        else
                        {
                            recorded_data = new string[5] { "Raw Top Left", "Raw Top Right", "Raw Bottom Left", "Raw Bottom Right","Time" };
                            record = !record;

                        }


                        
                        break;
                }
            }
            //disconnect before closing
            wiiDevice.Disconnect();
        }

        //This function collects sensor data, and handles various rate recordings
        private static void Tick()
        
        {
            // collect data from all 4 sensors
            var rwTopLeft = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesKg.TopLeft;
            var rwTopRight = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesKg.TopRight;
            var rwBottomLeft = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesKg.BottomLeft;
            var rwBottomRight = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesKg.BottomRight;
            
            //if user has toggled to see real time sensors, print them
            if (display_values) Console.Write("\r Raw weights: TL {0,6:##0.0000} TR {1,6:##0.0000} BL {2,6:##0.0000} BR{3,6:##0.0000} SUM {4,6:##0.0000}                                 ", rwTopLeft, rwTopRight, rwBottomLeft, rwBottomRight,
                rwBottomLeft + rwBottomRight + rwTopLeft + rwTopRight
                );
            
             // send each reading separately or send all together? Test
           Send_Data("rTL", rwTopLeft);
            Send_Data("rTR", rwTopRight);
            Send_Data("rBL", rwBottomLeft);
            Send_Data("rBR", rwBottomRight);
           // if testing socket latency, send time stamp
            if (test_latency)
            {
                var dt = DateTime.Now.TimeOfDay.TotalMilliseconds.ToString();
               
                Send_Data("timestamp", dt);
            };
            //if recording data, append to list of records
            if (record) {
                recorded_data[0] += ", " + rwTopLeft.ToString();
                recorded_data[1] += ", " + rwTopRight.ToString();
                recorded_data[2] += ", " + rwBottomLeft.ToString();
                recorded_data[3] += ", " + rwBottomRight.ToString();
                recorded_data[4] += ", " + DateTime.Now.TimeOfDay.TotalMilliseconds;


            }
            //if recording rate of sensor readings, add reading to list, compute mean and standard deviation, print values
            if (record_rate){            
                time_of_readings.Add(DateTime.Now.TimeOfDay.TotalMilliseconds-prevTime);
                double mean = time_of_readings.Sum() / time_of_readings.Count();
                double std = 0;
                foreach (var r in time_of_readings)
                {
                    std += Math.Pow(r - mean, 2);
                }

                std = Math.Sqrt(std/time_of_readings.Count());
                Console.Write("\r mean: " + mean + " std: " + std);
                prevTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
            }
        }
        //wrappers for sending data over socket
        private static void Send_Data(string name, float value)
        {
            c.Send($"{name}:{value}");
        }
        private static void Send_Data(string name, string value)
        {
            c.Send($"{name}:{value}");
        }

    
        //wrapper + UI for connecting to wiimote
        private static bool Connect_Device()
        {
            Console.WriteLine("Press any key to attempt to connect to device");
            Console.ReadKey(true);
            return Attempt_Connect();
        }

        //Attempt to connect to device
        //Returns true if connection established
        private static bool Attempt_Connect()
        {
            try
            {
                // Find all connected Wii devices to pc
                var deviceCollection = new WiimoteCollection();
                deviceCollection.FindAllWiimotes();

                for (var i = 0; i < deviceCollection.Count; i++)
                {
                    wiiDevice = deviceCollection[i];
    
                    // Device type can only be found after connection, so prompt for multiple devices- in case multiple devices are connected to PC
                    if (deviceCollection.Count > 1)
                    {
                        var devicePathId = new Regex("e_pid&.*?&(.*?)&").Match(wiiDevice.HIDDevicePath)
                            .Groups[1]
                            .Value.ToUpper();
                        if (!Choose_Device(devicePathId, i + 1, deviceCollection.Count)) continue;

                 
                    }

                    // Connect and send a request to verify it worked
                    wiiDevice.Connect();
                    wiiDevice.SetReportType(InputReport.IRAccel,
                        true); // FALSE = DEVICE ONLY SENDS UPDATES WHEN VALUES CHANGE!
                    wiiDevice.SetLEDs(true, false, false, false);

                    // Enable processing of updates
                    new Thread(new ThreadStart(threadingTick)).Start();
                    break;
                }
            }
            //catch and print any exceptions- usually when pc has not correctly linked with board
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return true;
        }
        //Thread wrapper for tick function, runs at fastest speed possible
        private static void threadingTick()
        {
            while (true)
            {
                Tick();
                Thread.Yield();
            }
        }
        
        //wrapper function for selecting device
        private static bool Choose_Device(string devicePathId, int index, int len)
        {
            return Ask_Yes_No_Question("Connect to HID " + devicePathId + " device " + index + " of " + len + " ?");
        }
        //utility function for asking yes or no questions to user
        private static bool Ask_Yes_No_Question(string str)
        {
            Console.WriteLine(str + " [Y]/[n]");
            return Console.ReadKey(true).Key != ConsoleKey.N;
        }
    }
}