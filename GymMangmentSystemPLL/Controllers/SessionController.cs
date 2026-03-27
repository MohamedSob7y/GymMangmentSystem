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
        public ActionResult Index()
        {
            var sessions=_sessionService.GetAllsessions();
             return View(sessions);//Send Data To View Index
        }
        #endregion
    }
}
