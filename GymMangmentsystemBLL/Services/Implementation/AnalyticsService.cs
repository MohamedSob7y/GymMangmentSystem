using GymMangmentsystemBLL.Services.Interface;
using GymMangmentsystemBLL.View_Models.Analytic_Service_View_Model;
using GymMangmentSystemDAL.Entities;
using GymMangmentSystemDAL.Unit_Of_Work.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.Services.Implementation
{
    public class AnalyticsService:IAnalyticsService
    {
        private readonly IUniteOfWork _uniteOfWork;

        public AnalyticsService(IUniteOfWork uniteOfWork)
        {
           _uniteOfWork = uniteOfWork;
        }

        public HomeAnalyticsViewModel GetHomeAnalyticsService()
        {
            //GetAllMembers
            //Get ActiveMembers=> Has Active Memberships
            //Get All Trainers
            //Get UpComingSessions
            //Get OnGoingSessions
            //Get Completed Session
            var session=_uniteOfWork.GetRepository<Session>().GetAll();
            return new HomeAnalyticsViewModel()
            {
                TotalMembers = _uniteOfWork.GetRepository<Member>().GetAll().Count(),
                TotalTrainers = _uniteOfWork.GetRepository<Trainer>().GetAll().Count(),
                ActiveMembers = _uniteOfWork.GetRepository<MemberShip>().GetAll(T => T.Status == "Active").Count(),
                //===========================================================
                #region Problem 
                //UpComingSessions = _uniteOfWork.GetRepository<Session>().GetAll(T => T.StartDate > DateTime.Now).Count(),//This Request For Data base 
                //OnGoingSessions = _uniteOfWork.GetRepository<Session>().GetAll(T => T.StartDate <= DateTime.Now && T.EndDate > DateTime.Now).Count(),//This Request For Data base 
                //CompletedSessions = _uniteOfWork.GetRepository<Session>().GetAll(T => T.EndDate < DateTime.Now).Count()//This Request For Data base 
                //                                                                                                       //انا عايز ابعت request مرة واحدة فقط لTable session اجيب كل session مرة واحدة عندى فى Memory + Filter Sessions عندى فى Memory بدل ماكل شوية ابعتله ويعمل Filteration in Database  
                #endregion
                //===========================================================
                #region Solution With More Optimization
                //كدة session بقت loaded عندى فى Memory
                UpComingSessions=session.Count(T=>T.StartDate>DateTime.Now),
                OnGoingSessions= session.Count(T => T.StartDate <= DateTime.Now&&T.EndDate>=DateTime.Now),
                CompletedSessions= session.Count(T =>T.EndDate<DateTime.Now )
                #endregion
            };
        }
    }
}
