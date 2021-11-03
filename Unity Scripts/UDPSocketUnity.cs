public class UDPSocketUnity : UDPSocket
{
    private BalanceboardSensor unity_script;
    public override void Server(string address, int port, BalanceboardSensor script): base(name, basepay)
    {
        unity_script = script;
        super.Server(address, port);
    }

    public override void Response
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