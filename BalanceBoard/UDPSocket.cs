namespace UDPSocket
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    //standard UDP Socket taken from https://gist.github.com/louis-e/888d5031190408775ad130dde353e0fd#file-udpsocket-cs
    public class UDPSocket
    {
        protected Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        protected const int bufSize = 8 * 1024;
        protected State state = new State();
        protected EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
        protected AsyncCallback recv = null;

        public class State
        {
            public byte[] buffer = new byte[bufSize];
        }

        public void Server(string address, int port)
        {
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            Receive();
        }

        //client does not need to recieve only send data
        public void Client(string address, int port)
        {
            _socket.Connect(IPAddress.Parse(address), port);

        }
        
        public void Send(string text)
        {
            byte[] data = Encoding.ASCII.GetBytes(text);
            _socket.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = _socket.EndSend(ar);

            }, state);
        }

        public virtual void Receive()
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

                _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);

            }, state);
        }
    }

}
