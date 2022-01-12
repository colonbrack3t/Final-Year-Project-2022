using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
public class UDPSocketUnity : UDPSocket.UDPSocket
{
    private BalanceBoardSensor unityScript;
    public void Server(string address, int port, BalanceBoardSensor script)
    {
        unityScript = script;
        base.Server(address, port);
    }

    public override void Receive()
    {
        _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
        {
            State so = (State)ar.AsyncState;

            SocketError errorCode;
            int bytes = _socket.EndReceive(ar, out errorCode);
            if (errorCode != SocketError.Success)
            {
                bytes = 0;
            }
            else
            {
                string msg = Encoding.ASCII.GetString(so.buffer, 0, bytes);
                string[] msg_parts = msg.Split(':');

                switch (msg_parts[0])
                {

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
                    case "aTL":
                        unityScript.adjustedTL = double.Parse(msg_parts[1]);
                        break;
                    case "aTR":
                        unityScript.adjustedTR = double.Parse(msg_parts[1]);
                        break;
                    case "aBL":
                        unityScript.adjustedBL = double.Parse(msg_parts[1]);
                        break;
                    case "aBR":
                        unityScript.adjustedBR = double.Parse(msg_parts[1]);
                        break;
                    case "timestamp":
                        double curr_ms = DateTime.Now.TimeOfDay.TotalMilliseconds;
                        double received_ms = Double.Parse(msg_parts[1]);
                        double latency = curr_ms - received_ms;
                        unityScript.latency = latency;
                        unityScript.AddLatency(latency);
                        if (latency < unityScript.minlatency) unityScript.minlatency = latency;
                        if (latency > unityScript.maxlatency) unityScript.maxlatency = latency;
                        if (latency < 0){
                            unityScript.display(latency + ", curr ms : " + curr_ms + ", received ms : " + received_ms + ", exact string : " + msg_parts[1]);
                        }
                        if (unityScript.latency_count < 2)
                        {
                            unityScript.avglatency = latency;
                            if (unityScript.latency_count == 1)
                            {
                                unityScript.avg_ms_per_reading = curr_ms - unityScript.prev_ms;
                            }

                        }
                        else
                        {
                            unityScript.avglatency = ((unityScript.avglatency * (unityScript.latency_count - 1)) + latency) / unityScript.latency_count;
                            unityScript.avg_ms_per_reading = ((unityScript.avg_ms_per_reading * (unityScript.latency_count - 1)) + (curr_ms - unityScript.prev_ms)) / unityScript.latency_count;
                        }
                        unityScript.prev_ms = curr_ms;
                        unityScript.latency_count += 1;
                        break;
                }
                if (unityScript.record_board_collision) 
                    unityScript.RecordBoard(msg_parts[0]);

            }

            _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);

        }, state);
    }


}