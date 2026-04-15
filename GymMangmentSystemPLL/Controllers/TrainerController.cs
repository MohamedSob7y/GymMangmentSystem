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
                TempData["SuccessMessage"] = "Trainer Created Successfully";
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
        public ActionResult Edit([FromRoute]int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot be negative or Zero";
                return RedirectToAction(nameof(Index));
            }
            var trainers = _trainerService.GetTrainerDetailsToUpdate(id);
            if (trainers is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainers);
        }
        [HttpPost]
        //Take Id from Route + باقى الداتا من Viewmodel
        //Will Take Id From The The Same Request Of this Action Edit اللى هى بتعمل Send Data To View Not Post Data on Server
        public ActionResult Edit([FromRoute]int id,TrainerToUpdateViewModel trainerToUpdateViewModel)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInValid", "Missing Field");
                return View(nameof(Edit),trainerToUpdateViewModel);
            }
            var result=_trainerService.UpdateTrainer(id, trainerToUpdateViewModel);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Update Trainer";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion
        //=========================================================
        #region delete Trainer Action
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot be negative or Zero";
                return RedirectToAction(nameof(Index));
            }
            var trainers = _trainerService.GetTrainerDetails(id);
            if (trainers is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainerId = trainers.Id;
            ViewBag.TrainerName= trainers.Name;
            return View();
        }
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var result=_trainerService.DeleteTrainer(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Delete Trainer";
            }
            return RedirectToAction(nameof(Index));

        }
        #endregion
        //=========================================================
        #endregion
    }
}
