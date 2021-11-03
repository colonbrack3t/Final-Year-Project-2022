using System;
using System.Text.RegularExpressions;
using System.Timers;
using WiiBalanceWalker;
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
        private static UDPSocket c = new UDPSocket();
        private static bool display_values = false;

        public static void Main(string[] args)
        {
            //Set up public variables
            infoUpdateTimer.Elapsed += InfoUpdateTimerOnElapsed;
            c.Client("127.0.0.1", 27002);

            //connect device
            var connected = Connect_Device();
            while (!connected)
            {
                if (!AskYesNoQuestion("Do you want to try again?")) return;

                connected = Connect_Device();
            }

            Console.WriteLine("Connection successful!");
            Console.ReadKey(true);

            //
            if (AskYesNoQuestion("Do you want to calibrate balance?")) zeroout_Click();
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
            if (display_values) Console.Write(String.Format(
                "\r Raw weights: TL {0,6:##0.0000} TR {1,6:##0.0000} BL {2,6:##0.0000} BR{3,6:##0.0000}. Adjusted weights TL {4,6:##0.0000} TR {5,6:##0.0000} BL {6,6:##0.0000} BR{7,6:##0.0000}.                                 ",
                rwTopLeft, rwTopRight, rwBottomLeft, rwBottomRight,
                adjustedTL, adjustedTR, adjustedBL, adjustedBR)
            );
            string msg = String.Format(
                "rTL:{0},rTR:{1},rBL:{2},rBR:{3};aTL:{4},aTR:{5},aBL:{6},aBR:{7}",
                rwTopLeft, rwTopRight, rwBottomLeft, rwBottomRight,
                adjustedTL, adjustedTR, adjustedBL, adjustedBR);
            c.Send(msg); // send each reading separately or send all together? Test

        }

        private static void zeroout_Click()
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
                        if (ChooseDevice(devicePathId, i + 1, deviceCollection.Count)) return true;

                        continue;
                    }

                    // Connect and send a request to verify it worked.

                    wiiDevice.Connect();
                    wiiDevice.SetReportType(InputReport.IRAccel,
                        false); // FALSE = DEVICE ONLY SENDS UPDATES WHEN VALUES CHANGE!
                    wiiDevice.SetLEDs(true, false, false, false);
                    // Enable processing of updates.

                    infoUpdateTimer.Enabled = true;

                    // Prevent connect being pressed more than once.

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

        private static bool ChooseDevice(string devicePathId, int index, int len)
        {
            return AskYesNoQuestion("Connect to HID " + devicePathId + " device " + index + " of " + len + " ?");
        }

        private static bool AskYesNoQuestion(string str)
        {
            Console.WriteLine(str + " [Y]/[n]");
            return Console.ReadKey(true).Key != ConsoleKey.N;
        }
    }
}