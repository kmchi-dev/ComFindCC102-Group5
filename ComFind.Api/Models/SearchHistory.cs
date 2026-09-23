namespace ComFind.Api.Models;

public class SearchHistory
{
    public int Id { get; set; }

    public string Query { get; set; } = string.Empty;

    public DateTime SearchedAt { get; set; }
}