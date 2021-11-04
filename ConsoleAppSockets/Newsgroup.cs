namespace ConsoleAppSockets;

public class Newsgroup
{
    public string Name { get; set; }
    public long ArticleCount => HighestIndex - LowestIndex;
    public long LowestIndex { get; set; }
    public long HighestIndex { get; set; }
    public bool CanPost { get; set; }
    public bool IsModerated { get; set; }
    public bool IsEmpty => HighestIndex <= LowestIndex;
    
    public List<Article> Articles{ get; set; } = new();

    public override string ToString()
    {
        return
            $"{nameof(Name)}: {Name}, {nameof(ArticleCount)}: {ArticleCount}, {nameof(LowestIndex)}: {LowestIndex}, {nameof(HighestIndex)}: {HighestIndex}, {nameof(CanPost)}: {CanPost}, {nameof(IsModerated)}: {IsModerated}, {nameof(IsEmpty)}: {IsEmpty}";
    }
}