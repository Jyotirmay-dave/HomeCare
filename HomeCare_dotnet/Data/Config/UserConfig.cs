using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeCare_dotnet.Data.Config;

public class UserConfig : IEntityTypeConfiguration<User>
{
    [Obsolete]
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).UseIdentityColumn();
        builder.Property(x => x.UserName).IsRequired().HasMaxLength(60);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasCheckConstraint("CK_Users_Email", "\"Email\" LIKE '_%@_%._%'");
    }
}
