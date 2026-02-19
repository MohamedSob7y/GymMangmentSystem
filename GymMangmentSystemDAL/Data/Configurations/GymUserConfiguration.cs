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
    //دا لو خليته implement interface :IEntityTypeConfiguration<GymUser> => ساعتها لازم يتحول الى table in Database عشان كدة مش هينفع اخليه implement interface :IEntityTypeConfiguration<GymUser>
    //Solution make this=>
    //لازم اللى يجى يعمل implement يكون بيورث من GymUser ومفيش عندى غير اتنين هما اللى بيورثوا من GymUser=> Member+ Trainer
    public class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(X => X.Name).HasColumnName("Name")
                .HasColumnType("varchar(50)");
            //====================================================================
            builder.Property(X => X.Email)
                .HasColumnName("Email").HasMaxLength(100);
            builder.ToTable(T => T.HasCheckConstraint("EmailValidformatConstrain","Email like '_%@_%._%'"));//Formate Email    '_%@_%._%' At Least One Character need For check Constrain
          //Unique [Non Clustered index For any Column معادا الPK] Constrain in Email
          //For Pk has Clutred Index only
          builder.HasIndex(T=>T.Email).IsUnique();
            //====================================================================
            builder.Property(T => T.Phone).HasColumnType("varchar(11)");
            builder.ToTable(T => T.HasCheckConstraint("PhoneConstrain","Phone like '01[125]%' "));//Phone Format '01[125]%' and Phone not like '%[^0-9]%'
            builder.HasIndex(T=>T.Phone).IsUnique();

            //====================================================================
            //Address is Owned
            builder.OwnsOne(X => X.Address, AddressBuilder =>
            {
                AddressBuilder.Property(T => T.Street).HasColumnType("varchar(30)");
                AddressBuilder.Property(T => T.City).HasColumnType("varchar(30)");
            });
        }
    }
}
