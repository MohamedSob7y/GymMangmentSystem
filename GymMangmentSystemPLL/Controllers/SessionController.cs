using GymManagementSystemBLL.View_Models.SessionVm;
using GymMangmentsystemBLL.Services.Implementation;
using GymMangmentsystemBLL.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymMangmentSystemPL.Controllers
{
    public class SessionController : Controller
    {
        #region Inject object from ISessionService
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        #endregion
        //==============================================
        #region All Actions
        //==============================================
        #region Get All Action
        public ActionResult Index()
        {
            var sessions = _sessionService.GetAllsessions();
            return View(sessions);//Send Data To View Index
        }
        #endregion
        //==============================================
        #region Get Details action
        public ActionResult Details([FromRoute]int id)
        {
            if(id<=0)
            {
                TempData["ErrorMessage"] = "Id Cannot be negative or zero";
                return  RedirectToAction(nameof(Index));
            }
            var session=_sessionService.GetSessionDetails(id);
            if(session is  null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }

        #endregion
        //==============================================
        #region Create Action
        public ActionResult Create()
        {
            #region Before Helper Method DropDown
            //هنا فى Action دى محتاج ابعت للForm Id + Name Of Category+ Trainer عشان يختار على طول والاختيار اللى هو اختراه يبقى مبعوت للaction التانيه 
            //var categoriyes = _sessionService.GetCategoryFromDropDown();
            //var trainers = _sessionService.GetTrainerForDropDown();
            //===========================================================
            #region Problem
            //ViewBag.Trainers = trainers;
            //ViewBag.Categories = categoriyes;
            //Convert Items To Select List بدل ماهو اللى يعمله 
            //الداتا دى كدة بتتعب من نوع  Ienumerable Not Select List وهو عايزها من نوع Select List عشان كدة لازم احولها بدل ماتتبعت Ienumerable

            #endregion
            //===========================================================
            #region Solving Problem By Converting this To SelectList
            //ViewBag.Trainers = new SelectList(trainers, "Id", "Name");//Id دى اللى هتتبعت
            //ViewBag.Categories = new SelectList(categoriyes, "Id", "Name");
            //كدة انا نقلت داتا لنفس الRequest
            #endregion
            #endregion
            //===========================================================
            #region After Helper Method DropDown
            LoadDropDowns();
            #endregion
            //===========================================================
            return View(); 
        }
        [HttpPost]
        public ActionResult Create(CreateSessionViewModel createSessionViewModel)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInValid","Missing Field");
                LoadDropDowns();//دى حل مشكلة انى لما برجع تانى للصفحة بتاعت create لازم يكون Loading دااتا موجود  معايا
                return View(nameof(Create),createSessionViewModel);//هنا لما بروح View Create مش هتبقى الداتا Loaded عندى عشان كدا فى مشكلة لازم احلها انى لو حصل اى غلط فى Model محتاج ارجع للView بتاعت create ومعاها Loading Data Category+ Trainer دا اللى محتاجه
            }
            var Result=_sessionService.CreateSession(createSessionViewModel);
            if(Result)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Session Failed To Created ";
                LoadDropDowns();
                return View(nameof(Create), createSessionViewModel);
            }
           
        }
        #endregion
        //==============================================
        #region Update Action
        public ActionResult Edit([FromRoute]int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot be negative or Zero";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetsessionToUpdate(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            //================================================================
            //Load Data For Trainers  And Send This To View 
            #region Before Helper Method
            //var trainer = _sessionService.GetTrainerForDropDown();
            //ViewBag.Trainers = new SelectList(trainer, "id", "Name"); 
            #endregion
            //================================================================
            #region After Helper Method
            LoadDropDownForTrainerOnly();
            #endregion
            //================================================================
            return View(session);
        }
        [HttpPost]
        //Take Id From Route and Other Data From Form
        public ActionResult Edit([FromRoute]int id, UpdateSessionViewModel updateSessionViewModel)
        {
            if (!ModelState.IsValid)
            {
                //محتاج لما ارجع لنفس  الصفحة لو مثلا Skip Client Application Validation => دا هيعمل model Satat Error بالتالى فى مشكلة وانا بقا عايزه لما يرجعنى لنفس الصفحة تكون Data For Trainer Loaded ونفس الحوار دا فى Error Of Result 
                ModelState.AddModelError("DataInValid", "Missing Field");
                //================================================================
                #region Before Helper Method
                //var trainer = _sessionService.GetTrainerForDropDown();
                //ViewBag.Trainers = new SelectList(trainer, "id", "Name"); 
                #endregion
                //================================================================
                #region After Helper Method
                LoadDropDownForTrainerOnly();
                #endregion
                //================================================================
                return View(nameof(Edit), updateSessionViewModel);//هنا لما بروح View Create مش هتبقى الداتا Loaded عندى عشان كدا فى مشكلة لازم احلها انى لو حصل اى غلط فى Model محتاج ارجع للView بتاعت create ومعاها Loading Data Category+ Trainer دا اللى محتاجه
            }
            var Result = _sessionService.UpdateSession(id,updateSessionViewModel);
            if (Result)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Session Failed To Updated ";
                //================================================================
                //محتاج لما ارجع لنفس  الصفحة لو مثلا Skip Client Application Validation => دا هيعمل model Satat Error بالتالى فى مشكلة وانا بقا عايزه لما يرجعنى لنفس الصفحة تكون Data For Trainer Loaded ونفس الحوار دا فى Error Of Result 
                #region Before Helper Method
                //var trainer = _sessionService.GetTrainerForDropDown();
                //ViewBag.Trainers = new SelectList(trainer, "id", "Name"); 
                #endregion
                //================================================================
                #region After Helper Method
                LoadDropDownForTrainerOnly();
                #endregion
                //================================================================
                return View(nameof(Edit), updateSessionViewModel);
            }
        }


        #endregion
        //==============================================
        #region Delete Action
        public ActionResult Delete([FromRoute]int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot be negative or zero";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionDetails(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            //Send Id To action Post Data To Server 
            ViewBag.SessionId = id;
            return View();
        }
        [HttpPost]
        //This Action Will Take Id From Action Delete عشان كدة ببعته لNext Action that will Post Data To Server فاول action will take Id From Route
        //Second Action will Take Id From First Action عن طريق انى بعت الداتا دى لنفس الRequest by ViewBage
        public ActionResult DeleteConfirmed(int id)
        {
            var Result = _sessionService.RemoveSession(id);
            if(Result)
            {
                TempData["SuccessMessage"] = "Session Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Session Failed To Delete ";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
        //==============================================
        #region Helper Method
        //this Helper Method For Load Data For Trainer + Category
        private void LoadDropDowns()
        {
            var categoriyes = _sessionService.GetCategoryFromDropDown();
            var trainers = _sessionService.GetTrainerForDropDown();
            ViewBag.Trainers = new SelectList(trainers, "Id", "Name");//Id دى اللى هتتبعت
            ViewBag.Categories = new SelectList(categoriyes, "Id", "Name");
        }
        private void LoadDropDownForTrainerOnly()
        {
            var trainer = _sessionService.GetTrainerForDropDown();
            ViewBag.Trainers = new SelectList(trainer, "id", "Name");
        }
        #endregion
        //==============================================
        #endregion
    }
}
