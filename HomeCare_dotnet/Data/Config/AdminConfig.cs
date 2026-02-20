using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeCare_dotnet.Data.Config;

public class AdminConfig : IEntityTypeConfiguration<Admin>
{
    [Obsolete]
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.ToTable("Admins");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).UseIdentityColumn();
        builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasCheckConstraint("CK_Admins_Email", "\"Email\" LIKE '_%@_%._%'");
        
        builder.Property(x => x.UserName).IsRequired().HasMaxLength(60);
        builder.Property(x => x.Password).IsRequired();
        builder.Property(x => x.PasswordSalt).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.IsDeleted).IsRequired();
        builder.Property(x => x.CreatedDate).IsRequired().HasDefaultValueSql("timezone('utc', now())");
    }
}
