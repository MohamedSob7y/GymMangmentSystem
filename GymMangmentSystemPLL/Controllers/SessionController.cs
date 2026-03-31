using GymMangmentsystemBLL.Services.Interface;
using Microsoft.AspNetCore.Mvc;

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





        //==============================================







        //==============================================

        //==============================================
        #endregion
    }
}
