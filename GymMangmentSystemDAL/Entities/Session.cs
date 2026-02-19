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
        //========================================================

        #region Relathion
        //Relathion with Category+Session 1 Partial-M Total
        public Category Category { get; set; }
        public int CategoryId {  get; set; }//this Foreighn Key by Convention
        //========================================================
        //Trainer With Session 1 Partial -M Total
        public Trainer Trainer { get; set; }
        public int TrainerId {  get; set; }//This Foreign Key
        //========================================================
        //Member with Session M-M
        public ICollection<MemberSession>? MemberSessions { get; set; }
        #endregion

    }
}
