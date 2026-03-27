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
        #region Inject Service
        private readonly IMemberServices _memberServices;
        public MemberController(IMemberServices memberServices)
        {
            _memberServices = memberServices;
        }
        #endregion
        //========================================================
        #region Get All Action
        //1: Action Index => GetAllMembers From Database ولازم يكون معايا id عشان ابقى اعمله Edite+Remove 
        public ActionResult Index()
        {
            var members = _memberServices.GetallMembers();
            return View(members);
        }
        #endregion
        //========================================================
        #region Get Details
        //Action Get Details=>baseurl/Member/MemberDetails/Id => Take Id From Route
        public ActionResult MemberDetails(int id)
        {
            //Validation Id
            if (id <= 0)
            {
                //Error وهرجع نفس الصفحة اللى انا كنت عليها اللى هى GetallMember
                TempData["ErrorMessage"] = "Id Cannot Be Negative or Zero";
                return RedirectToAction(nameof(Index));//Redirect Move Data From Request To Anthore Request So Use TempData عشان ابين فيها المشكلة 

            }
            var member = _memberServices.GetMemberDetails(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";//المفروض error دا يتعرض فى Index بتاعت الMember
                return RedirectToAction(nameof(Index));//Redirect Move Data From Request To Anthore Request So Use TempData عشان ابين فيها المشكلة 

            }

            return View(member);
        }
        #endregion
        //========================================================
        #region Get Health Record
        //Take Id From Route
        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot Be Negative or Zero";
                return RedirectToAction(nameof(Index));
            }
            var healthrecord = _memberServices.GetMemberHealthDetails(id);
            if (healthrecord is null)
            {
                TempData["ErrorMessage"] = "HealthRecord is  Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(healthrecord);
        }
        #endregion
        //========================================================
        #region Create Action
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
                TempData["ErrorMessage"] = "Member Failed To Created ";//عشان هنقل داتا من Request For Anthore Request
                //المفروض لما Create Member Successfully المفروض اروح بقا على All Members واشوف الMember الجديد بعد ماتعمل ول لاء برضو نفس الطكلام واعرض رسالة وبعدين اعملها فى View بتاعت Index
            }
            return RedirectToAction(nameof(Index));//With New Data عشان كدة بعمل Redirect مش View بنفس الاسم عشان فيها الداتا القديم انما لما اعمل Redirect with New Data
        }
        #endregion
        //========================================================
        #region Update Action
        //Update Action has Two Action 
        //First Action Send Data To View Then View Will Send Data To Second Action that Post Data On Server
        //طالما فى action بتاخد الId يبقى لازم اعملها فى View asp-Route-id=@member.id 
        public ActionResult MemberEdit(int id)
        {
            if(id<=0)
            {
                TempData["ErrorMessage"] = "Id Cannot be negative or Zero";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberServices.GetMemberDetailsToUpdate(id);
            if (member is  null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        [HttpPost]//Take Data After Change then Post Data To Server 
        //نفس اسم Method اللى فوق بس اختلفنا وميزنا بينهم عن طريق Parameter Overloading + Post اللى فوق كانت Get
        public ActionResult MemberEdit([FromRoute]int id, MemberToUpdateViewModel memberToUpdateView)//بدل ماحط الId هناك فى View بدل كدة لازم ابعته هنا عشان احدد بس مين بالظبط الMember اللى اعمله Update بالتالى انا بقؤله هات الId From Route
        {
            //So Will Take Data From Viewmodel [MemberToUpdateViewModel] and Take Id From Route=> الحركة دى لازم تكون نفس اسم Action دى هى هى نفس اسم الaction اللى فوق 
            if(!ModelState.IsValid)
            {//Validation For Application Validation لان فى طريقة لحذف الvalidation عادى 
                ModelState.AddModelError("DataInValid", "There are missing Fields");
                return View(memberToUpdateView);//عشان دى نفس اسم اللى فوق مش هقؤل name of (MemberEdit)
            }
            var Result=_memberServices.UpdateMember(id, memberToUpdateView);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed To Updated ";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion
        //========================================================
        #region Delete Action
        //First Action => return View has Page Confirmation
        //Second Action Post Delete On Server
        //المفروض لما ادوسعلى الزرار بتاع Delete Member => يودينى على Action دى 
        public ActionResult Delete([FromRoute]int id)//Take Id From [Route] عشان الRequest اصلا بنفس اسم Action دى انما اللى تحت مش هتعرف تاخده عشان مش نفس الاسم 
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot be negative or Zero";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberServices.GetMemberDetails(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            //==============================================
            #region Solve Problem With Id 
            //1:
            //TempData["Id"]=member.Id;//واعملها هناك فى Delete View as hidden input عادى او اعملها فى asp-reoute-id=@TempData["Id"]=> مشكلة الحل دا اية هو 
            //المشكلة انى مش ضامن الSecond Request هى deleteConfirmed  ممكن تكون BackToList
            //=====================================================================
            //2: r eturn View(member);//دا حل عشان التانيه تعرف تاخد الId من هنا  وفى ال View بتاعت الDelete اعمله هناك asp-route
            //دى مشكلة انى مرجع الMember كله وانا بس عايز Id
            //=====================================================================
            //Final Solution=> احسن حل 
            //عايز انقل داتا لنفس الRequest وبعدين اخد الداتا دى انقلها باديى الى Second Request 
            ViewBag.MemberId =member.Id;//=> المفروض استقبلها فى Delete View For Second Request Delete Confirmed
            ViewBag.MemberName = member.Name;//عشان اتاكد انه هيمسح الMember دا 
            return View();
            //=====================================================================
            #endregion
            //==============================================
            //return View();//Without Any Data مش عايز اخد اى داتا معايا باخدها فقط فى Update
           
        }
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)//Take Id From []
        {
            //still فى مشكلة ان الAction دى مش عارفة تاخد الId من نفس الRequest بتاع الDelete اللى فوق عشان DeletedConfiremd مش نفس الاسم بااتلى فى مشكلة 
            var Result=_memberServices.RemoveMember(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed To Delete ";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
        //========================================================
        #endregion
    }
}
