using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManager.Common.Contants;
using StudentManager.Data.Entities;

namespace StudentManager.Data.Configurations
{
    public class StudentClassConfiguration : IEntityTypeConfiguration<StudentClass>
    {
        public void Configure(EntityTypeBuilder<StudentClass> builder)
        {
            // Properties
            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(MaxLengths.TITLE);

            builder.Property(p => p.Description)
                   .HasMaxLength(MaxLengths.DESCRIPTION);
        }
    }
}
