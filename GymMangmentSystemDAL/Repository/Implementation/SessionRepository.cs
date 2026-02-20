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
    public class SessionRepository : ISessionRepository
    {

        #region Problem With Open Connection with DbContext
        //private readonly GymDbContext _gymDbContext = new GymDbContext();

        //طالما عملت Object To Open Connection with Database مش هعمل اى تعديل عليها عشان كدة هعملها Private / Readonly
        //هنا عملته Manual واا مش عاريز كدة عشان عملى مشكلة Dependency injection
        #endregion
        //===============================================
        #region Solving Problem With Dependency injection
        private readonly GymDbContext _gymDbContext;
        public SessionRepository(GymDbContext gymDbContext)
        {
            _gymDbContext = gymDbContext;
        }
        #endregion
        //===================================================
        public int Add(Session session)
        {
           _gymDbContext.Add(session);
           return _gymDbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var session = _gymDbContext.Sessions.Find(id);
            if (session != null) return 0;
            _gymDbContext.Sessions.Remove(session);
            return _gymDbContext.SaveChanges();
        }

        public IEnumerable<Session> GetAllSession() => _gymDbContext.Sessions.ToList();
        public Session GetById(int id) => _gymDbContext.Sessions.Find(id);

        public int Update(Session session)
        {
            _gymDbContext.Sessions.Remove(session);
            return _gymDbContext.SaveChanges();
        }
    }
}
