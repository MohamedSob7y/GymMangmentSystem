using GymMangmentsystemBLL.View_Models.MembershipViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.Services.Interface
{
    public interface IMembershipService
    {
        IEnumerable<MembershipVM> GetAll();
        MembershipVM? GetById(int id);
        bool Create(CreateMembershipVM createMembership);
        bool Cancel(int memberId, int planId);
    }
}
