using GymMangmentsystemBLL.View_Models.MemberSession_Viewmodel;
using GymMangmentSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.Services.Interface
{
    public interface IMemberSession
    {
        IEnumerable<MemberSessionVM> GetAll();

        bool Create(CreateMemberSession vm);

        bool Delete(int bookingId);

        IEnumerable<MemberSessionVM> GetMembersForUpcomingSession(int sessionId);

        IEnumerable<MemberSessionVM> GetMembersForOngoingSession(int sessionId);

        void MarkAttendance(int bookingId);

    }
}
