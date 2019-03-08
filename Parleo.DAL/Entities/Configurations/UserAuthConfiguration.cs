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
            
        }
    }
}
