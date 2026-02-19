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
    public class MemberConfiguration : GymUserConfiguration<Member>,IEntityTypeConfiguration<Member>
    {
        //Override For Configure عشان كدة استخدم الNew 
        public new void Configure(EntityTypeBuilder<Member> builder)
        {
            base.Configure(builder);//To Call Configure on GymUserConfiguration
            builder.Property(T => T.CreatedAt).
                HasColumnName("JoinDate").
                HasDefaultValueSql("Getdate()");
        }
    }
}
