using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.View_Models.Member_View_Model
{
    public class HealthRecordViewmodel
    {
        [Required(ErrorMessage ="Height is Required")]
        [Range(0.1,300,ErrorMessage ="Height Must between 0.1 and 300CM")]
        public decimal Height { get; set; }
        [Range(1, 350, ErrorMessage = "Weight Must between 1 and 350KG")]
        [Required(ErrorMessage = "Weight is Required")]
        public decimal Weight { get; set; }
        public string? Note { get; set; }
        [Required(ErrorMessage = "BloodType is Required")]
        [StringLength(3,ErrorMessage ="BloodType is Max 3")]
        public string BloodType { get; set; } = null!;
    }
}
