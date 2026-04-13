using AutoMapper;
using GymMangmentsystemBLL.Services.Interface;
using GymMangmentsystemBLL.View_Models.MemberSession_Viewmodel;
using GymMangmentSystemDAL.Entities;
using GymMangmentSystemDAL.Unit_Of_Work.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.Services.Implementation
{
    public class MemberSessionService : IMemberSession
    {
        private readonly IUniteOfWork _uniteOfWork;
        private readonly IMapper _mapper;

        public MemberSessionService(IUniteOfWork uniteOfWork, IMapper mapper)
        {
            _uniteOfWork = uniteOfWork;
            _mapper = mapper;
        }

        // ================= Get All =================
        public IEnumerable<MemberSessionVM> GetAll()
        {
            var bookings = _uniteOfWork.GetRepository<MemberSession>().GetAll();

            if (bookings == null || !bookings.Any())
                return new List<MemberSessionVM>();

            return _mapper.Map<IEnumerable<MemberSessionVM>>(bookings);
        }

        // ================= Create =================
        public bool Create(CreateMemberSession vm)
        {
            var member = _uniteOfWork.GetRepository<Member>().GetById(vm.MemberId);
            if (member is null) return false;

            var session = _uniteOfWork.GetRepository<Session>().GetById(vm.SessionId);
            if (session is null) return false;

            // Active Membership
            var hasMembership = _uniteOfWork.GetRepository<MemberShip>()
                .GetAll(m => m.MemberId == vm.MemberId && m.EndDate > DateTime.Now)
                .Any();

            if (!hasMembership) return false;

            // Future Session
            if (session.StartDate <= DateTime.Now)
                return false;

            // Duplicate
            var exists = _uniteOfWork.GetRepository<MemberSession>()
                .GetAll(b => b.MemberId == vm.MemberId && b.SessionId == vm.SessionId)
                .Any();

            if (exists) return false;

            // Capacity
            var count = _uniteOfWork.GetRepository<MemberSession>()
                .GetAll(b => b.SessionId == vm.SessionId)
                .Count();

            if (count >= session.Capacity)
                return false;

            var booking = _mapper.Map<MemberSession>(vm);
            booking.IsAttend = false;

            _uniteOfWork.GetRepository<MemberSession>().Add(booking);
            _uniteOfWork.SaveChanges();

            return true;
        }

        // ================= Delete =================
        public bool Delete(int bookingId)
        {
            var booking = _uniteOfWork.GetRepository<MemberSession>().GetById(bookingId);
            if (booking is null) return false;

            var session = _uniteOfWork.GetRepository<Session>().GetById(booking.SessionId);

            if (session.StartDate <= DateTime.Now)
                return false;

            _uniteOfWork.GetRepository<MemberSession>().Delete(booking);
            _uniteOfWork.SaveChanges();

            return true;
        }

        // ================= Attendance =================
        public void MarkAttendance(int bookingId)
        {
            var booking = _uniteOfWork.GetRepository<MemberSession>().GetById(bookingId);
            if (booking is null) return;

            var session = _uniteOfWork.GetRepository<Session>().GetById(booking.SessionId);

            var now = DateTime.Now;

            if (!(session.StartDate <= now && session.EndDate > now))
                return;

            booking.IsAttend = true;

            _uniteOfWork.GetRepository<MemberSession>().Update(booking);
            _uniteOfWork.SaveChanges();
        }

        // ================= Upcoming =================
        public IEnumerable<MemberSessionVM> GetMembersForUpcomingSession(int sessionId)
        {
            return _uniteOfWork.GetRepository<MemberSession>()
                .GetAll(b => b.SessionId == sessionId && b.Session.StartDate > DateTime.Now)
                .Select(b => new MemberSessionVM
                {
                    Id = b.Id,
                    MemberName = b.Member.Name,
                   
                });
        }

        // ================= Ongoing =================
        public IEnumerable<MemberSessionVM> GetMembersForOngoingSession(int sessionId)
        {
            var now = DateTime.Now;

            return _uniteOfWork.GetRepository<MemberSession>()
                .GetAll(b => b.SessionId == sessionId &&
                             b.Session.StartDate <= now &&
                             b.Session.EndDate > now)
                .Select(b => new MemberSessionVM
                {
                    Id = b.Id,
                    MemberName = b.Member.Name,
                    IsAttended = b.IsAttend
                });
        }
    }
}
