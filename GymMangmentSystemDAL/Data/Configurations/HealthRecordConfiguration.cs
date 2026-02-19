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
    public class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.ToTable("Members").HasKey(T=>T.Id);
            builder.HasOne<Member>()
                .WithOne(T => T.HealthRecord)
                .HasForeignKey<HealthRecord>(T => T.Id);

            #region Problem While Adding Migration
            //لما عملى فايل الMigration كان منزل createdAt + JoinDate المفروض ينزلها مرة واحدة بتغيير اسمها انما هنا نزل الاتنين دا المشكلة 
            //HealthRecord بيورث من Base Entity انا المفروض انزلها بتغيير اسمها انما عمل كدة عشان انا كنت عامل واحدة جديدة باسم JoinData + انه نزل اللى Created At أللى وريثتها من Base Entityt
            builder.Ignore(T => T.CreatedAt); 
            #endregion
        }
    }
}
