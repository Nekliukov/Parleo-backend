using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parleo.DAL.Entities.Configurations
{
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.ToTable("tbl_language").HasKey(l => l.Id);
            builder.Property(l => l.Id).HasColumnName("cln_id").ValueGeneratedOnAdd();
            builder.Property(l => l.Name).HasColumnName("cln_name");
            builder.HasMany(l => l.Events).WithOne(e => e.Language).HasForeignKey(e => e.LanguageId);
        }
    }
}
