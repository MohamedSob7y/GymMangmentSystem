using GymMangmentsystemBLL.Services.Interface;
using GymMangmentsystemBLL.View_Models.Member_View_Model;
using Microsoft.AspNetCore.Mvc;

namespace GymMangmentSystemPL.Controllers
{
    public class MemberController : Controller
    {
        #region Test Routes
        //public ActionResult Index()
        //{
        //    // return View();
        //    return RedirectToAction(nameof(GetMember));//دا لو موجودة فى نفس الController
        //}
        //public ActionResult GetMember(int id)//عشان اقدر اخد قيمة id من routing لازم اسم id يكون نفس الاسم اللى مكتوب فى Action + ونفس الاسم فى Routing 
        //{
        //    //return View();
        //    //return RedirectToRoute("Trainers"); //هنا كدة لما يعمل Redirect هيروح على Trainer/Index
        //    return RedirectToRoute("Trainers",new { action="Create"}); //طب انا عايز لما  يعمل redirect to anthore route يروح على Trainer/Create
        //    //كدة لما اكتب فى request Member / GetMember هيروح على Trainer/Create ينفذها 
        //}
        //public ActionResult Create()
        //{
        //    return View();
        //} 
        #endregion
        //========================================================
        #region All Actions
        private readonly IMemberServices _memberServices;
        public MemberController(IMemberServices memberServices)
        {
            _memberServices = memberServices;
        }
        //1: Action Index => GetAllMembers From Database ولازم يكون معايا id عشان ابقى اعمله Edite+Remove 
        public ActionResult Index()
        {
            var members = _memberServices.GetallMembers();
            return View(members);
        }
        //Action Get Details=>baseurl/Member/MemberDetails/Id => Take Id From Route
        public ActionResult MemberDetails(int Id)
        {
            //Validation Id
            if (Id <= 0)
            {
                //Error وهرجع نفس الصفحة اللى انا كنت عليها اللى هى GetallMember
                TempData["ErrorMessage"] = "Id Cannot Be Negative or Zero";
                return RedirectToAction(nameof(Index));//Redirect Move Data From Request To Anthore Request So Use TempData عشان ابين فيها المشكلة 

            }
            var member = _memberServices.GetMemberDetails(Id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";//المفروض error دا يتعرض فى Index بتاعت الMember
                return RedirectToAction(nameof(Index));//Redirect Move Data From Request To Anthore Request So Use TempData عشان ابين فيها المشكلة 

            }

            return View(member);
        }
        //Take Id From Route
        public ActionResult HealthRecordDetails(int Id)
        {
            if (Id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot Be Negative or Zero";
                return RedirectToAction(nameof(Index));
            }
            var healthrecord = _memberServices.GetMemberHealthDetails(Id);
            if (healthrecord is null)
            {
                TempData["ErrorMessage"] = "HealthRecord is  Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(healthrecord);
        }
        //Create Action has Two Action with One Service 
        //First Action بتودينى على View فيها Form عشان تاخد الداتا من User وبعدين 
        //Second Action دى تاخد الداتا اللى User دخلها واعملها Submit
        //BaseUrl/Member/Create => this Request
        public ActionResult Create()
        {
            return View();//return view فاضية بيكون فيها Form بقا 
        }
        [HttpPost] //دى نوعها HTTP Post عشان الaction دى تستقبل الداتا من View عشان تعمل Post For Data On Server
        public ActionResult CreateMember(CreateMember createMember)
        {
            if (!ModelState.IsValid)//دى فى حالة ان Data اللى جاية من Form قيها Error لازم تاتكد ان كل الApplication Validation اللى جاية من Form اتطبقت ومفيش اى مشكلة 
            {
                //Error Message
                ModelState.AddModelError("DataInValid", "There are missing Fields");
                return View(nameof(Create), createMember);//يقف عند نفس الصفحة اللى هو فيها اللى هى Create Action  with The Same Data
            }
            bool Result = _memberServices.Create(createMember);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";//عشان هنقل داتا من Request For Anthore Request
                //المفروض لما Create Member Successfully المفروض اروح بقا على All Members واشوف الMember الجديد بعد ماتعمل ول لاء برضو نفس الطكلام واعرض رسالة وبعدين اعملها فى View بتاعت Index
               
            }
            else
            {
                TempData["ErrorMessage"] = "Member Created Successfully";//عشان هنقل داتا من Request For Anthore Request
                //المفروض لما Create Member Successfully المفروض اروح بقا على All Members واشوف الMember الجديد بعد ماتعمل ول لاء برضو نفس الطكلام واعرض رسالة وبعدين اعملها فى View بتاعت Index
            }
            return RedirectToAction(nameof(Index));//With New Data عشان كدة بعمل Redirect مش View بنفس الاسم عشان فيها الداتا القديم انما لما اعمل Redirect with New Data
        }
        #endregion
    }
}
