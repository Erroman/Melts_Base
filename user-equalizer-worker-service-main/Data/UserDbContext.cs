﻿using Microsoft.EntityFrameworkCore;
using UserEqualizerWorkerService.Models;

namespace UserEqualizerWorkerService.Data;

public class UserDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder options) => 
        options.UseSqlite("DataSource = userBD; Cache=Shared");

    public DbSet<User> Users { get; set; }
    public DbSet<Address> Address { get; set; }
    public DbSet<Geo> Geo { get; set; }
    public DbSet<Company> Company { get; set; }
}   