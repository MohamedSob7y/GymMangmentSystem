using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Entities
{
    public class HealthRecord:BaseEntity
    {
        public decimal Height {  get; set; }
        public decimal Weight { get; set; }  
        public string BloodType {  get; set; }
        public string? Note { get; set; }//Allow Null عشان ممكن الدتور ميكنشى عنده اى ملاحظات على المريض
        //Rename LastUpdated At لما احول  دا الى table in Database => Updatedat
    }
}
