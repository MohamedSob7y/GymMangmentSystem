using GymMangmentsystemBLL.View_Models.Plan_View_Model;
using GymMangmentSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.Services.Interface
{
    public interface IPlanService
    {
        IEnumerable<PlanVM> GetAllPlans();
        //===================================================
        PlanVM? GetPlanDetails(int PlanId);
        //===================================================
        PlanToUpdateVM? GetPlanToUpdate(int PlanId);    
        bool UpdatePlan(int PlanId,PlanToUpdateVM planVM);
        //===================================================
        bool ToggleStatus(int PlanId);
    }
}
