using GymMangmentsystemBLL.Services.Implementation;
using GymMangmentsystemBLL.Services.Interface;
using GymMangmentsystemBLL.View_Models.Member_View_Model;
using GymMangmentsystemBLL.View_Models.MembershipViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GymMangmentSystemPL.Controllers
{
    public class MembershipController : Controller
    {
        private readonly IMembershipService _membershipService;

        public MembershipController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }
        // ================= Index =================
        public IActionResult Index()
        {
            var memberships = _membershipService.GetAll();
            return View(memberships);
        }

        // ================= Create (GET) =================
        public IActionResult Create()
        {
            return View();
        }

        // ================= Create (POST) =================
        [HttpPost]
        public ActionResult Create(CreateMembershipVM vm)
        {
            if (!ModelState.IsValid)//دى فى حالة ان Data اللى جاية من Form قيها Error لازم تاتكد ان كل الApplication Validation اللى جاية من Form اتطبقت ومفيش اى مشكلة 
            {
                //Error Message
                ModelState.AddModelError("DataInValid", "There are missing Fields");
                return View(nameof(Create), vm);//يقف عند نفس الصفحة اللى هو فيها اللى هى Create Action  with The Same Data
            }
            bool Result = _membershipService.Create(vm);
            if (Result)
            {
                TempData["SuccessMessage"] = "MemberShip Created Successfully";//عشان هنقل داتا من Request For Anthore Request
                                                                           //المفروض لما Create Member Successfully المفروض اروح بقا على All Members واشوف الMember الجديد بعد ماتعمل ول لاء برضو نفس الطكلام واعرض رسالة وبعدين اعملها فى View بتاعت Index
                return RedirectToAction(nameof(Index));//With New Data عشان كدة بعمل Redirect مش View بنفس الاسم عشان فيها الداتا القديم انما لما اعمل Redirect with New Data
            }
            else
            {
                TempData["ErrorMessage"] = "MemberShip Failed To Created ";//عشان هنقل داتا من Request For Anthore Request
                return View(nameof(Create), vm);
                //المفروض لما Create Member Successfully المفروض اروح بقا على All Members واشوف الMember الجديد بعد ماتعمل ول لاء برضو نفس الطكلام واعرض رسالة وبعدين اعملها فى View بتاعت Index
            }
        }

        // ================= Cancel =================
        public ActionResult Cancel(int memberId, int planId)
        {
            bool result = _membershipService.Cancel(memberId, planId);

            if (result)
            {
                TempData["Error"] = result;
            }

            return RedirectToAction("Index");
        }
       
    }
}
