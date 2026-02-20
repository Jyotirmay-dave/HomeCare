using System;
using HomeCare_dotnet.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace HomeCare_dotnet.Data;

public class HomecareContext : DbContext
{
    public HomecareContext(DbContextOptions<HomecareContext> options) : base(options) {}

    public DbSet<Admin> Admins { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AdminConfig());
    }
}
