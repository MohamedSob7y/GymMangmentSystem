using GymMangmentSystemDAL.Configuration;
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
    public class TrainerConfiguration :GymUserConfiguration<Trainer>, IEntityTypeConfiguration<Trainer>
    {
        public new void Configure(EntityTypeBuilder<Trainer> builder)
        {
            base.Configure(builder);//To Call Configure on GymUserConfiguration
            builder.Property(T => T.CreatedAt).
                HasColumnName("HireDate").
                HasDefaultValueSql("Getdate()");
        }
    }
}
