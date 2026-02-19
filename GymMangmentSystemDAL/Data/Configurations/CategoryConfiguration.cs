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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(T => T.CategoryName)
                .HasMaxLength(20);

            //Category With Session
            builder.HasMany(T => T.Sessions)
                    .WithOne(T => T.Category)
                    .HasForeignKey(T => T.CategoryId);



        }
    }
}
