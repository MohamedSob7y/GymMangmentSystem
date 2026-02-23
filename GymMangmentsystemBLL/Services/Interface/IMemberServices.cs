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
        MemberViewModel? GetMemberDetails(int MemberId);
        HealthRecordViewmodel? GetMemberHealthDetails(int MemberId);
        //عايز HealthRecord لMember معين عشان كدة عملته بالMemberId
        //===============================================================
        //Update ليها 2 Service 
        //واحدة ادوس على زرار عشان يعرضلى الصفحة والتانيه عشام يعمل Update in Database 
        //First Service=> GetDetails Before Update
        //Second Service=>Submit Update
        MemberToUpdateViewModel? GetMemberDetailsToUpdate(int MemberId);//عشان تجيب الMember اللى هعمله Update
        bool UpdateMember(int MemberId,MemberToUpdateViewModel memberToUpdateViewModel);//تاخد شكل الداتا اللى هروح اغيرها فى الداتاDatabase 

        //===============================================================
    }
}
