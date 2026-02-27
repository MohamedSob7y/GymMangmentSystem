using GymMangmentsystemBLL.View_Models.Trainer_View_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.Services.Interface
{
    public interface ITrainerService
    {
        IEnumerable<TrainerViewModel> GetAllTrainers();
        bool CreateTrainer(CreateTrainerViewModel createTrainerViewModel);
        TrainerViewModel? GetTrainerDetails(int TrainerId);
        TrainerToUpdateViewModel? GetTrainerDetailsToUpdate(int TrainerId);
        bool UpdateTrainer(int TrainerId,TrainerToUpdateViewModel modelToUpdate);
        bool DeleteTrainer(int TrainerId);
    }
}
