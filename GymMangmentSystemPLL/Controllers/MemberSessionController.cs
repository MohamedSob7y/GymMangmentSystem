using GymMangmentsystemBLL.Services.Interface;
using GymMangmentsystemBLL.View_Models.MemberSession_Viewmodel;
using Microsoft.AspNetCore.Mvc;

namespace GymMangmentSystemPL.Controllers
{
    public class MemberSessionController : Controller
    {
        private readonly IMemberSession _memberSession;

        public MemberSessionController(IMemberSession memberSession)
        {
            _memberSession = memberSession;
        }

        // ================= Index =================
        public IActionResult Index()
        {
            var bookings = _memberSession.GetAll();
            return View(bookings);
        }

        // ================= Create (GET) =================
        public IActionResult Create()
        {
            return View();
        }

        // ================= Create (POST) =================
        [HttpPost]
        public IActionResult Create(CreateMemberSession vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            bool result = _memberSession.Create(vm);

            if (result)
            {

                return View(vm);
            }

            return RedirectToAction("Index");
        }

        // ================= Cancel =================
        public IActionResult Cancel(int bookingId)
        {
            bool result = _memberSession.Delete(bookingId);

            if (result)
            {
                TempData["Error"] = result;
            }

            return RedirectToAction("Index");
        }

    }
}
