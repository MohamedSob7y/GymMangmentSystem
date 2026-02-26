using GymMangmentsystemBLL.Services.Interface;
using GymMangmentsystemBLL.View_Models.Plan_View_Model;
using GymMangmentSystemDAL.Entities;
using GymMangmentSystemDAL.Unit_Of_Work.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.Services.Implementation
{
    public class PlanService : IPlanService
    {
        private readonly IUniteOfWork _uniteOfWork;

        public PlanService(IUniteOfWork uniteOfWork)
        {
            _uniteOfWork = uniteOfWork;
        }

        public IEnumerable<PlanVM> GetAllPlans()
        {
            var plans = _uniteOfWork.GetRepository<Plan>()
                 .GetAll();
            if (plans is null)
                // return Enumerable.Empty<PlanVM>();
                return [];
            //================================================
            #region Manual Mapping
            //================================================
            #region First Mapping 
            //var emptylist = new List<PlanVM>();
            //foreach (var plan in plans)
            //{
            //    var planviewmodel = new PlanVM()
            //    {
            //        Name = plan.Name,
            //        Description = plan.Description,
            //        DurationDays = plan.DurationDays,
            //        Id = plan.Id,
            //        Price = plan.Price,
            //    };
            //    emptylist.Add(planviewmodel);
            //}
            //return emptylist; 
            #endregion
            //================================================
            #region Second Mapping
            var planviewmodel = plans.Select(T => new PlanVM()
            {
                Name = T.Name,
                Description = T.Description,
                DurationDays = T.DurationDays,
                Id = T.Id,
                Price = T.Price,
            });
            return planviewmodel;
            #endregion 
            //================================================
            #endregion
        }

        public PlanVM? GetPlanDetails(int PlanId)
        {
            var plan=_uniteOfWork.GetRepository<Plan>()
                .GetById(PlanId);
            if (plan is null) return null;
            return new PlanVM()
            {
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                Id= plan.Id,//هو بيعمل Hide ليه عادى 
            };

        }

        public PlanToUpdateVM? GetPlanToUpdate(int PlanId)
        {

        }

        public bool ToggleStatus(int PlanId)
        {
           
        }

        public bool UpdatePlan(int PlanId, PlanToUpdateVM planVM)
        {

        }
    }
}
