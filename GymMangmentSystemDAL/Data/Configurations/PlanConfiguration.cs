using GymMangmentSystemDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Data.Configurations
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(T => T.Name).HasColumnType("varchar(50)");
            builder.Property(T => T.Description).HasColumnType("varchar(200)");
            builder.Property(T => T.Price).HasColumnType("decimal(10,2)").HasPrecision(10,2);
            builder.ToTable(T => T.HasCheckConstraint("DurationDaysConstrain", "DurationDays between 1 and 364"));
        }
    }
}
