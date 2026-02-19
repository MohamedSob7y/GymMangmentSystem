using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Entities
{
    public class Category:BaseEntity
    {
        public string CategoryName {  get; set; }
        //========================================================

        #region Relathion
        //Relathion with Category+Session 1 Partial-M Total
        public ICollection<Session>? Sessions { get; set; }
        //========================================================

        #endregion
    }
}
