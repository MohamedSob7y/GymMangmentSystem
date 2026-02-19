using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Entities
{
    [PrimaryKey(nameof(MemberId),nameof(PlanId))]//are Composite PK
    public class MemberShip:BaseEntity
    {
        public int MemberId {  get; set; }
        public int PlanId {  get; set; }
       //وقت اللى اتعمل فيه Membership as table in Database == CreatedAt   => Must Be Rename in Configurations
        public DateTime EndDate {  get; set; }
        public Member Member { get; set; }
        //make Computed Property مش هتتحول فى database  calculated based on Start Date+ End Date اللى اى حاجة هى اصلا 
        //Must Be Ignored in Configuration
        public string Status
        {

            get
            {
                if(EndDate>=DateTime.Now)//انا عديت خلاص الوقت المطلوب كدة الوقت فات بقيت Expired
                {
                    return "Expired";
                }else
                {
                    return "Active";
                }
            }
        }
        
        public Plan Plan { get; set; }
    // Ignore Id in Configuration لانه لما وريثه هيعتبره هو Pk وانا اصلا عايز الاتنين دول MemberId+PlanId are Composite PK
    }
}
