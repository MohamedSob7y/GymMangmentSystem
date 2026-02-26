using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.View_Models.Plan_View_Model
{
    public class PlanVM
    {
        public int Id { get; set; }
        //رجعت الId عشان اعرف استخدمها فى Create+Update+Delete
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price {  get; set; }
        public bool IsActive { get; set; }
        public int DurationDays {  get; set; }
    }
}
