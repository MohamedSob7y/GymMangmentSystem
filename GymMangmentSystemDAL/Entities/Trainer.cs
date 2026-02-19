using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymMangmentSystemDAL.Entities.Enums;
namespace GymMangmentSystemDAL.Entities
{
    public class Trainer:GymUser
    {
        public Speciality Speciality {  get; set; }
        //Rename CreatedAt To HireDateلما يتحول الى table in Database 
        //==========================================================================
        #region Relations
        //Trainer With Session 1 Partial -M Total
        public ICollection <Session>? Sessions { get; set; }
        #endregion

    }
}
