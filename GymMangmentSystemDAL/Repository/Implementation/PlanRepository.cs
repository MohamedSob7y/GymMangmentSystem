using GymMangmentSystemDAL.Data.Context;
using GymMangmentSystemDAL.Entities;
using GymMangmentSystemDAL.Repository.Generic_Repository.Implementation;
using GymMangmentSystemDAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Repository.Implementation
{
    #region Before Generic Repository
    public class PlanRepository : IPlanRepository
    {
        #region Problem With Open Connection with DbContext
        //private readonly GymDbContext _gymDbContext = new GymDbContext();
        //طالما عملت Object To Open Connection with Database مش هعمل اى تعديل عليها عشان كدة هعملها Private / Readonly
        //هنا عملته Manual واا مش عاريز كدة عشان عملى مشكلة Dependency injection
        #endregion
        //===============================================
        #region Solving Problem With Dependency injection
        private readonly GymDbContext _gymDbContext;
        public PlanRepository(GymDbContext gymDbContext) : base()
        {
            _gymDbContext = gymDbContext;
        }
        #endregion
        //===================================================
        public IEnumerable<Plan> GetAllPlan() => _gymDbContext.Plans.ToList();
        public Plan GetById(int id) => _gymDbContext.Plans.Find(id);

        public int Update(Plan plan)
        {
            _gymDbContext.Plans.Update(plan);
            return _gymDbContext.SaveChanges();
        }
    }
    #endregion
    //=======================================
    #region After Generic Repository
    //public class PlanRepository : 
    //    GenericRepository<Plan>, IPlanRepository
    //{
    //    private readonly GymDbContext _gymDbContext;

    //    public PlanRepository(GymDbContext gymDbContext)
    //    {
    //        _gymDbContext = gymDbContext;
    //    }
    //    public bool IsActivate(int id)
    //    {
    //        _gymDbContext.Set<Plan>().Find(id);
    //        return true;
    //    }
    //}


    #endregion
    //=======================================
}
