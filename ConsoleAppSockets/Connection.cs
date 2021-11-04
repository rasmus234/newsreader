using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleAppSockets;

public class Connection
{
    private TcpClient socket;
    private readonly NetworkStream stream;
    private readonly StreamReader reader;
    private readonly IPEndPoint endpoint;


    public Connection(string url, int port = 119)
    {
        var ip = Dns.GetHostAddresses(url)[0];
        var endPoint = new IPEndPoint(ip, port);
        this.endpoint = endPoint;
        this.socket = new TcpClient();
        this.socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
        this.socket.Connect(endPoint);
        this.stream = socket.GetStream();
        this.reader = new StreamReader(socket.GetStream());
        Console.Out.WriteLine(ReadBufferLine());
        
    }


    public void ClearBuffer() => this.reader.DiscardBufferedData();
    public string ReadBufferLine()
    {
        return this.reader.ReadLine();
    }

    public IEnumerable<string> ReadBufferLines()
    {
        return this.reader.ReadAllLines();
    }

    public bool Write(string data)
    {
        data += "\r\n";
        try
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            this.stream.Write(bytes, 0, bytes.Length);
            return true;
        }
        catch (Exception e)
        {
            Console.Out.WriteLine(e.Message);
            return false;
        }
    }
}