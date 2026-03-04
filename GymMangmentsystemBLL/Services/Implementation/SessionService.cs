using AutoMapper;
using GymManagementSystemBLL.View_Models.SessionVm;
using GymMangmentsystemBLL.Mapping;
using GymMangmentsystemBLL.Services.Interface;
using GymMangmentsystemBLL.View_Models.Session_View_Model;
using GymMangmentSystemDAL.Entities;
using GymMangmentSystemDAL.Repository.Generic_Repository.Interface;
using GymMangmentSystemDAL.Unit_Of_Work.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.Services.Implementation
{
    public class SessionService:ISessionService
    {
        private readonly IUniteOfWork _uniteOfWork;
        public IMapper _Mapper { get; }
        //Ask CLr To inject Object From IMapper 
        public SessionService(IUniteOfWork uniteOfWork,IMapper mapper)
            
        {
            _uniteOfWork = uniteOfWork;
            _Mapper = mapper;
        }
        //===============================================================================
        public bool CreateSession(CreateSessionViewModel createSessionViewModel)
        {
            //BuisnessRule
            //capacity Is Limited From 1 To 25
            //EndDate must > StartDate
            //TrainerId Exsist  
            //Category Exsist
            if (!TrainerExsist(createSessionViewModel.TrainerId))
                return false;
            if (!CategoryExsist(createSessionViewModel.CategoryId))
                return false;
            if (!TimeValid(createSessionViewModel.StartDate,
           createSessionViewModel.EndDate))
                return false;
            if (createSessionViewModel.Capacity > 25 || createSessionViewModel.Capacity < 0)
                return false;
            //AutomaticMapping
            var MapperSession= _Mapper.Map<CreateSessionViewModel,Session>(createSessionViewModel);
            try
            {
                _uniteOfWork.GetRepository<Session>().Add(MapperSession);
                return _uniteOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }
        //===============================================================================
        public IEnumerable<SessionViewModel> GetAllsessions()
        {
            #region Solving Before GetAllSessikonWithCategoryandTrainer in SessionReposityr 
            //var session=_uniteOfWork.GetRepository<Session>()
            //     .GetAll();
            // if (!session.Any() || session is null)
            //     return [];
            // var sessionVm = session.Select(T => new SessionViewModel()
            // {
            //     Id = T.Id,
            //     Description = T.Description,
            //     StartDate = T.StartDate,
            //     EndDate = T.EndDate,
            //     Capacity = T.Capacity,

            //     //Categoryname + TrainerName+ Avialbel Slots مش هعرف اجبهم حتى بالNavigation Proprty not Loaded 
            //     //TrainerName=session.Trainer.name//is invalid
            //     //Avlaible =Capacity-CountofBookingForSession
            //     //كل مرة بجيب فيها Session Must Get TrainerName+CategoryName => So this Related Date يعنى لازم اشتغل على  Igger Loading
            //     //Using Include بس فى مشكلة عشان اشتغل على Igger Loading Must SessionVM ترجع Querable 
            //     //يبقى لازم اشتغل على حاجة بترجع Querable يعنى class بيكلم الDatabase => in Generic Repository oin GetAll Take Func

            // });
            // return sessionVm; 
            #endregion
            //========================================================================
            #region With SessionRepository
            var session = _uniteOfWork.SessionRepository
                     .GetAllSessionWithCategoryAndTrainer();
            if (session.Any() ||
                session is null)
                return [];
            #region Manual Mapping
            //return session.Select(T => new SessionViewModel()
            //{
            //    Id = T.Id,
            //    Description = T.Description,
            //    StartDate = T.StartDate,
            //    EndDate = T.EndDate,
            //    Capacity = T.Capacity,
            //    TrainerName = T.Trainer.Name,
            //    CategoryName = T.Category.CategoryName,
            //    AvailableSlots = T.Capacity - _uniteOfWork.SessionRepository.GetCountOfBookingSlots(T.Id)
            //});  
            #endregion
            //=========================================
            #region Auto Mapping
            var Mappersession = _Mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(session);
            foreach(var sessions in  Mappersession)
            {
                sessions.AvailableSlots
               = sessions.Capacity -
               _uniteOfWork.SessionRepository
               .GetCountOfBookingSlots(sessions.Id);
            }
           
            return Mappersession;
            #endregion
            //=========================================
            #endregion
        }
        //===============================================================================
        public SessionViewModel? GetSessionDetails(int SessionId)
        {
           var session=_uniteOfWork.SessionRepository
                .GetByIdWithCategoryAndTrainer(SessionId);
            if (session is null) return null;

            #region Manual Mapping
            //return new SessionViewModel()
            //{
            //    Id = session.Id,
            //    Description = session.Description,
            //    StartDate = session.StartDate,
            //    EndDate = session.EndDate,
            //    Capacity = session.Capacity,
            //    TrainerName = session.Trainer.Name,
            //    CategoryName = session.Category.CategoryName,
            //    AvailableSlots = session.Capacity - _uniteOfWork.SessionRepository.GetCountOfBookingSlots(session.Id)
            //};

            #endregion
            //===============================================================================
            #region Auto Mapping
            var Mappersession=_Mapper.Map<Session,SessionViewModel>(session);
            Mappersession.AvailableSlots 
                = Mappersession.Capacity -
                _uniteOfWork.SessionRepository
                .GetCountOfBookingSlots(session.Id);
            return Mappersession;
            #endregion
        }
        //===============================================================================
        public UpdateSessionViewModel? GetsessionToUpdate(int SessionId)
        {
           
        }
        //===============================================================================
        public bool RemoveSession(int SessionId)
        {
           
        }
        //===============================================================================
        public bool UpdateSession(int SessionId, UpdateSessionViewModel updateSessionViewModel)
        {
            
        }
        //===============================================================================
        #region Helper Method
        private bool TrainerExsist(int TrainerId)
        {
            return _uniteOfWork.GetRepository<Trainer>()
                .GetById(TrainerId) is not null;
        }
        private bool CategoryExsist(int CategoryId)
        {
            return _uniteOfWork.GetRepository<Category>()
                .GetById(CategoryId) is not null;
        }
        private bool TimeValid(DateTime StartDate,DateTime EndDate)
        {
            return EndDate > StartDate;
        }
        #endregion
    }
}
