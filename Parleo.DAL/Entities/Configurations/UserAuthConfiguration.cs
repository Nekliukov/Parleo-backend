using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parleo.DAL.Entities.Configurations
{
    public class UserAuthConfiguration : IEntityTypeConfiguration<UserAuth>
    {
        public void Configure(EntityTypeBuilder<UserAuth> builder)
        {
            builder.ToTable("tbl_user_auth").HasKey(ua => ua.UserInfoId);
            builder.Property(ua => ua.UserInfoId).HasColumnName("cln_user_info_id");
            builder.Property(ua => ua.Email).HasColumnName("cln_email");
            builder.Property(ua => ua.PasswordHash).HasColumnName("cln_password_hash");
            builder.Property(ua => ua.PasswordSalt).HasColumnName("cln_password_salt");
            builder.Property(ua => ua.LastLogin).HasColumnName("cln_last_login");
            builder.HasOne(ua => ua.UserInfo).WithOne(ui => ui.UserAuth).HasForeignKey<UserAuth>(ua => ua.UserInfoId)
                .IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
