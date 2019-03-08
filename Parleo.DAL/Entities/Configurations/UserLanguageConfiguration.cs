using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parleo.DAL.Entities.Configurations
{
    public class UserLanguageConfiguration : IEntityTypeConfiguration<UserLanguage>
    {
        public void Configure(EntityTypeBuilder<UserLanguage> builder)
        {
            builder.ToTable("tbl_user_language").HasKey(ul => new { ul.UserId, ul.LanguageId });
            builder.Property(ul => ul.LanguageId).HasColumnName("cln_language_id");
            builder.Property(ul => ul.UserId).HasColumnName("cln_user_id");
            builder.Property(ul => ul.Level).HasColumnName("cln_level");
            builder.HasOne(ul => ul.Language).WithMany(l => l.UserLanguages).HasForeignKey(ul => ul.LanguageId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(ul => ul.UserInfo).WithMany(ui => ui.Languages).HasForeignKey(ul => ul.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
