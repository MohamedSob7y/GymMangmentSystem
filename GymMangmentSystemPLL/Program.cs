using AutoMapper;
using GymMangmentsystemBLL.Mapping;
using GymMangmentSystemDAL.Data.Context;
using GymMangmentSystemDAL.Entities;
using GymMangmentSystemDAL.Repository.Generic_Repository.Implementation;
using GymMangmentSystemDAL.Repository.Generic_Repository.Interface;
using GymMangmentSystemDAL.Repository.Implementation;
using GymMangmentSystemDAL.Repository.Interface;
using GymMangmentSystemDAL.Seed_Data;
using GymMangmentSystemDAL.Unit_Of_Work.Implementation;
using GymMangmentSystemDAL.Unit_Of_Work.Interface;
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
            #region Object From IplanRepository
            builder.Services.AddScoped(typeof(IPlanRepository),typeof(PlanRepository));
            #endregion
            //==========================================
            #region Object From IUnitOfWork
            //هنا مش محتاجين  Object from IGeneric Repository
            builder.Services.AddScoped(typeof(IUniteOfWork),typeof(UniteOfWork));
            //بمجرد اننتهاء Request => CLr Dispose object from Heap by default 
            //ممكن تلاقى UnitofWork implement interface Idisposable 
            #endregion
            //==========================================
            #region Object From ISessionRepository
            builder.Services.AddScoped(typeof(ISessionRepository),typeof(SessionRepository));
            #endregion
            //==========================================
            #region Object From IMapper
            //use AddProfile عشان يعرف يوصل للConfiuration in BLL اللى متعلم فيها انه يحول من object To Object
            builder.Services.AddAutoMapper(T => T.AddProfile(typeof(IMapper)));
            //builder.Services.AddAutoMapper(T => T.AddProfile(new MappingProfile())));
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
            //==========================================
            #region Call Methods Seeding 
            //امتى محتاج اعمل Seeding or Calling This Method to Seeding Data=> اول ما الapplication بتاعى يستغل وقبل ماسيتقبل اى request 

            //ومش هعرف اخلى Programe inject object from RunTime => لان مش هينفع اعمل Constrcutor هنا فى Main => So make this Manual 
            //GymDbContext dbContext = new GymDbContext();//مش هبنفع برضو عشان محتاج options لو انا عملته Manual
            //=================================================================================================================================
            using var scope =app.Services.CreateScope();//Create Scope that has all Object For Scoped Lifetime
            //Get object From Scoped وعايز بعد مااجيبه واستخدمه خلاص يمسحه من heap على طول 
            var dbcontext = scope.ServiceProvider.GetRequiredService<GymDbContext>(); //Servies Providers عارف الاماكن جوه الScope
            var peningMigration = dbcontext.Database.GetPendingMigrations();//دا يرجعلى كل Migrations اللى لسة متعملهاش appying in Dataabase 
            if(peningMigration?.Any()??false)//عايز اتاكد ان مفيش اى Changes in application  مسمعتشى فى Database يعنى لازم تاتكد ان كل Migrations اتعملها Update in Database
            {
                //لازم تاتكد ان كل التغيرات حصلت فى Database عشان ميحصلشى اى مشكلة 
                dbcontext.Database.Migrate();
               //ممكن يكون فى Column IsActive in Table Plans ولسة معملتش التغيير دا فى Database فبالتالى العمود دا مش موجود اصلا وبعدها لوحت عامل seeding For Data for IsActive اللى هو اصلا مش موجود فبالتالى مش هينفع عشان كدة محتاج اتاكد ان كل حاجة سمعت فى dataabase عشان اقدر بعدها اعمل seeding Data for this Tables 
            }
            //انا بقا هخلى برضو Clr يعمله بطريقة غير مباشرة تسمى Explicit injections =>يعنى هطلبه منك بشكل صريح وانت تديهونى 
            //انما Implcicit injecttion Ask CLR to inject onject by Constrcutor بطلبه بس بطريقة غير مباشرة 
            GymDbcontextSeeding.SeedData(dbcontext);//هنا محتاج object From GymDbContext 
            //يبقى الفرق بين Implcicit injection  + Explcicit injection=> Implcicit Ask Clr to inject object in Runtime بطلبه منك بطريقة غير مباشرة تعملهولى Automatic
            //Explciti بطلبه منك بصراحة انك انت برضو اللى تعمله بطريقة Manual Not Automatic 

            #endregion
        }
    }
}
