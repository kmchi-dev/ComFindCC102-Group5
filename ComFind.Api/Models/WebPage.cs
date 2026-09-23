namespace ComFind.Api.Models;

public class WebPage
{
    public int Id { get; set; }

    public int WebsiteId { get; set; }

    public string Url { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime CrawledAt { get; set; }

    public Website? Website { get; set; }
}