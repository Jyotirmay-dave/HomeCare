using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeCare_dotnet.Data.Config;

public class OtpConfig : IEntityTypeConfiguration<OTP>
{
    [Obsolete]
    public void Configure(EntityTypeBuilder<OTP> builder)
    {
        builder.ToTable("OTPs");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).UseIdentityColumn();
        builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
        builder.HasCheckConstraint("CK_OTP_Email", "\"Email\" LIKE '_%@_%._%'");
        
        builder.Property(x => x.OtpCode).IsRequired().HasMaxLength(4);
        builder.HasCheckConstraint("CK_OTP_OtpCode", "char_length(\"OtpCode\") = 4");

        builder.Property(x => x.IsUsed).IsRequired();
        builder.Property(x => x.ExpiresAt).IsRequired();
        builder.Property(x => x.CreatedDate).IsRequired().HasDefaultValueSql("timezone('utc', now())");
    }
}
