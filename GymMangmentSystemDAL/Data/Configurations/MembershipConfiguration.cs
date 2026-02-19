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
    internal class MembershipConfiguration : IEntityTypeConfiguration<MemberShip>
    {
        public void Configure(EntityTypeBuilder<MemberShip> builder)
        {
            //فى حالة واحدة بس هيكون فيها Id اللى ورثته من Base Entityt is PK فى حالة لو Member يقدر بعد ما الPlan تخلص يقدر يحجزها تانى عادى ساتعها كدة كررت MemberId with PlanId تانى ودا المفروض غلط ساعتها هخلى Id اللى ورثته هو الPK
            builder.Ignore(T => T.Id);
            builder.Ignore(T => T.Status);
            /*builder.HasOne(T => T.Member)
                .WithMany(T => T.Memberships)
                .HasForeignKey(T => T.MemberId);
            builder.HasOne(T => T.Plan)
              .WithMany(T => T.Memberships)
              .HasForeignKey(T => T.PlanId);*/
            builder.HasKey(T =>new {T.MemberId, T.PlanId}); 
            builder.Property(T => T.CreatedAt)
                .HasColumnName("StartDate")
                .HasDefaultValueSql("GetDate()");
        }
    }
}
