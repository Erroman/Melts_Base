// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using (var db = new BloggingContext())
{
    Console.WriteLine(db.Database.CanConnect());
    var blog = new Blog { Url = "https://blogs.oracle.com" };
    //var blog = new Blog { Url = "https://blogs.oracle.com", Rating = 10 };
    db.Blogs!.Add(blog);
    db.SaveChanges();
}

using (var db = new BloggingContext())
{
    var blogs = db.Blogs;
    foreach (var item in blogs!)
    {
        Console.WriteLine(item.Url);
        //Console.WriteLine(item.Url + " has rating " + item.Rating );
    }
}
Console.WriteLine("Hello, World!");
//0BloggingContext oracleContext = new BloggingContext();
//Console.WriteLine(oracleContext.Database.CanConnect());
public class BloggingContext : DbContext
{
    public DbSet<Blog>? Blogs { get; set; }
    public DbSet<Post>? Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseOracle(@"User Id=C##BOOKS_ADMIN1;MyPassword;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=oracle19.errdonald.net)))");
        //optionsBuilder.UseOracle(@"User Id=C##BOOKS_ADMIN1;MyPassword;Data Source=oracle19.errdonald.net");
        //optionsBuilder.UseOracle(@"User Id=blog;WWelcome22##;Data Source=atp21c_tpurgent");
        //optionsBuilder.UseOracle(@"User Id=blog;WWelcome22##;Data Source=oracle19.errdonald.net");
        //optionsBuilder.UseOracle(@"User Id=blog;;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=oracle19.errdonald.net)))");
        //optionsBuilder.UseOracle(@"User Id=scott;tiger;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=oracle19.errdonald.net)))");
        //optionsBuilder.UseOracle(@"User Id=scott;tiger;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=oracle19.errdonald.net)))");
        //optionsBuilder.UseOracle(@"User Id=C##blog;Password = dotchka1SS;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=oracle19.errdonald.net)))");
        //optionsBuilder.UseOracle(@"User Id=C##blog;Password = dotchka1SS;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=oracle19.errdonald.net)))");
        //optionsBuilder.UseOracle(@"User Id=blog;Password = dotchka1SS;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=pdb1.errdonald.net)))");//for home computer
        optionsBuilder.UseOracle(@"User Id=blog;Password = dotchka1SS;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME= pdb1.mshome.net)))"); //for work computer
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
