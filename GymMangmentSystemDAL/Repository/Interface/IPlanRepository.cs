using GymMangmentSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Repository.Interface
{
    public interface IPlanRepository
    {
        int Add(Plan plan);
        int Update(Plan plan);
        int Delete(int id);
        IEnumerable<Plan> GetAllPlan();
        Plan GetById(int id);
    }
}
