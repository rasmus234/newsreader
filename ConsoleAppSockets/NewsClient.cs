using System.Text;

namespace ConsoleAppSockets;

public class NewsClient
{
    private IEnumerable<Newsgroup> _newsgroups = null;
    public IEnumerable<Newsgroup> Newsgroups => _newsgroups ??= RetrieveNewsGroups();

    public Newsgroup CurrentNewsGroup { get; set; }
    public int CurrentArticleNumber { get; set; }
    public bool IsLoggedIn { get; private set; }
    private Connection Connection { get; set; }

    public NewsClient(string url, int port = 119)
    {
        this.Connection = new Connection(url, port);
    }

    public string ReadLine()
    {
        return this.Connection.ReadBufferLine();
    }

    public IEnumerable<string> ReadAllLines()
    {
        return this.Connection.ReadBufferLines();
    }

    public bool Write(string data)
    {
        return this.Connection.Write(data);
    }

    public bool Login(string user, string pass)
    {
        //send username and password data
        Write($"authinfo user {user}");
        var userResponse = ReadLine().StatusCode(out var userStatusCode);

        Write($"authinfo pass {pass}");
        var passResponse = ReadLine().StatusCode(out var passStatusCode);
        
        
        //return false if login fails
        if (!(userResponse & passResponse & userStatusCode == 381 & passStatusCode == 281)) return false;

        this._newsgroups = RetrieveNewsGroups();
        this.IsLoggedIn = true;
        return true;
    }

    private IEnumerable<Newsgroup> RetrieveNewsGroups()
    {
        Write("list");
        var data = ReadAllLines().ToArray();
        var groups = data.Skip(1).Select(str =>
        {
            var values = str.Split(" ");

            if (!long.TryParse(values[1].TrimStart('0'), out var highestIndex)) highestIndex = 0;
            if (!long.TryParse(values[2].TrimStart('0'), out var lowestIndex)) lowestIndex = 0;
            var isModerated = values[3].Equals("m");
            var canPost = values[3].Equals("y") || isModerated;

            return new Newsgroup()
            {
                Name = values[0],
                LowestIndex = lowestIndex,
                HighestIndex = highestIndex,
                CanPost = canPost,
                IsModerated = isModerated
            };
        });

        return groups;
    }

    public Newsgroup SelectNewsgroup(string name)
    {
        var selected = Newsgroups.First(newsgroup => newsgroup.Name.Equals(name)) ?? null;
        if (selected is not null)
        {
            Write($"group {selected.Name}");
        }

        var response = ReadLine();
        this.CurrentNewsGroup = selected;

        this.CurrentNewsGroup.Articles = GetArticleHeadLines().Select(headers => new Article { Headers = headers }).ToList();
        
        return selected;
    }

    public Article SelectArticle(long index)
    {
        Write($"body {index}");
        var response = ReadAllLines().Skip(1).ToArray();

        StringBuilder body = new StringBuilder();
        foreach (var s in response)
        {
            body.Append(s + "\n");
        }

        var headers = CurrentNewsGroup.Articles.FirstOrDefault(article => article.Headers.Index == index).Headers;
        return new Article()
        {
            Body = body.ToString(),
            Headers = headers
        };
    }

    public List<ArticleHeaders> GetArticleHeadLines()
    {
        var group = this.CurrentNewsGroup;
        Write($"hdr subject {group.LowestIndex}-{group.HighestIndex}");
       
        return ReadAllLines().Skip(1).Select(s =>
        {
            
            var values = s.Split(" ", 2);
            return new ArticleHeaders
            {
                Index = long.Parse(values[0]),
                Subject = values[1]
            };
        }).ToList();
    }
    
    public bool AddArticle(string subject, string body)
    {
        if (!this.CurrentNewsGroup.CanPost) return false;

        Write($"post {subject}");
        Write(body);
        var response = ReadLine();
        return response.StatusCode(out var statusCode) && statusCode == 340;
    }
}