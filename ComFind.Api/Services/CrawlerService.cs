using ComFind.Api.Data;
using ComFind.Api.Models;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ComFind.Api.Services;

public class CrawlerService
{
    private readonly HttpClient _httpClient;
    private readonly ComFindDbContext _db;

    public CrawlerService(HttpClient httpClient, ComFindDbContext db)
    {
        _httpClient = httpClient;
        _db = db;
    }

    public async Task CrawlWebsiteAsync(Website website)
    {
        if (!website.IsAllowedToCrawl)
        {
            return;
        }

        var robotsUrl = website.RobotsTxtUrl;

        if (string.IsNullOrWhiteSpace(robotsUrl))
        {
            robotsUrl = new Uri(new Uri(website.Url), "/robots.txt").ToString();
        }

        var robotsResponse = await _httpClient.GetAsync(robotsUrl);

        if (!robotsResponse.IsSuccessStatusCode)
        {
            return;
        }

        var robotsContent = await robotsResponse.Content.ReadAsStringAsync();

        var websiteUri = new Uri(website.Url);

        if (!IsAllowedByRobots(robotsContent, websiteUri.AbsolutePath))
        {
            return;
        }

        var response = await _httpClient.GetAsync(website.Url);

        if (!response.IsSuccessStatusCode)
        {
            return;
        }

        var html = await response.Content.ReadAsStringAsync();

        var title = ExtractTitle(html);
        var content = ExtractContent(html);

        var existingPage = await _db.WebPages
    .FirstOrDefaultAsync(page => page.Url == website.Url);

if (existingPage == null)
{
    var webPage = new WebPage
    {
        WebsiteId = website.Id,
        Url = website.Url,
        Title = title,
        Content = content,
        CrawledAt = DateTime.UtcNow
    };

    _db.WebPages.Add(webPage);
}
else
{
    existingPage.Title = title;
    existingPage.Content = content;
    existingPage.CrawledAt = DateTime.UtcNow;
}

        website.LastCrawled = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }

    private static bool IsAllowedByRobots(string robotsContent, string path)
    {
        var lines = robotsContent.Split(
            '\n',
            StringSplitOptions.RemoveEmptyEntries
        );

        bool appliesToAllAgents = false;
        var disallowedPaths = new List<string>();

        foreach (var rawLine in lines)
        {
            var line = rawLine.Trim();

            if (line.StartsWith("#"))
            {
                continue;
            }

            var separator = line.IndexOf(':');

            if (separator == -1)
            {
                continue;
            }

            var directive = line[..separator].Trim().ToLowerInvariant();
            var value = line[(separator + 1)..].Trim();

            if (directive == "user-agent")
            {
                appliesToAllAgents = value == "*";
                continue;
            }

            if (appliesToAllAgents && directive == "disallow")
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    disallowedPaths.Add(value);
                }
            }
        }

        foreach (var disallowedPath in disallowedPaths)
        {
            if (path.StartsWith(
                disallowedPath,
                StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }

        return true;
    }

    private static string ExtractTitle(string html)
    {
        var document = new HtmlDocument();

        document.LoadHtml(html);

        var titleNode = document.DocumentNode.SelectSingleNode("//title");

        if (titleNode == null)
        {
            return string.Empty;
        }

        return WebUtility.HtmlDecode(
            titleNode.InnerText.Trim()
        );
    }

    private static string ExtractContent(string html)
    {
        var document = new HtmlDocument();

        document.LoadHtml(html);

        var nodesToRemove = document.DocumentNode.SelectNodes(
            "//script|//style|//noscript|//svg"
        );

        if (nodesToRemove != null)
        {
            foreach (var node in nodesToRemove)
            {
                node.Remove();
            }
        }

        var text = document.DocumentNode.InnerText;

        text = WebUtility.HtmlDecode(text);

        text = System.Text.RegularExpressions.Regex.Replace(
            text,
            @"\s+",
            " "
        );

        return text.Trim();
    }
}