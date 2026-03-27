using GymMangmentsystemBLL.Services.Interface;
using GymMangmentsystemBLL.View_Models.Trainer_View_Model;
using Microsoft.AspNetCore.Mvc;

namespace GymMangmentSystemPL.Controllers
{
    public class TrainerController : Controller
    {
        #region Test Routes
        //public ActionResult Index()
        //{
        //    return View();
        //}
        //public ActionResult GetTrainers()
        //{
        //    return View();

        //}
        //public ActionResult Create()
        //{
        //    return View();
        //} 
        #endregion
        //=========================================================
        #region Inject Object From ITrainerService
        private readonly ITrainerService _trainerService;
        public TrainerController(ITrainerService trainerService)
        {
           _trainerService = trainerService;
        }
        #endregion
        //=========================================================
        #region All Action
        //=========================================================
        #region Get All Action
        public ActionResult Index()
        {
            var Trainers=_trainerService.GetAllTrainers();
            return View(Trainers);
        }

        #endregion
        //=========================================================
        #region GetDetails Action
        public ActionResult Details([FromRoute]int id)
        {
            if(id<=0)
            {
                TempData["ErrorMessage"] = "Id Cannot be negative or Zero";
                return RedirectToAction(nameof(Index));
            }
            var trainers=_trainerService.GetTrainerDetails(id);
            if(trainers is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainers);
        }

        #endregion
        //=========================================================
        #region Create Trainer Action

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateTrainer(CreateTrainerViewModel createTrainerViewModel)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInValid", "Missing Fields");
                return View(nameof(Create),createTrainerViewModel);
            }
            var Result=_trainerService.CreateTrainer(createTrainerViewModel);
            if(Result)
            {
                TempData["SuccessMessage"] = "Successfully To Create Trainer";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Create Trainer";
            }
            return RedirectToAction(nameof(Index));
        }



        #endregion
        //=========================================================
        #region Update Trainer Action


        #endregion
        //=========================================================
        #region delete Trainer Action


        #endregion
        //=========================================================
        #endregion
    }
}
