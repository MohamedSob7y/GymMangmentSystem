using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymMangmentSystemDAL;
using GymMangmentSystemDAL.Entities.Enums;
namespace GymMangmentSystemDAL.Entities
{
    //this Class مش هيتحول الى table in Database احنا عملناه عشان نورث منه 
    //So make this Abtract عشان مش هيتحول  + مش هكون منه object
    public abstract class GymUser:BaseEntity
    {
        public string Name {  get; set; }
        public string Email { get; set; }
        public string Phone {  get; set; }
        public DateOnly DateofBirth {  get; set; }//Dateonly مش محتاج اى ساعة ولا دقائق ولا ثوانى 
        //DateTime محتاج تاريخ ودقايق وثوانى 
        public Gender Gender {  get; set; }
        //===============================================
        //Address 
        public Address Address { get; set; }
    }
}
