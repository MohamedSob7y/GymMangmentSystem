using GymMangmentSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Repository.Interface
{
    public interface IMemberRepository
    {
        //This Method Crud Operations
        IEnumerable<Member> GetAllMembers();
        Member? GetById(int MemberId);
        int Add(Member member);
        int Update(Member member);
        int Delete(int MemberId);

    }
}
