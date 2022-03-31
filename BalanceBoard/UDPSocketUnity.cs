using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
//extends UDPSocket to parse data as it is received
public class UDPSocketUnity : UDPSocket.UDPSocket
{
    // reference to balance board sensor obj that will recieve parsed data
    private BalanceBoardSensor unityScript;

    //override constructor to add script parameter
    public void Server(string address, int port, BalanceBoardSensor script)
    {
        unityScript = script;
        base.Server(address, port);
    }


    //edited implementation of Recieve()
    public override void Receive()
    {
        _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
        {
            State so = (State)ar.AsyncState;
            //fixed errorccode bug
            SocketError errorCode;
            int bytes = _socket.EndReceive(ar, out errorCode);
            if (errorCode != SocketError.Success)
            {
                bytes = 0;
            }
            else
            {
                //Recieving balanceboard client data
                string msg = Encoding.ASCII.GetString(so.buffer, 0, bytes);
                string[] msg_parts = msg.Split(':');
                // all data will be sent as sensor_name:sensor_value, or in case of latency testing,
                // timestamp:timestamp-value
                switch (msg_parts[0])
                {
                    //for all sensor data, update balanceboard script attributes
                    case "rTL":
                        unityScript.rwTopLeft = double.Parse(msg_parts[1]);
                        break;
                    case "rTR":
                        unityScript.rwTopRight = double.Parse(msg_parts[1]);
                        break;
                    case "rBL":
                        unityScript.rwBottomLeft = double.Parse(msg_parts[1]);
                        break;
                    case "rBR":
                        unityScript.rwBottomRight = double.Parse(msg_parts[1]);
                        break;

                    case "timestamp":
                        monitor_latency(msg_parts);
                        break;
                }


            }

            _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);

        }, state);
    }
    void monitor_latency(string[] msg_parts)
    {
        //get current time in milliseconds
        double curr_ms = DateTime.Now.TimeOfDay.TotalMilliseconds;
        //get time data was sent out in milliseconds
        double received_ms = Double.Parse(msg_parts[1]);
        //compute latency
        double latency = curr_ms - received_ms;
        //assign latency variables of balance board
        unityScript.latency = latency;
        unityScript.AddLatency(latency);
        //assign min and max latency when applicable
        if (latency < unityScript.minlatency) unityScript.minlatency = latency;
        if (latency > unityScript.maxlatency) unityScript.maxlatency = latency;
        //catch impossible latencies - this could should never run
        if (latency < 0)
        {
            unityScript.display("Impossible Latency");
            unityScript.display(latency + ", curr ms : " + curr_ms + ", received ms : " + received_ms + ", exact string : " + msg_parts[1]);
        }
        //if we have less than 2 previous latencies, cannot calculate mean
        if (unityScript.latency_count < 2)
        {
            //just set average latency to current- not enough samples
            unityScript.avglatency = latency;
            if (unityScript.latency_count == 1)
            {
                //set average delay between readings to current delay- not enough samples to produce average
                unityScript.avg_ms_per_reading = curr_ms - unityScript.prev_ms;

            }

        }
        else
        {
            //have enough samples to start computing a dynamic mean for both sensor latencies and sensor capture rates
            unityScript.avglatency = ((unityScript.avglatency * (unityScript.latency_count - 1)) + latency) / unityScript.latency_count;
            unityScript.avg_ms_per_reading = ((unityScript.avg_ms_per_reading * (unityScript.latency_count - 1)) + (curr_ms - unityScript.prev_ms)) / unityScript.latency_count;
        }
        //update incremental values
        unityScript.prev_ms = curr_ms;
        unityScript.latency_count += 1;
    }

}
