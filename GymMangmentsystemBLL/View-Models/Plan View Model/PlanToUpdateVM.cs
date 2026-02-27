using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.View_Models.Plan_View_Model
{
    public class PlanToUpdateVM
    {
        public string PlanName { get; set; } = null!;//For Static Canot Modify this
        [Required(ErrorMessage = "Description is Required")]
        [StringLength(50,MinimumLength =5,ErrorMessage =("Must Between 50 and 5"))]
        public string Description {  get; set; } = null!;
        [Required(ErrorMessage ="DurationDay is Required")]
        [Range(1,365,ErrorMessage ="DurationDay between 1 and 365")]
        public int DurationDays {  get; set; }
        [Required(ErrorMessage = "Price is Required")]
        [Range(250, 10000, ErrorMessage = "Price between 250 and 10000 EGP")]
        public decimal Price {  get; set; }
    }
}
