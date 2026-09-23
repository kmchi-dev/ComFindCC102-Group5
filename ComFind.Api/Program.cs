using ComFind.Api.Data;
using ComFind.Api.Models;
using ComFind.Api.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ComFindDbContext>(options =>
    options.UseSqlite("Data Source=../Database/comfind.db"));

builder.Services.AddHttpClient<CrawlerService>();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    name = "ComFind API",
    status = "Running"
}));

app.MapGet("/api/websites", async (ComFindDbContext db) =>
{
    var websites = await db.Websites.ToListAsync();

    return Results.Ok(websites);
});

app.MapPost("/api/websites", async (Website website, ComFindDbContext db) =>
{
    db.Websites.Add(website);
    await db.SaveChangesAsync();

    return Results.Created($"/api/websites/{website.Id}", website);
});


app.MapPost("/api/websites/{id}/crawl", async (
    int id,
    ComFindDbContext db,
    CrawlerService crawler) =>
{
    var website = await db.Websites.FindAsync(id);

    if (website is null)
    {
        return Results.NotFound();
    }

    await crawler.CrawlWebsiteAsync(website);

    return Results.Ok(new
    {
        message = "Crawl completed",
        websiteId = website.Id,
        website = website.Name
    });
});

app.MapGet("/api/webpages", async (ComFindDbContext db) =>
{
    var pages = await db.WebPages.ToListAsync();

    return Results.Ok(pages);
});

app.MapDelete("/api/websites/{id}", async (
    int id,
    ComFindDbContext db) =>
{
    var website = await db.Websites.FindAsync(id);

    if (website is null)
    {
        return Results.NotFound();
    }

    db.Websites.Remove(website);
    await db.SaveChangesAsync();

    return Results.Ok(new
    {
        message = "Website deleted",
        websiteId = id
    });
});

app.MapGet("/api/search", async (
    string q,
    ComFindDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(q))
    {
        return Results.BadRequest(new
        {
            message = "Search query cannot be empty."
        });
    }

    var results = await db.WebPages
        .Where(page =>
            page.Title.Contains(q) ||
            page.Content.Contains(q))
        .Select(page => new
        {
            page.Id,
            page.WebsiteId,
            page.Url,
            page.Title,
            page.CrawledAt,
            Content = page.Content
        })
        .ToListAsync();

    var searchResults = results.Select(page =>
    {
        var content = page.Content ?? string.Empty;

        var index = content.IndexOf(
            q,
            StringComparison.OrdinalIgnoreCase
        );

        var start = index >= 0
            ? Math.Max(0, index - 100)
            : 0;

        var length = Math.Min(
            250,
            content.Length - start
        );

        var snippet = content.Substring(start, length);

        return new
        {
            page.Id,
            page.WebsiteId,
            page.Url,
            page.Title,
            page.CrawledAt,
            snippet
        };
    });

    return Results.Ok(new
    {
        query = q,
        results = searchResults
    });
});

app.MapDelete("/api/webpages/{id}", async (
    int id,
    ComFindDbContext db) =>
{
    var page = await db.WebPages.FindAsync(id);

    if (page is null)
    {
        return Results.NotFound();
    }

    db.WebPages.Remove(page);
    await db.SaveChangesAsync();

    return Results.Ok(new
    {
        message = "WebPage deleted",
        pageId = id
    });
});

app.Run();