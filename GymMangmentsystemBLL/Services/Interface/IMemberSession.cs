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
        MemberSessionVM? GetById(int id);
        bool Create(CreateMemberSession createMemberSession);
        bool Delete(int BookingId);
        public string MarkAttendance(int bookingId);

    }
}
