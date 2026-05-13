namespace WpfApp1.Models;

public class Snippet
{
    public int Id { get; set; }

    public string Keyword { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public bool Enabled { get; set; } = true;
    public override string ToString()
    {
        return $"{Keyword} - {Content}";
    }
}
