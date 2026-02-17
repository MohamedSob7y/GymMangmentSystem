using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Entities
{
    public class Member:GymUser
    {
        public string? Photo {  get; set; }
       //Rename this in Configuration as it call CreatedAt
        //لما member يتحول الى table in Database اغير اسم createdat to JoinDate
    }
}
