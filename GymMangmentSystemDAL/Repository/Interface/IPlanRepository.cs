using GymMangmentSystemDAL.Entities;
using GymMangmentSystemDAL.Repository.Generic_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Repository.Interface
{
    #region Before Generic Repository
    public interface IPlanRepository
    {
        int Update(Plan plan);
        IEnumerable<Plan> GetAllPlan();
        Plan GetById(int id);
    }
    #endregion
    //=======================================
    #region After Generic Repository
    //public interface IPlanRepository:IGenericRepository<Plan>
    //{
    //    //it has Getall+ Getbyid+ Update+Remove + Add
    //    bool IsActivate(int id);
    //}
    #endregion
    //=======================================
}
