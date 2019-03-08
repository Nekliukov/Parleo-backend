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
            
        }
    }
}
