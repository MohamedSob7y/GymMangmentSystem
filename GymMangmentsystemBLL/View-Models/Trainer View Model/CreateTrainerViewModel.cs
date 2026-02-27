using GymMangmentSystemDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.View_Models.Trainer_View_Model
{
    public class CreateTrainerViewModel
    {
        //Make Application Validation
        [Required(ErrorMessage = "Please Enter Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name Must be between 2 and 50")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name Must Constain At Least One characters")]//* Zero or more يعنى ممكن مايبعتشى 
        //+ AtLeast One Or More يعنى لازم يدخل حرف على الاقل 
        //^ Start with 
        //$ End With
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Email is Required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email Must Between 5 and 100")]
        [DataType(DataType.EmailAddress)]//for Ui Hint
        [EmailAddress(ErrorMessage = "InValid Email Format")]//For application Validation For Formate Email
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Gender is Required")]
        public Gender Gender { get; set; }
        [Required(ErrorMessage = "Phone is Required")]
        [Phone(ErrorMessage = "inValid Format Phone")]
        [DataType(DataType.PhoneNumber)]//for ui Hint
        [RegularExpression(@"^(010|011|015|012)\d{8}$")]//عشان اخليه مصرى بس 
       //Allowed Number After 010 هو 8 
       // [StringLength(11,MinimumLength =,ErrorMessage ="Phone must be betwee")]//دى لوعملتها ممكن يبعت اقل من 11 عادى فكدة مش صح عشان كدة اشيلها
        public string Phone { get; set; } = null!;
        [Required(ErrorMessage = "DateOfBirth is Required")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }
        //مش هعمل addressViewmodel لانى مش هستخدمه فى اكتر من مكان
        [Required(ErrorMessage = "Buidling Number is Required")]
        [Range(1, 9000, ErrorMessage = "BuilingNumber must be Between 1 and 9000")]
        public int BuildingNumber { get; set; }
        [Required(ErrorMessage = "City is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "City must between 30 and 2")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City Must Constain At Least One characters")]
        public string City { get; set; } = null!;
        [Required(ErrorMessage = "Street is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "street must between 30 and 2")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "street Must Constain At Least One characters")]
        public string Street { get; set; } = null!;
        [EnumDataType(typeof(Speciality))]
        [Required(ErrorMessage = "Speciality is Required")]
        public Speciality Specialities { get; set; }
    }
}
