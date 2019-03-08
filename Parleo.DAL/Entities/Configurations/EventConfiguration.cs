using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parleo.DAL.Entities.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("tbl_event").HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("cln_id").ValueGeneratedOnAdd();
            builder.Property(e => e.Name).HasColumnName("cln_name");
            builder.Property(e => e.Description).HasColumnName("cln_description");
            builder.Property(e => e.MaxParticipants).HasColumnName("cln_max_participants");
            builder.Property(e => e.Latitude).HasColumnName("cln_latitude").HasColumnType("decimal(10, 2)");
            builder.Property(e => e.Longitude).HasColumnName("cln_longitude").HasColumnType("decimal(11, 8)"); ;
            builder.Property(e => e.IsFinished).HasColumnName("cln_is_finished");
            builder.Property(e => e.StartTime).HasColumnName("cln_start_time");
            builder.Property(e => e.EndDate).HasColumnName("cln_end_date");
            builder.Property(e => e.CreatorId).HasColumnName("cln_creator_id");
            builder.Property(e => e.LanguageId).HasColumnName("cln_language_id");
            builder.HasOne(e => e.Creator).WithMany(ui => ui.Events).HasForeignKey(e => e.CreatorId).IsRequired();
            builder.HasOne(e => e.Language).WithMany(l => l.Events).HasForeignKey(e => e.LanguageId).IsRequired();
        }
    }
}
