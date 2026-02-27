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
            var plan = _uniteOfWork.GetRepository<Plan>()
                .GetById(PlanId);
            if (plan is null) return null;
            return new PlanVM()
            {
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                Id = plan.Id,//هو بيعمل Hide ليه عادى 
            };

        }

        public PlanToUpdateVM? GetPlanToUpdate(int PlanId)
        {
            //Get Plan From Database 
            //عشان اعرف اعدل على Plan => Must Active + Not has Active Memberships + Not Null
            //انا مش هينفع اعدل على Plan  الا لما تكون => Active + Not Null + Not Has Active Membership
            var plan = _uniteOfWork.GetRepository<Plan>()
                .GetById(PlanId);
            if (plan is null || plan.IsActive == false || HasActiveMemberships(PlanId)) return null;
            return new PlanToUpdateVM()
            {
                PlanName = plan.Name,
                Price = plan.Price,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
            };
        }

        public bool ToggleStatus(int PlanId)
        {
            //هنا بحول Plan From Active To Deacive  or From Deactive to Active
            //مش هقدر اعمل Dective for Plan that has active Membership
            var plan = _uniteOfWork.GetRepository<Plan>()
                .GetById(PlanId);
            if(plan is null||HasActiveMemberships(PlanId)) return false;
           
            plan.IsActive=plan.IsActive==true ? false : true;
            //object اتغير يبقى انا محتاج اسمع التغيرات دى فى database 
            plan.UpdatedAt = DateTime.Now;
            try
            {
                _uniteOfWork.GetRepository<Plan>()
                      .Update(plan);
                return _uniteOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool UpdatePlan(int PlanId, PlanToUpdateVM planVM)
        {
            var plan = _uniteOfWork.GetRepository<Plan>()
                 .GetById(PlanId);
            //Object PlanVM هو اللى فيه فوق كل Validation بدل مااعملها تانى خت الObject وخلاص 
            if (plan is null||planVM is null) return false;
            #region Way 01
            //plan.Name = planVM.PlanName;
            //plan.Description = planVM.Description;
            //plan.DurationDays = planVM.DurationDays;
            //plan.Price = planVM.Price; 
            #endregion
            //===========================================
            #region Way Using Tuple
            (plan.Name, plan.Description, plan.DurationDays, plan.Price) =
                (planVM.PlanName, planVM.Description, planVM.DurationDays, planVM.Price);

            #endregion
            //===========================================
            plan.UpdatedAt = DateTime.Now;

            try
            {
                _uniteOfWork.GetRepository<Plan>()
                      .Update(plan);
                return _uniteOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }

        }
        //=============================================================
        #region Helper Method 


        private bool HasActiveMemberships(int PlanId)
        {
            return _uniteOfWork.GetRepository<MemberShip>()
                .GetAll(T => T.PlanId==PlanId&&T.Status=="Active").Any();
            //Get All Active Memberships For this Plan
        }


        #endregion
    }
}
