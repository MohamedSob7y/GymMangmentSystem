using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.View_Models.Trainer_View_Model
{
    public class TrainerViewModel
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public int Id {  get; set; }
        public string Specialities {  get; set; } = null!;
        public string? Gender {  get; set; }
        public string? DateOfBirth { get; set; }
        public string? Address {  get; set; }
    }
}
