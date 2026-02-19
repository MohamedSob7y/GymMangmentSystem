using GymMangmentSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Repository.Interface
{
    public interface ISessionRepository
    {
        int Add(Session session);
        int Update(Session session);
        int Delete(int id);
        IEnumerable<Session> GetAllSession();
        Session GetById(int id);
    }
}
