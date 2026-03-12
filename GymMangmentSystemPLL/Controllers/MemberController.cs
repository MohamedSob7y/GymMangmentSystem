using GymMangmentsystemBLL.Services.Interface;
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
        //Action Get Details=>baseurl/Member/MemberDetails/Id 
        public ActionResult MemberDetails(int Id)
        {
            //Validation Id
            if (Id <= 0)
                //Error وهرجع نفس الصفحة اللى انا كنت عليها اللى هى GetallMember
                return RedirectToAction(nameof(Index));
            
                var member = _memberServices.GetMemberDetails(Id);
                if (member is null)
                  return RedirectToAction(nameof(Index));
                
               
                return View(member);   
        }

        #endregion
    }
}
