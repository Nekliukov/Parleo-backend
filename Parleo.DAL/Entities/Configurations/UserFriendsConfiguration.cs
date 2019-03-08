using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parleo.DAL.Entities.Configurations
{
    public class UserFriendsConfiguration : IEntityTypeConfiguration<UserFriends>
    {
        public void Configure(EntityTypeBuilder<UserFriends> builder)
        {
            builder.ToTable("tbl_user_friends").HasKey(uf => new { uf.UserFromId, uf.UserToId });
            builder.Property(uf => uf.UserFromId).HasColumnName("cln_user_from");
            builder.Property(uf => uf.UserToId).HasColumnName("cln_user_to");
            builder.Property(uf => uf.Status).HasColumnName("cln_status");
            builder.HasOne(uf => uf.UserFrom).WithMany(ui => ui.Friends).HasForeignKey(uf => uf.UserFromId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(uf => uf.UserTo).WithMany(ui => ui.Friends).HasForeignKey(uf => uf.UserToId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
