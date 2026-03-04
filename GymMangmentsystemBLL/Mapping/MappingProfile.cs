using AutoMapper;
using GymManagementSystemBLL.View_Models.SessionVm;
using GymMangmentsystemBLL.View_Models.Session_View_Model;
using GymMangmentSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            #region Automatic Mapping For Members
            MapMember();
            #endregion
            //=======================================
            #region Automatic Mapping For Trainers
            MapTrainer();
            #endregion
            //=======================================
            #region Automatic Mapping For Session
            MapSession();
            #endregion
            //=======================================
            #region Automatic Mapping For Plan
            MapPlan();
            #endregion
            //=======================================
        }

        private void MapMember()
        {

        }
        private void MapSession()
        {
           
            CreateMap<Session, SessionViewModel>()
                .ForMember(des => des.TrainerName, options => options.MapFrom(Src => Src.Trainer.Name)).
                ForMember(des => des.CategoryName, options => options.MapFrom(Src => Src.Category.CategoryName))
                .ForMember(des => des.AvailableSlots, option => option.Ignore());//مش عايزه يعمل Mapping For Avialble Slots عشان انا هعلمها فى Service  
            //بس مش هيعرف يحول TrainerName+CategoryName+ Available Slots 
            //Automapper يحول fRom Object To Object  has the same Strcture => Same Datatype + نفس اسامى الProprty
            //غير كدة لازم اعرفه 
            //ةكمان لو فى حاجة مش عارف يحولها بطريقة مباشرة يعنى زى address دى مش هيعرف يجبها بطريقة مباشرة
            //لازم اعلمه فى الحتة دى انما باقى الحجات اللى بيعرف يجبها بطريقة مباشرة على طول فى Manual Mapping => مش هعلمه هو بيعملها بطريقة مباشرة على طول 
            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();

        }
        private void MapTrainer()
        {

        }
        private void MapPlan()
        {

        }
    }
}
