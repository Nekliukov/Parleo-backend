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
            
        }
    }
}
