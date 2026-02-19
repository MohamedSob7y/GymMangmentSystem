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
    public class PlanRepository : IPlanRepository
    {
        #region Problem With Open Connection with DbContext
        private readonly GymDbContext _gymDbContext = new GymDbContext();
        //طالما عملت Object To Open Connection with Database مش هعمل اى تعديل عليها عشان كدة هعملها Private / Readonly
        //هنا عملته Manual واا مش عاريز كدة عشان عملى مشكلة Dependency injection
        #endregion
        public int Add(Plan plan)
        {
           _gymDbContext.Plans.Add(plan);
            return _gymDbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var plan = _gymDbContext.Plans.Find(id);
            if (plan is null) return 0;
            _gymDbContext.Plans.Remove(plan);
            return _gymDbContext.SaveChanges();
        }

        public IEnumerable<Plan> GetAllPlan()=> _gymDbContext.Plans.ToList();
        public Plan GetById(int id) => _gymDbContext.Plans.Find(id);

        public int Update(Plan plan)
        {
           _gymDbContext.Plans.Update(plan);
            return _gymDbContext.SaveChanges();
        }
    }
}
