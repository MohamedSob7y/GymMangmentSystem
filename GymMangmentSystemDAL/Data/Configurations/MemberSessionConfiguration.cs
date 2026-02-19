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
    public class MemberSessionConfiguration : IEntityTypeConfiguration<MemberSession>
    {
        public void Configure(EntityTypeBuilder<MemberSession> builder)
        {
            builder.Property(T => T.CreatedAt)
                .HasColumnName("BookingDate").HasDefaultValueSql("GetDate()");
            builder.Ignore(T => T.Id);
           /* builder.HasOne(T => T.Member)
                .WithMany(T => T.MemberSessions)
                .HasForeignKey(T => T.MemberId);
            builder.HasOne(T => T.Session)
               .WithMany(T => T.MemberSessions)
               .HasForeignKey(T => T.SessionId);*/
            builder.HasKey(T => new {T.MemberId,T.SessionId });
        }
    }
}
