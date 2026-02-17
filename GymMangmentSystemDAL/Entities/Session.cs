using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Entities
{
    public class Session:BaseEntity
    {
        public string Description {  get; set; }
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }//دا وقت بدايت المحاضرة مش وقت تحويلها الى table in Database 
        public DateTime EndDate { get; set; }
    }
}
