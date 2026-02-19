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

        //=====================================================================
        #region Relation
        //Member with HealthRecord 1-1 Total From Two Side بطريقتين لو Owned Or Stack Overflow
        public HealthRecord HealthRecord { get; set; }
        //=====================================================================
        //Member With Plan M-M
        public ICollection<MemberShip>? Memberships { get; set; }
        //=====================================================================
        //Member with session M-M
        public ICollection<MemberSession>? MemberSessions { get; set; }
        #endregion
    }
}
