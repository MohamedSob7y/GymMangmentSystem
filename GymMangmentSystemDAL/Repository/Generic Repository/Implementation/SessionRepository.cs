using GymMangmentSystemDAL.Data.Context;
using GymMangmentSystemDAL.Entities;
using GymMangmentSystemDAL.Repository.Generic_Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Repository.Generic_Repository.Implementation
{
    public class SessionRepository:GenericRepository<Session>,ISessionRepository
    {
        private readonly GymDbContext _gymDbContext;

        public SessionRepository(GymDbContext gymDbContext)
            :base(gymDbContext) 
        {
            _gymDbContext = gymDbContext;
        }

        public IEnumerable<Session> GetAllSessionWithCategoryAndTrainer()
        {
           return _gymDbContext.Sessions.Include(T=>T.Category)
                .Include(T=>T.Trainer).AsNoTracking().ToList();
        }

        public Session? GetByIdWithCategoryAndTrainer(int SessionId)
        {
            return _gymDbContext.Sessions.Include(T=>T.Category)
                .Include(T=>T.Trainer)
                .FirstOrDefault(T=>T.Id==SessionId);
            //مش هينفع استخدم Find عشان Data Not local لان معايا Data From Category+ Trainers لو هى Session فقط اعرف استخدم عادى Find
        }

        public int GetCountOfBookingSlots(int SessionId)
        {
            //هعايز اعرف عدد Members اللى عاملين Booking For This Session
            return _gymDbContext.MemberSessions.Count(T => T.SessionId == SessionId);
        }
    }
}
