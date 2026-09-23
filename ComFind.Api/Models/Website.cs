namespace ComFind.Api.Models;

public class Website
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public string? RobotsTxtUrl { get; set; }

    public bool IsAllowedToCrawl { get; set; } = true;

    public DateTime LastCrawled { get; set; }
}