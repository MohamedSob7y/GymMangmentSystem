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
        public IMapper _Mapper { get; }
        public MemberSessionService(IUniteOfWork uniteOfWork,IMapper mapper)
        {
            _uniteOfWork = uniteOfWork;
            _Mapper = mapper;
        }



        public bool Create(CreateMemberSession createMemberSession)
        {
            //           🎯 Create Booking لازم يتحقق من:
            //            Member موجود
            //Session موجودة
            //عنده Membership Active
            //السيشن Future(لسه ما بدأتش)
            //مفيش Duplicate Booking
            //Capacity متاحة
            //IsAttended = false
            try
            {
                var member = _uniteOfWork.GetRepository<Member>()
                       .GetById(createMemberSession.MemberId);
                var session = _uniteOfWork.GetRepository<Session>()
                    .GetById(createMemberSession.SessionId);
                if (session is null || member is null)
                    return false;
                var HasActiveMembership = _uniteOfWork.GetRepository<MemberShip>()
                    .GetAll(m => m.MemberId == createMemberSession.MemberId && m.EndDate > DateTime.Now).Any();
                if (HasActiveMembership)
                    return false;
                //Future Session
                if (session.StartDate > DateTime.Now) return false;

                // Rule 3: Duplicate booking
                var alreadyBooked = _uniteOfWork.GetRepository<MemberSession>()
                    .GetAll(b => b.MemberId == createMemberSession.MemberId && b.SessionId == createMemberSession.SessionId)
                    .Any();

                if (alreadyBooked)
                    return false;

                // Rule 2: Capacity
                var currentCount = _uniteOfWork.GetRepository<MemberSession>()
                    .GetAll(b => b.SessionId == createMemberSession.SessionId)
                    .Count();

                if (currentCount >= session.Capacity)
                    return false;

                // Create
                var booking = _Mapper.Map<MemberSession>(createMemberSession);
                booking.IsAttend = false;

                _uniteOfWork.GetRepository<MemberSession>().Add(booking);
                return _uniteOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Delete(int BookingId)
        {
            try
            {
                var booking = _uniteOfWork.GetRepository<MemberSession>().GetById(BookingId);
                if (booking is null)
                    return false;

                var session = _uniteOfWork.GetRepository<Session>().GetById(booking.SessionId);
                if (session is null)
                    return false;

                // Rule 5
                if (session.StartDate <= DateTime.Now)
                    return false;

                _uniteOfWork.GetRepository<MemberSession>().Delete(booking);
                return _uniteOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }

          
        }

        public IEnumerable<MemberSessionVM> GetAll()
        {
            var membersession = _uniteOfWork.GetRepository<MemberSession>()
                .GetAll();
            if (membersession is null||!membersession.Any())
                return [];
            return _Mapper.Map<IEnumerable<MemberSession>,IEnumerable<MemberSessionVM>>(membersession); 
        }

        public MemberSessionVM? GetById(int id)
        {
            var membersession = _uniteOfWork.GetRepository<MemberSession>()
                .GetById(id);
            if (membersession is null)
                return null;
            return _Mapper.Map<MemberSession,MemberSessionVM>(membersession);

        }
        // ================= Attendance =================
        public string MarkAttendance(int bookingId)
        {
            var booking = _uniteOfWork.GetRepository<MemberSession>().GetById(bookingId);
            if (booking is null)
                return "Booking not found";

            var session = _uniteOfWork.GetRepository<Session>().GetById(booking.SessionId);
            if (session is null)
                return "Session not found";

            var now = DateTime.Now;

            // Rule 6
            if (!(session.StartDate <= now && session.EndDate > now))
                return "Session is not ongoing";

            booking.IsAttend = true;

            _uniteOfWork.GetRepository<MemberSession>().Update(booking);
            _uniteOfWork.SaveChanges();

            return "Attendance marked";
        }
    }
}
