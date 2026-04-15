using GymMangmentsystemBLL.View_Models.Member_View_Model;
using GymMangmentsystemBLL.View_Models.Session_View_Model;
using GymMangmentSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.View_Models.MemberSession_Viewmodel
{
    public class CreateMemberSession
    {
        public int MemberId { get; set; }
        public int SessionId { get; set; }

        // للـ dropdowns
        public List<MemberViewModel> Members { get; set; } = new();
        public List<SessionViewModel> Sessions { get; set; } = new();
    }
}
