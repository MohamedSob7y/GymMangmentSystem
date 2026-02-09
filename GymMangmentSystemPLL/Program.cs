namespace GymMangmentSystemPLL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Create Application
            var builder=WebApplication.CreateBuilder(args);
            //Make Services To Build this Application on server
            builder.Services.AddControllersWithViews();
            //Build Application on server to Recieve any Request 
            var app=builder.Build();
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
        }
    }
}
