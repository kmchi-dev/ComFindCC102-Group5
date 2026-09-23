using ComFind.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ComFind.Api.Data;

public class ComFindDbContext : DbContext
{
    public ComFindDbContext(DbContextOptions<ComFindDbContext> options)
        : base(options)
    {
    }

    public DbSet<Website> Websites => Set<Website>();

    public DbSet<WebPage> WebPages => Set<WebPage>();

    public DbSet<SearchHistory> SearchHistories => Set<SearchHistory>();
}