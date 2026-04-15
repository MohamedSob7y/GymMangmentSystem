using GymManagementSystemBLL.View_Models.SessionVm;
using GymMangmentsystemBLL.View_Models.Session_View_Model;
using GymMangmentSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.Services.Interface
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllsessions();
        SessionViewModel? GetSessionDetails(int SessionId);
        bool CreateSession(CreateSessionViewModel createSessionViewModel);
        UpdateSessionViewModel? GetsessionToUpdate(int SessionId);
        bool UpdateSession(int SessionId,UpdateSessionViewModel updateSessionViewModel);
        bool RemoveSession(int SessionId);


        #region Select Trainer_Category
        IEnumerable<CategorySelectViewModel> GetCategoryFromDropDown();
        IEnumerable<TrainerSelectViewmodel> GetTrainerForDropDown(); 
        #endregion
    }
}
