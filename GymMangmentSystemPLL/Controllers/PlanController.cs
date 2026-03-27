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





        //===============================================
        #endregion
    }
}
