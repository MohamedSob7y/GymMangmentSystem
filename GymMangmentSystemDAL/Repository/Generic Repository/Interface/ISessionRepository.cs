using GymMangmentSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Repository.Generic_Repository.Interface
{
    public interface ISessionRepository:IGenericRepository<Session>
    {
        IEnumerable<Session> GetAllSessionWithCategoryAndTrainer();
        int GetCountOfBookingSlots(int SessionId);
        Session? GetByIdWithCategoryAndTrainer(int SessionId);
    }
}
