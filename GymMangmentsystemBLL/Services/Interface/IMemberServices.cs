using GymMangmentsystemBLL.View_Models.Member_View_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.Services.Interface
{
    public interface IMemberServices
    {
        //GetAllMembers
        IEnumerable<MemberViewModel> GetallMembers();
        //Create New Member Must make Validations in View model
        bool Create(CreateMember Member);//Convert Create Member to Member to add it in Database 
    }
}
