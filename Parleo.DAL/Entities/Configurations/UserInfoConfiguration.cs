using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Parleo.DAL.Entities.Configurations
{
    public class UserInfoConfiguration : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.ToTable("tbl_user_info").HasKey(ui => ui.Id);
            builder.Property(ui => ui.Id).HasColumnName("cln_id").ValueGeneratedOnAdd();
            builder.Property(ui => ui.Name).HasColumnName("cln_name");            
            builder.Property(ui => ui.Birthdate).HasColumnName("cln_birth_date").HasColumnType("Date");
            builder.Property(ui => ui.Gender).HasColumnName("cln_gender");
            builder.Property(ui => ui.Latitude).HasColumnName("cln_latitude").HasColumnType("decimal(10, 2)");
            builder.Property(ui => ui.Longitude).HasColumnName("cln_longitude").HasColumnType("decimal(11, 8)");
            builder.Property(ui => ui.CreatedAt).HasColumnName("cln_created_at");            
        }
    }
}
