using GymMangmentsystemBLL.Services.Implementation;
using GymMangmentsystemBLL.Services.Interface;
using GymMangmentsystemBLL.View_Models.MemberSession_Viewmodel;
using Microsoft.AspNetCore.Mvc;

namespace GymMangmentSystemPL.Controllers
{
    public class MemberSessionController : Controller
    {
        private readonly IMemberSession _memberSession;
        private readonly IMemberServices _memberService;
        private readonly ISessionService _sessionService;

        public MemberSessionController(
            IMemberSession memberSession,
            IMemberServices memberService,
            ISessionService sessionService)
        {
            _memberSession = memberSession;
            _memberService = memberService;
            _sessionService = sessionService;
        }

        // ================= Index =================
        public IActionResult Index()
        {
            var sessions = _sessionService.GetAllsessions(); // Upcoming + Ongoing
            return View(sessions);
        }

        // ================= Create (GET) =================
        public IActionResult Create()
        {
            var vm = new CreateMemberSession
            {
                Members = _memberService.GetallMembers().ToList(),
                Sessions = _sessionService.GetAllsessions().ToList()
            };

            return View(vm);
        }

        // ================= Create (POST) =================
        [HttpPost]
        public IActionResult Create(CreateMemberSession vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Members = _memberService.GetallMembers().ToList();
                vm.Sessions = _sessionService.GetAllsessions().ToList();
                return View(vm);
            }

            bool result = _memberSession.Create(vm);

            if (!result)
            {
                ModelState.AddModelError("", "Booking failed");

                vm.Members = _memberService.GetallMembers().ToList();
                vm.Sessions = _sessionService.GetAllsessions().ToList();

                return View(vm);
            }

            return RedirectToAction("Index");
        }

        // ================= Cancel =================
        public IActionResult Cancel(int bookingId)
        {
            _memberSession.Delete(bookingId);
            return RedirectToAction("Index");
        }

        // ================= Upcoming =================
        public IActionResult GetMembersForUpcomingSession(int sessionId)
        {
            var members = _memberSession.GetMembersForUpcomingSession(sessionId);
            return View(members);
        }

        // ================= Ongoing =================
        public IActionResult GetMembersForOngoingSessions(int sessionId)
        {
            var members = _memberSession.GetMembersForOngoingSession(sessionId);
            return View(members);
        }

        // ================= Attendance =================
        [HttpPost]
        public IActionResult Attend(int bookingId)
        {
            _memberSession.MarkAttendance(bookingId);
            return RedirectToAction("GetMembersForOngoingSessions");
        }
    }
}
