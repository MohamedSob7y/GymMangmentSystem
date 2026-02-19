using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Entities
{
    [PrimaryKey(nameof(MemberId),nameof(SessionId))]
    public class MemberSession:BaseEntity
    {
        public Member Member { get; set; }
        public Session Session { get; set; }
        public int MemberId {  get; set; }
        public int SessionId {  get; set; }
        public bool IsAttend {  get; set; }
    }
}
