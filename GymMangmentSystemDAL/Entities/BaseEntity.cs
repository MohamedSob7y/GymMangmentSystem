using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Entities
{
    //this Class For Admin فى معلومات لازم يعرفها الادمن 
    //this Class مش هيتحول الى Table in Database احنا عملنا عشان نورث منه فقط 
    //Make this Class Abstract عشان مش هكون منه object يعنى مش هعمل منه Constructor لانه مش هيتحول وطالما مش هيتحول الى table in Database make this Class abstract لانى مش هعرف اعمله Object from This Class + Contrcutor 
    public abstract class BaseEntity
    {
        public int Id   { get; set; }
        public DateTime CreatedAt {  get; set; }//امتى الRow دا اتعمل فى database 
        public DateTime? UpdatedAt {  get; set; }//امتى اتعمله Update
        //Allow Null لأانه لسة داخل عندى السيستم عشان مش هيكون عنده UpdateTime فى الاول بس 
    }
}
