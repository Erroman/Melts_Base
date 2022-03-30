// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");
BloggingContext oracleContext = new BloggingContext();
Console.WriteLine(oracleContext.Database.CanConnect());
public class BloggingContext : DbContext
{
    public DbSet<Blog>? Blogs { get; set; }
    public DbSet<Post>? Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseOracle(@"User Id=romanovskiy_vg@vsmpo.ru;Password=GR6JbEe4vFy@g7d;Data Source=oracle19");
    }
}

public class Blog
{
    public int BlogId { get; set; }
    public string? Url { get; set; }
    //public int? Rating { get; set; }
    public List<Post>? Posts { get; set; }
}

public class Post
{
    public int PostId { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }

    public int BlogId { get; set; }
    public Blog? Blog { get; set; }
}
