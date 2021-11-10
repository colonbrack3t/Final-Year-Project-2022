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
                        unityScript.rwTopLeft = float.Parse(msg_parts[1]);
                        break;
                    case "rTR":
                        unityScript.rwTopRight = float.Parse(msg_parts[1]);
                        break;
                    case "rBL":
                        unityScript.rwBottomLeft = float.Parse(msg_parts[1]);
                        break;
                    case "rBR":
                        unityScript.rwBottomRight = float.Parse(msg_parts[1]);
                        break;
                    case "aTL":
                        unityScript.adjustedTL = float.Parse(msg_parts[1]);
                        break;
                    case "aTR":
                        unityScript.adjustedTR = float.Parse(msg_parts[1]);
                        break;
                    case "aBL":
                        unityScript.adjustedBL = float.Parse(msg_parts[1]);
                        break;
                    case "aBR":
                        unityScript.adjustedBR = float.Parse(msg_parts[1]);
                        break; 
                }

            }

            _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);

        }, state);
    }


}