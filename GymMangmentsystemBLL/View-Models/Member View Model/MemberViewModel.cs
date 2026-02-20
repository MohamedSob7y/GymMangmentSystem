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

    }
}
