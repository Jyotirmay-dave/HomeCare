using System;
using HomeCare_dotnet.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace HomeCare_dotnet.Data;

public class HomecareContext : DbContext
{
    public HomecareContext(DbContextOptions<HomecareContext> options) : base(options) {}

    public DbSet<Admin> Admins { get; set; }
    public DbSet<OTP> OTPs { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AdminConfig());
        modelBuilder.ApplyConfiguration(new OtpConfig());
        modelBuilder.ApplyConfiguration(new UserConfig());
    }
}
