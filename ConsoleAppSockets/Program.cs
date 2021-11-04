


using ConsoleAppSockets;

var url = "news.dotsrc.org";
var username = "rasm652e@easv365.dk";
string password = 948947.ToString();


NewsClient client = new NewsClient(url);


Console.Out.WriteLine("login: " + client.Login(username,password));




client.SelectNewsgroup("alt.comp.os.windows-11");
var article = client.SelectArticle(250);




void printAllLines()
{
    foreach (var line in client.ReadAllLines())
    {
        Console.Out.WriteLine(line);
    }
}