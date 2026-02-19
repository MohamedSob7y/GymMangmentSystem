using GymMangmentSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Repository.Interface
{
    public interface ITrainerRepository
    {
        IEnumerable<Trainer> GetAllTrainers();
        Trainer? GetbyId(int id);
        int Add(Trainer trainer);
        int Update(Trainer trainer);    
        int Delete(int id);
    }
}
