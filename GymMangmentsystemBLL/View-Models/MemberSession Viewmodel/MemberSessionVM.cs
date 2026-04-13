using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.View_Models.MemberSession_Viewmodel
{
    public class MemberSessionVM
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int SessionId { get; set; }
        public bool IsAttended { get; set; }
    }
}
