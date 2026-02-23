using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.View_Models.Member_View_Model
{
    public class MemberViewModel
    {
        //For GetAllMember service without Validation 
        public int Id {  get; set; }
        //return Id in Getall Service عشان اعرف اعمل العمليات زى Create / Update/ Delete using Id
        public string Name { get; set; } = null!;
        public string Email {  get; set; }=null!;
        public string? Photo { get; set; }
        public string Phone { get; set; } = null!;
        public string Gender {  get; set; } = null!;
        //=====================================================
        //For ViewModel GetAllDetails
        //هحط باقى الData هنا وعشان فى GetallMembers لازم ادخلهم ساعتها اعملهم Allow Null 
        //لما اعملهم Allow Null كدة وانا بعمل GetAllMembers مش شرط املاهم 
        //وبرضو مش شرط املاهم فى GetDetails
        public string? PlanName {  get; set; } 
        public string? DateOfBirth {  get; set; }
        public string? Address {  get; set; }
        public string? MembershipStartDate {  get; set; }
        public string? MembershipEndDate { get; set; }
    }
}
