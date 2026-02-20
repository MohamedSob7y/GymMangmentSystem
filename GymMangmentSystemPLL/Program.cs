using GymMangmentSystemDAL.Data.Context;
using GymMangmentSystemDAL.Entities;
using GymMangmentSystemDAL.Repository.Generic_Repository.Implementation;
using GymMangmentSystemDAL.Repository.Generic_Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GymMangmentSystemPLL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region Create + Apply Service for Application
            //Create Application
            var builder = WebApplication.CreateBuilder(args);
            //Make Services To Build this Application on server
            builder.Services.AddControllersWithViews(); 
            #endregion
            //==========================================
            #region Ask Clr to inject object
            #region Old Way to inject Dbcontext
            //Ask CLr to inject object from GymDbcontext => Will Take Optios From OnConfigure 
            //MemberReposityro Construcotr chain GymDbcontext Construcotr chain for Dbcontext Constrcuotr chain Construcotr that take options from On Configure 
            //فبعمل كل الحتة دى لو هاخد الOptions بالطريقة اللى شرحتها هنا طب لو مش عارزها كدة 
            //builder.Services.AddDbContext<GymDbContext>(); 
            #endregion
            //==========================================
            #region New Way to inject object From Dbcontext
            //Will Take Options From  
            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                #region UnSafe
                //options.UseSqlServer("Server=DESKTOP-9DG3E18;Database=GymMangmentSystem;Trusted_Connection=true;TrustServerCertificate=true"); 
                #endregion
                //============================================
                #region Add Connection string in AppSetting.Development Not Appsetting General and Call it in This Options
                //Section is ConnectionStrings
                //Key : DefaultConnection
                //Value اللى موجودة داخل DefaultConnection
                //options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
                //options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["DefaultConnection"]);
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                #endregion
                //============================================
            });

            #endregion
            //==========================================
            #region Object from IGeneric Repository
            //Ask CLR to inject object from any class implement interface IGeneric Repository 
            builder.Services.AddScoped(typeof(IGenericRepository<Member>), typeof(GenericRepository<Member>));//دا للMember only طب منا كدة هفضل اعمل لكل واحد Session+Plan and So on ودا مش احسن حاجة 
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));//دا لاى Entityt
            #endregion
            //==========================================
            #endregion
            //==========================================
            #region Build + Configuration+ Run application
            //Build Application on server to Recieve any Request 
            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
            //Run Application 
            app.Run(); 
            #endregion
        }
    }
}
