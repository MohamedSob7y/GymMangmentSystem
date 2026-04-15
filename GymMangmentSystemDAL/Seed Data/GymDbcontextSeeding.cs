using GymMangmentSystemDAL.Data.Context;
using GymMangmentSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Seed_Data
{
    public class GymDbcontextSeeding
    {
        //this Class Will Has Two Method Seeding To Seed Data From Layer Pl in Folder wwwroot\Seed Files اللى موجودين هنالك
        //object from Class seeding Data Depend On Object from DbContext
        //انا بعمل seeding for Data الىلى موجودين فى Folders معينة  for Tables in Database 
        //So Filses Seeding موجودة فى WWWRoot in Pl
        //Class Will Seeding Data in Dal 

        //this Class Has Two Method 
        public static bool SeedData(GymDbContext dbContext)
        {
            try
            {
                //محتاج اتاكد ان table plan + table Category not has any data before seeding 
                var hasplan = dbContext.Plans.Any();
                var hascategories = dbContext.Categories.Any();
                if (hasplan && hascategories)
                    return false; //لو موجود فيهم اى data مش هعمل اى seeding
                if (!hasplan)
                {
                    //عايز اقرا الاول من Json + ابدا انزلهم عندى بقا واعملهم seeding
                    var plans = LoadDataFromJson<Plan>("plans.json");
                    //كدة خلاص قرات منه 
                    if (plans.Any())//كدة خلاص بقا فيها items
                    {
                        dbContext.AddRange(plans);
                    }

                }
                if (!hascategories)
                {
                    var categoies = LoadDataFromJson<Category>("categories.json");
                    if (categoies.Any())//كدة خلاص بقا فيها items
                    {
                        dbContext.AddRange(categoies);
                    }
                }

                return dbContext.SaveChanges() > 0;

            }
            catch (Exception )
            {
                return false;
            }

        }

        private static List<T> LoadDataFromJson<T>(string filename) //المفروض انا بقرا من Json ومعرفش نوع اللى هرجعه اية 
        {
            //اولا يقدر يقرا على طول من غير الmethod دى لو الفايلات موجودة فى Bin 
            //انما طالما موجودة فى wwwroot اللى هو static Files ساعتها لازم اعمل الحوار دا 
            //هكون الFile Path عشان مش معايا 
            //wwwroot موجود دايما فى Layer  اللى تقدر تعملها Run 
            //يعنى موجود دايما فى Excutable Layer Assemply
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Seed Files", filename);
            //يوصل للPL Layer=>Directory.GetCurrentDirectory()
            // كدة انا كونت الفايل path عشان اقدر اعمل Seeding منه على طول لانه اصلا لو كان موجود فى Bin Folder مكنتش هعمل الكلام دا كله
            if (!File.Exists(filepath)) return [];  //اتاكد ان الpath موجود اصلا 
            //لو موجود ابدا اقرا منه بقا 
            var JsonData=File.ReadAllText(filepath); //Read All Data From This File

            //محتاج بس اعمل حاجة زيادى عشان ممكن ميكنش عامل حساب للCase Senstive عشان كدة لازم اوجهه عشان يعمل حسابه وهو بيقرا 
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };

            //Deserialization
            return JsonSerializer.Deserialize<List<T>>(JsonData)??new List<T>();
        }

        //محتاج تعمل seeding امتى.
        //اول ماApplicaition يتعمل 
        //قبل مايستقبل اى Request 
        //Call this Method For Seeding Data Before Any Request For Application 
        //لازم اعمل seeding Data قبل ما اى request يدخل جوة الapplication
    }
}
