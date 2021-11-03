using System;
using System.Text.RegularExpressions;
using System.Timers;
using WiimoteLib;

namespace Wii_Balanceboard_client
{
    internal class WiiServer
    {
        private static Wiimote wiiDevice;

        private static readonly Timer infoUpdateTimer = new Timer {Interval = 50, Enabled = false};

        private static float adjustTopLeft;
        private static float adjustTopRight;
        private static float adjustBottomLeft;
        private static float adjustBottomRight;
        private static UDPSocket.UDPSocket c = new UDPSocket.UDPSocket();
        private static bool display_values;

        public static void Main(string[] args)
        {
            //Set up public variables
            infoUpdateTimer.Elapsed += InfoUpdateTimerOnElapsed;
            c.Client("127.0.0.1", 27002);

            //connect device
            var connected = Connect_Device();
            while (!connected)
            {
                if (!Ask_Yes_No_Question("Do you want to try again?")) return;

                connected = Connect_Device();
            }

            Console.WriteLine("Connection successful!");
            Console.ReadKey(true);

            //
            if (Ask_Yes_No_Question("Do you want to calibrate balance?")) Zero_Out();
            Console.WriteLine("Press d to toggle display. Press Esc to exit program");
            bool cont = true;
            while (cont)
            {
                var keyPressed = Console.ReadKey(true);
                switch (keyPressed.Key)
                {
                    case ConsoleKey.D:
                        if (!display_values)
                        {
                            
                            display_values = true;
                        }
                        else
                        {
                            display_values = false;
                            
                            Console.WriteLine("\nPress d to toggle display. Press Esc to exit program");
                        }

                        break;
                    case ConsoleKey.Escape:
                        cont = false;
                        break;
                    
                }
            }
            wiiDevice.Disconnect();
        }

        private static void InfoUpdateTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            var rwTopLeft = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesKg.TopLeft;
            var rwTopRight = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesKg.TopRight;
            var rwBottomLeft = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesKg.BottomLeft;
            var rwBottomRight = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesKg.BottomRight;
            var adjustedTL = rwTopLeft - adjustTopLeft;
            var adjustedTR = rwTopRight - adjustTopRight;
            var adjustedBL = rwBottomLeft - adjustBottomLeft;
            var adjustedBR = rwBottomRight - adjustBottomRight;
            if (display_values) Console.Write("\r Raw weights: TL {0,6:##0.0000} TR {1,6:##0.0000} BL {2,6:##0.0000} BR{3,6:##0.0000}. Adjusted weights TL {4,6:##0.0000} TR {5,6:##0.0000} BL {6,6:##0.0000} BR{7,6:##0.0000}.                                 ", rwTopLeft, rwTopRight, rwBottomLeft, rwBottomRight, adjustedTL, adjustedTR, adjustedBL, adjustedBR);
            
             // send each reading separately or send all together? Test
            Send_Data("rTL", rwTopLeft);
            Send_Data("rTR", rwTopRight);
            Send_Data("rBL", rwBottomLeft);
            Send_Data("rBR", rwBottomRight);
            Send_Data("aTL", adjustedTL);
            Send_Data("aTR", adjustedTR);
            Send_Data("aBL", adjustedBL);
            Send_Data("aBR", adjustedBR);
        }

        private static void Send_Data(string name, float value)
        {
            c.Send($"{name}:{value}");
        }

        private static void Zero_Out()
        {
            adjustTopLeft = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesKg.TopLeft;
            adjustTopRight = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesKg.TopRight;
            adjustBottomLeft = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesKg.BottomLeft;
            adjustBottomRight = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesKg.BottomRight;
        }

        private static bool Connect_Device()
        {
            Console.WriteLine("Press any key to attempt to connect to device");
            Console.ReadKey(true);
            return Attempt_Connect();
        }

        private static bool Attempt_Connect()
        {
            try
            {
                // Find all connected Wii devices.

                var deviceCollection = new WiimoteCollection();
                deviceCollection.FindAllWiimotes();

                for (var i = 0; i < deviceCollection.Count; i++)
                {
                    wiiDevice = deviceCollection[i];

                    // Device type can only be found after connection, so prompt for multiple devices.

                    if (deviceCollection.Count > 1)
                    {
                        var devicePathId = new Regex("e_pid&.*?&(.*?)&").Match(wiiDevice.HIDDevicePath)
                            .Groups[1]
                            .Value.ToUpper();
                        if (!Choose_Device(devicePathId, i + 1, deviceCollection.Count)) continue;

                 
                    }

                    // Connect and send a request to verify it worked.

                    wiiDevice.Connect();
                    wiiDevice.SetReportType(InputReport.IRAccel,
                        false); // FALSE = DEVICE ONLY SENDS UPDATES WHEN VALUES CHANGE!
                    wiiDevice.SetLEDs(true, false, false, false);

                    // Enable processing of updates.
                    infoUpdateTimer.Enabled = true;
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return true;
        }

        private static bool Choose_Device(string devicePathId, int index, int len)
        {
            return Ask_Yes_No_Question("Connect to HID " + devicePathId + " device " + index + " of " + len + " ?");
        }

        private static bool Ask_Yes_No_Question(string str)
        {
            Console.WriteLine(str + " [Y]/[n]");
            return Console.ReadKey(true).Key != ConsoleKey.N;
        }
    }
}