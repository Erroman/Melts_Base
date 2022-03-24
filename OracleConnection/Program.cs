// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");
public class BloggingContext : DbContext
{
    public DbSet<Blog>? Blogs { get; set; }
    public DbSet<Post>? Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseOracle(@"User Id=blog;Password=<Password>;Data Source=<Net Service Name>");
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
