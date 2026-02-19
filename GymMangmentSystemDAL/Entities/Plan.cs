using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Entities
{
    public class Plan:BaseEntity
    {
        public string Name {  get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int DurationDays { get; set; }
        public decimal Price {  get; set; }
        //============================================================
        #region Relations
        //Relation with Plan - Member M-M
        public ICollection<MemberShip>? Memberships { get; set; }

        #endregion

    }
}
