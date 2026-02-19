using GymMangmentSystemDAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Data.Context
{
    public class GymDbContext:DbContext
    {

        #region Open Connection
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-9DG3E18;Database=GymMangmentSystem;Trusted_Connection=true;TrustServerCertificate=true");
        }
        #endregion
        //===============================================
        #region Apply Configurations
        //To Call any object  implement interface Ientityttypeconfiguration in Runtime => To make Fluent Api Configuration
        //هو بيقؤل هاتلى الExtcutable Project => In Layer PL دى الوحيدة اللى Run + Excute طب ازاى برضو الLayer دى تقدر توصل For Configuration in DAL Layer by References in Dependencies 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

           
        }
        
        
        #endregion
        //===============================================
        #region Dbset
        public DbSet<Member> Members { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<MemberShip> MemberShips { get; set; }
        public DbSet<MemberSession> MemberSessions { get; set; }
        #endregion
        //===============================================
        #region Add Migrations
        //وانا بكتب الMigration لازم default Project in DAL ولو عندى اكتر من Context بكتبه 
        //Add-Migration  "FirstMigration" -StartUpProject لو هعملها بالكود ول لاء احددها من فوق اصلا  
        //Add-Migration  "First Create" -OutputDir "Data/Migrations" علشان يحطها فى Folder Data/Migration
        //ظهرت مشكلة 

        #endregion

    }
}
