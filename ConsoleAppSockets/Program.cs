
using System.Net;
using System.Net.Sockets;
using System.Text;

TcpClient socket = new();

var url = "news.dotsrc.org";
var username = "rasm652e@easv365.dk";
var password = 948947;
int port = 119;

NetworkStream ns;
StreamReader reader;

try
{
    IPAddress ip = Dns.GetHostEntry(url).AddressList[0];
    IPEndPoint endPoint = new IPEndPoint(ip, port);
    socket.Connect(endPoint);
    ns = socket.GetStream();
    Console.Out.WriteLine("socket connected to remote server");
    reader = new StreamReader(ns, Encoding.UTF8);
    Console.Out.WriteLine(reader.ReadToEnd());
}
catch (Exception e)
{
    Console.Out.WriteLine(e.Message);
}
