using GymMangmentSystemDAL.Data.Context;
using GymMangmentSystemDAL.Entities;
using GymMangmentSystemDAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Repository.Implementation
{
    public class MemberRepository : IMemberRepository
    {
        #region Problem With Open Connection with DbContext
        private readonly GymDbContext _gymDbContext = new GymDbContext(); 
        //طالما عملت Object To Open Connection with Database مش هعمل اى تعديل عليها عشان كدة هعملها Private / Readonly
        //هنا عملته Manual واا مش عاريز كدة عشان عملى مشكلة Dependency injection
        #endregion
        //==========================================================
        
        public int Add(Member member)
        {
             _gymDbContext.Members.Add(member);
            return _gymDbContext.SaveChanges();
        }

        public int Delete(int MemberId)
        {
            var obj = _gymDbContext.Members.Find(MemberId);
            if (obj is null) return 0;
            _gymDbContext.Members.Remove(obj);
            return _gymDbContext.SaveChanges(); 
        }

        public IEnumerable<Member> GetAllMembers()=> _gymDbContext.Members.ToList();

        public Member? GetById(int MemberId) => _gymDbContext.Members.Find(MemberId);
        //Find عشان Datalocal عندى 


        public int Update(Member member)
        {
            _gymDbContext.Members.Remove(member);
            return _gymDbContext.SaveChanges();
        }
    }
}
