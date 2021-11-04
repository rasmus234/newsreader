
namespace ConsoleAppSockets;

public static class ExtensionMethods
{
    public static IEnumerable<string> ReadAllLines(this StreamReader streamReader)
    {
        string readLine;
        while ((readLine = streamReader.ReadLine()) != null)
        {
            if (readLine == ".") break;

            if (readLine.StartsWith(".."))
                readLine = readLine[1..];

            yield return readLine;
        }
    }

    public static bool StatusCode(this string responseString,out int code)
    {
        return int.TryParse(responseString[..4], out code);
    }
}