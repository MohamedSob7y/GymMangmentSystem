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
using System.Net;
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
            //مفيش هنا بقا TrainerName+CategoryName يبقى استخدم GetReposiroty Not SessionRepository هستخدم عادى GetAll مش GetSessionTrainer+Category
            var session = _uniteOfWork.GetRepository<Session>()
                 .GetById(SessionId);
            //BuisneessRule For 
            //Buseness Rule => Canont Update Session has Active Booking
            // is Session is Null => No Updateing
            //Sessionis Completed => No Updated
            //Session is Startded => No Updateing
            //Make Helper Method has All Validation 
            //محتاج Valida For isTrainerIsExsist عشان اعرف انه متغيرشى 
            if (!IsSessionAvailableToUpdate(session!)) return null;
            #region Manual Mapping
            //return new UpdateSessionViewModel()
            //{
            //    TrainerId = session.TrainerId, //دا موجود اصلا بطريقة مباشرة فى Table session عشان كدة خليته هو اللى يعملها مش انا اللى بعمله فيها 
            //    Description = session.Description,
            //    EndDate=session.EndDate,
            //    StartDate=session.StartDate,

            //};
            #endregion
            //===============================================================================
            #region Automatic Mapping
            //Automatic Mapping 
            return _Mapper.Map<Session,UpdateSessionViewModel>(session);
            #endregion
        }
        //===============================================================================
        public bool RemoveSession(int SessionId)
        {
            try
            {
                var session = _uniteOfWork.GetRepository<Session>()
                        .GetById(SessionId);
                //Canont Delete Session with Future Date
                //Canont Delete Sessiion ongoign and Not Completed 
                //If Session has ActiveBooking =>Not Delete this
                //Make Helper Method
                if (!ISessionValidToDelete(session!)) return false;
                _uniteOfWork.GetRepository<Session>()
                  .Delete(session!);
                return _uniteOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
               
        }
        //===============================================================================
        public bool UpdateSession(int SessionId, UpdateSessionViewModel updateSessionViewModel)
        {
            try
            {
                var session = _uniteOfWork.GetRepository<Session>()
                      .GetById(SessionId);
                if (!IsSessionAvailableToUpdate(session!) ||
                    !TrainerExsist(updateSessionViewModel.TrainerId) ||
                    !TimeValid(updateSessionViewModel.StartDate, updateSessionViewModel.EndDate)) return false;
                //يعمل كدة تانى Validation in This Services مع انى عملتها اصلا فى GetSessionToUpdate عشان ممكن يكونوا غيره فى Id in Request 
                #region Using Manual Mapping
                //session.StartDate = updateSessionViewModel.StartDate;
                //session.EndDate = updateSessionViewModel.EndDate;
                //session.TrainerId = updateSessionViewModel.TrainerId;
                //session.Description = updateSessionViewModel.Description; 
                #endregion

                //using Automapper
                _Mapper.Map(updateSessionViewModel, session);
                session!.UpdatedAt = DateTime.Now;
                _uniteOfWork.GetRepository<Session>()
                    .Update(session!);
                return _uniteOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
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
        private bool IsSessionAvailableToUpdate(Session session)
        {
            //مش هينفع اعمل Update For session لو هى Null => No Updating
            // Session is Started => No Updating
            //Session is Completed => No updating
            //Session has Active Booking => No Updating
            if (session is null) return false;
            if (session.StartDate<=DateTime.Now) return false;//Session is OnGoing شغالة 
            if (session.EndDate<DateTime.Now) return false;//Session is Completed 
            var ActiveBooking = _uniteOfWork.SessionRepository.GetCountOfBookingSlots(session.Id)>0;//لو رجعلى عدد الناس اللى حاجزة علىsession دى يبقى مش هعرف اعملها Update
            if(ActiveBooking) return false;
            return true;
        }
        private bool ISessionValidToDelete(Session session)
        {
            if(session is null) return false;
            if (session!.StartDate > DateTime.Now)//UpComing
                return false;
            if (session.StartDate <= DateTime.Now && 
                session.EndDate > DateTime.Now)//OnGoing
                return false;
            var ActiveBooking = _uniteOfWork.SessionRepository.GetCountOfBookingSlots(session.Id)>0;
            if (ActiveBooking) return false;
            return true;
        }
        #endregion
    }
}
