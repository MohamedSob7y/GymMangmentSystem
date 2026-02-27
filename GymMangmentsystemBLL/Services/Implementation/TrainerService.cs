using GymMangmentsystemBLL.Services.Interface;
using GymMangmentsystemBLL.View_Models.Trainer_View_Model;
using GymMangmentSystemDAL.Entities;
using GymMangmentSystemDAL.Unit_Of_Work.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.Services.Implementation
{
    public class TrainerService:ITrainerService
    {
        private readonly IUniteOfWork _uniteOfWork;

        public TrainerService(IUniteOfWork uniteOfWork)
        {
            _uniteOfWork = uniteOfWork;
        }

        public bool CreateTrainer(CreateTrainerViewModel createTrainerViewModel)
        {
            var IsEmailExsist=_uniteOfWork.GetRepository<Trainer>()
                .GetAll(T=>T.Email==createTrainerViewModel.Email).Any();
            var IsPhoneExsist = _uniteOfWork.GetRepository<Trainer>()
              .GetAll(T => T.Phone == createTrainerViewModel.Phone).Any();
            //Canot Create Trainer with The Same Email + Phone مش هينفع ادخل Phone + email موجودين بالفعل فى Database 
            if (createTrainerViewModel is null || IsEmailExsist|| IsPhoneExsist)
                return false;
            var trainer = new Trainer()
            {
                Name = createTrainerViewModel.Name,
                Email = createTrainerViewModel.Email,
                Phone = createTrainerViewModel.Phone,
                Gender = createTrainerViewModel.Gender,
                DateofBirth = createTrainerViewModel.DateOfBirth,
                Speciality = createTrainerViewModel.Specialities,
                Address = new Address
                {
                    City = createTrainerViewModel.City,
                    Street = createTrainerViewModel.Street,
                    BuildingNumber = createTrainerViewModel.BuildingNumber,
                }
            };
            try
            {
                _uniteOfWork.GetRepository<Trainer>()
                      .Add(trainer);
                return _uniteOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }


        public bool DeleteTrainer(int TrainerId)
        {
            var trainer=_uniteOfWork.GetRepository<Trainer>().GetById(TrainerId);
            //Canont Delete Any Trainer that has Future Session
            var FutureSessions=_uniteOfWork.GetRepository<Session>()
                .GetAll(T=>T.TrainerId == TrainerId&&T.StartDate>DateTime.Now).Any();
            if(trainer is null || FutureSessions)
                return false;
            try
            {
                _uniteOfWork.GetRepository<Trainer>()
                       .Delete(trainer);
                return _uniteOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var Trainers = _uniteOfWork.GetRepository<Trainer>()
                .GetAll();
            if (Trainers is null||!Trainers.Any())
                return [];
            return Trainers.Select(T => new TrainerViewModel()
            {
                Id=T.Id,
                Name=T.Name,
                Email=T.Email,
                Specialities=T.Speciality.ToString(),
                Phone=T.Phone,
            });
        }

        public TrainerViewModel? GetTrainerDetails(int TrainerId)
        {
            var trainer = _uniteOfWork.GetRepository<Trainer>()
                   .GetById(TrainerId);
            if(trainer is null) 
                return null;
            return new TrainerViewModel()
            {
                Email= trainer.Email,
                Phone=trainer.Phone,
                DateOfBirth=trainer.DateofBirth.ToShortDateString(),
                Address=$"{trainer.Address.BuildingNumber}-{trainer.Address.City}-{trainer.Address.Street}"
            };
        }

        public TrainerToUpdateViewModel? GetTrainerDetailsToUpdate(int TrainerId)
        {
            var trainer = _uniteOfWork.GetRepository<Trainer>()
                  .GetById(TrainerId);
            if (trainer is null)
                return null;
            return new TrainerToUpdateViewModel()
            {
                Email = trainer.Email,
                Phone = trainer.Phone,
               Name=trainer.Name,
               Street=trainer.Address.Street,
               City=trainer.Address.City,
               BuildingNumber=trainer.Address.BuildingNumber,
               Specialities=trainer.Speciality,
            };
        }

        public bool UpdateTrainer(int TrainerId, TrainerToUpdateViewModel modelToUpdate)
        {
            var trainer = _uniteOfWork.GetRepository<Trainer>()
                  .GetById(TrainerId);
          var EmailExsist=_uniteOfWork.GetRepository<Trainer>()
                .GetAll(T=>T.Email==modelToUpdate.Email&&T.Id!=TrainerId).Any();
            var PhoneExsist = _uniteOfWork.GetRepository<Trainer>()
              .GetAll(T => T.Phone == modelToUpdate.Phone && T.Id != TrainerId).Any();

            if(EmailExsist || PhoneExsist||modelToUpdate is null)
                return false;
            trainer.Name = modelToUpdate.Name;
            trainer.Email = modelToUpdate.Email;
            trainer.Phone = modelToUpdate.Phone;
            trainer.Address.City = modelToUpdate.City;
            trainer.Address.Street = modelToUpdate.Street;
            trainer.Address.BuildingNumber = modelToUpdate.BuildingNumber;
            trainer.Speciality = modelToUpdate.Specialities;
            trainer.UpdatedAt= DateTime.Now;

            try
            {
                _uniteOfWork.GetRepository<Trainer>()
                     .Update(trainer);
                return _uniteOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }

        }
       
    }
}
