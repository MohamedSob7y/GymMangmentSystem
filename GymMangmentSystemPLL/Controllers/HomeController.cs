using GymMangmentSystemDAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymMangmentSystemPL.Controllers
{
    public class HomeController : Controller
    {
        #region Explain Controller
        //Request For This Action [BaseURL/Nameofcontroller/NameOfAction]
        //[BaseUrl/Home/Index] //دا كدة هيروح على HomeController=> Index 
        //Action عشان اسميها كدة محتاج تكون 
        //1: Inside Controller Inherit from Controller is Parent Class
        //Any MVc Controller بيورث من Controller اللى عنده كل Fetures of MVc => Controller بيورث من Controllerbase اللى عنده Api Features
        //Any Api Controller بيورث من Controller base اللى اصلا عنده مل Features of Api 
        //Object From Controller depend on Object from Services 
        //Each ACtion Call Logic in Service والمفروض تكون بتعمل حاجة واحدة بس => GetAll مثلا then return view for user 
        //مش هينفع الactiion تكون Geeneric + Static + Contructor Or setter /getter
        //مش هينفع تكون نفس اسم اى method موجودة فى class اللى بورث منه 
        //يعنى مثلا انا بورث من Controller عنده method اسمها view بالتالى مش هينفع اعمل Action نفس الاسم دا 
        //Must Be Public 
        //مش هينفع تكون Extention method
        //مش هينفع الParameters اللى جوهاه يخدوا passing by out / ref
        //Flow =>  Url اللى بيستقبله قبل مايدخل جوه الapp is Middleware /Pipline make Configurations for this Request 
        //then يدخل جوه الConroller اللى عنده action بتروح تنادى على GetAll مثلا and return view for user
        public IActionResult Index()
        {
            return View();
        }
        #endregion
        //=========================================
        #region Action Return Type
        ////1: ViewResult=>return ViewPage
        ////[BaseUrl/Home/Index]
        //public ViewResult Index()
        //{
        //    //كل action return viewلازم تكون موجودة بنفس اسم الAction 
        //    //Name of View Same Name of Action
        //    return View();//using Helper Method ورثتها من Controller
        //    //var Result=new ViewResult();
        //    //return Result;
        //}
        ////2: JsonResult => return Json Data
        ////[Baseurl/Home/Trainers]
        //public JsonResult Trainers()
        //{
        //    var Trainers = new List<Trainer>()
        //    {
        //        new Trainer(){Name="Mohamed",Phone="0102569495"},
        //         new Trainer(){Name="Ahmed",Phone="010155655495"}
        //    };
        //    return Json(Trainers);
        //}

        ////RedirectResult=> Return Redirect to action or To Anthore Page using URL
        ////[Baseurl/Home/RedirectMethod]
        //public RedirectResult RedirectMethod()
        //{
        //    //Status Of Response => 300
        //    return Redirect("https://www.linkedin.com/in/mohamed-sobhy-519b8b320/");//ممكن اعمل redirect to anthore Action or anthore View عادى مش شرط لينك عادى 
        //}

        ////ContentResult=> Return Text with HTML and CSS
        ////[Baseurl/Home/ContentText]
        //public ContentResult ContentText()
        //{
        //    // return Content("Hello Mohamed");//return Text/plain طب انا عايز ارجعه text/html
        //    return Content("<h1>Hello Mohamed<h1>", "text/html");//return Text/Html
        //    //حتى لو حطيت الTages من غير مافاهمه ان يكون Contenttype بتاعه text/html برضو مش هيفهم بالتالى لازم اقؤله انا عايز الtype يكون  text/html
        //}
        ////FileResult=> Return File has Data
        ////[Baseurl/Home/FileMethod]
        //public FileResult FileMethod()
        //{
        //    var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "css", "site.css");
        //    var filebytes = System.IO.File.ReadAllBytes(filepath);//استخدمت الحتة دى لان لو استخدمت File هيشوفها انها helper method بترجع ال File مش class 
        //    return File(filebytes, "text/css", "MyFile");
        //    //كدة انا جبت الفايل قرياته بالBytes 
        //    //المفروض احدد الtype then احدد ال اسمه لما ينزل 
        //}

        //ممكن كل action ترجع اكتر من return type 
        //public ActionResult TypesofAction() //عشان كدة مش بستخدم اى return type واحددها لانى معرفش انا استخدم actionResult 
        //    //لان كل Return types بيورثوا من actionResult
        //{
        //    int x = 10, y = 20;
        //    if (x == y)
        //        return View();
        //    else
        //        return NotFound();
        //}
        //بيتعمل لكل Controller Folder Views تحتوى على كل views اللى اتعملت جوه الController كلها 
        //يعنى لكل Action return view بيتعمل view بنفس اسم action in Folder Controller لو مش موجودة هتلاقيها فى Folder Shared 
        //So For Each Action has View بنفس الاسم عشان تعرض الPages للuser 
        #endregion
        //=========================================
    }
}
