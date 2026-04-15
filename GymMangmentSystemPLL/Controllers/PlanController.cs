using GymMangmentsystemBLL.Services.Interface;
using GymMangmentsystemBLL.View_Models.Plan_View_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GymMangmentSystemPL.Controllers
{
    public class PlanController : Controller
    {
        #region Inject Object From Service
        private readonly IPlanService _planService;
        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }
        #endregion
        //===============================================
        #region all Actions
        //===============================================
        #region Get All Plan 
        public ActionResult Index()
        {
            var plans=_planService.GetAllPlans();
            return View(plans);
        }
        #endregion
        //===============================================
        #region Get Plan Details
        public ActionResult Details(int id)
        {
            if(id<=0)
            {
                TempData["ErrorMessage"] = "Id Cannot be Negative or Zero";
                return RedirectToAction(nameof(Index));
            }
            var plan=_planService.GetPlanDetails(id);
            if(plan is null)
            {
                TempData["ErrorMessage"] = "Plan Is Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        #endregion
        //===============================================
        #region update Plan
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot be Negative or Zero";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanToUpdate(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Is Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
            //===============================================
            //لو هعمل Resuse For Details Action 
            //return Details(id);
            //===============================================
        }
        [HttpPost]
        public ActionResult Edit([FromRoute]int id,[FromForm]PlanToUpdateVM planToUpdateVM)
        {
            //Take Id From Route From The same Request Of First Action Edit then Take Other Data From form in ViewModel 
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "There are missing Fields");
                return View(planToUpdateVM);//اوقفه على نفس الصفحة 
            }
            var Result=_planService.UpdatePlan(id, planToUpdateVM);
            if(Result)
            {
                TempData["SuccessMessage"] = "Plan Success To Update";
            }
            else
            {
                TempData["ErrorMessage"] = "Plan Failed To Update";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
        //===============================================
        #region Activate
        //This Method From Type HTTPPOst المفروض بعمل فى Index View المفروض يكون فيها Form for activation المفروض بقا الForm دى تكلم Action From Type Httppost فلازم اعمل الMethod دى على طول طب بتاحد الId منين؟
        //This Method Will Take Id From => []
        //الفرق بين انى اكتب Return View(nameof(Index))=>هنا المفروض هو بعت Reqest مرة واحدة بااتلى لو غيرت حاجة مش هيبعت Reqest تانى بالتالى الداتا مش Updated
        //return Redirecttoaction(nameof(index))=> هنا الداتا بتبقى Updated معايا عشان هو بيبعت Reqest كمان للDatabase عشان الReqest اللى انا عامله
        [HttpPost] 
        public ActionResult Activate([FromRoute]int id) //لو باخده من Form يعنى معمولى فى الForm as input hidden المفروض اعملها [FromForm]
        {
            var Result = _planService.ToggleStatus(id);
            if (Result)//If True 
            {
                TempData["SuccessMessage"] = "Plan Toggled Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Plan Failed To Toggled";
            }
            return RedirectToAction(nameof(Index));

        }

        #endregion
        //===============================================
        #endregion
    }
}
