using AutoMapper;
using GymManagementSystemBLL.View_Models.SessionVm;
using GymMangmentsystemBLL.View_Models.Member_View_Model;
using GymMangmentsystemBLL.View_Models.MemberSession_Viewmodel;
using GymMangmentsystemBLL.View_Models.MembershipViewModel;
using GymMangmentsystemBLL.View_Models.Plan_View_Model;
using GymMangmentsystemBLL.View_Models.Session_View_Model;
using GymMangmentsystemBLL.View_Models.Trainer_View_Model;
using GymMangmentSystemDAL.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.Mapping
{
    public class MappingProfile : Profile
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
            #region Automatic Mapping For Memberships
            MapMembership();
            #endregion
            //=======================================
            #region Automatic Mapping For MemberSession
            MapMemberSession(); 
            #endregion
        }
        //==========================================
        private void MapMember()
        {
            #region Mapping Address
            #region الطريقة الاولى 
            //امتى اعمل دى
            //لو انا مش هستخدمه تانى غير مرة واحدة فقط ومش عالمه اى VM 
            //يعنى مش هستخدمه تانى فى اى service حتة بتاعت mapping Addresss 
            CreateMap<CreateMember, Member>()
                 .ForMember(dest => dest.Address, options => options.MapFrom(src => new Address
                 {
                     BuildingNumber = src.BuildingNumber,
                     Street = src.Street,
                     City = src.City,
                 }));
            #endregion
            //====================================================
            #region الطريقة التاينه 
            //امتى استخدم دى 
            //لو انا عامل object from Address وهستخدمه فى اكتر من service 
            //ولو انا عامله VM وعايز برضو استخدمه فى اكتر من Service زى Healthrecord
            //CreateMap<CreateMember, Member>()
            //     .ForMember(dest => dest.Address, options => options.MapFrom(src => src));
            //CreateMap<CreateMember, Address>()
            //    .ForMember(dest => dest.BuildingNumber, options => options.MapFrom(src => src.BuildingNumber))
            //     .ForMember(dest => dest.Street, options => options.MapFrom(src => src.Street))
            //      .ForMember(dest => dest.City, options => options.MapFrom(src => src.City));
            #endregion
            //====================================================
            #endregion
            //====================================================
            #region Mapping HealthRecord
            //دى بقا معمولها VM وهستخدمه فى اكتر من services 
            CreateMap<HealthRecordViewmodel, HealthRecord>().ReverseMap();
            #endregion
            //====================================================
            #region Mapping GetAll Service
            CreateMap<Member, MemberViewModel>()
                  .ForMember(dest => dest.Gender, options => options.MapFrom(src => src.Gender.ToString()));
            //لو انا بحول من حاجة مش string to string => Automatic Mapper هو بيعرف لوحده ويعملها من غير ماتعمله 
            //عشان كدة انا مش محتاج اعرفه انى عايز احول من Eunm to string هو كدة كدة هيفهمها 
            #endregion
            //====================================================
            #region GetMemberDetails
            CreateMap<Member, MemberViewModel>()
                .ForMember(dest => dest.Address, options => options.MapFrom(src => $"{src.Address.BuildingNumber}-{src.Address.Street}-{src.Address.City}"))
                .ForMember(dest => dest.DateOfBirth, options => options.MapFrom(src => src.DateofBirth.ToShortDateString()));

            #endregion
            //====================================================
            #region GetMemberDetailsToUpdate
            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber,options => options.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.Street,options =>options.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City,options =>options.MapFrom(src => src.Address.City));
            #endregion
            //====================================================
            #region UpdateMember
            //انا عايزه يعمل Update على نفسobject بتاع address 
            //مش عايزه يعمل object جديد انا عايزه يعدل على نفس المكان 
            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(dest => dest.Name, options => options.Ignore())
                .ForMember(dest => dest.Photo, options => options.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.Address.City = src.City;
                    dest.Address.Street = src.Street;
                    //فى فى فرق كبير  فى services انى انادى على Mapper 
                    //بين Mapper and Update ساعتها مش هعمل Dest.UpdatedAt هنا اروح اعملها فى service انما طالما الفرق مش كبير والاتين ورا بعض على طول عادى اعملها فى automapper
                    dest.UpdatedAt=DateTime.Now;
                });
            #endregion
        }
        //==========================================
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

            //Mapping For Select_Trainer_Category Viewmodel
            #region Mapping For Select_Trainer_Category
            CreateMap<Trainer, TrainerSelectViewmodel>();
            CreateMap<Category, CategorySelectViewModel>()
                .ForMember(dest => dest.Name, options => options.MapFrom(src=>src.CategoryName));
                //فى كمشكلة انى لازم اعرفه ان الاسم بتاع CategoryName==Name اللى موجود فى Viewmodel
            #endregion

        }
        //==========================================
        private void MapTrainer()
        {
            //دى لو انا مش عامله VM + عايزه اعمله object عشان اعمله createing in Heap
            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Address, options => options.MapFrom(src => new Address
                {
                    Street = src.Street,
                    City = src.City,
                    BuildingNumber = src.BuildingNumber,
                }));

            //الطريقة دى لو انا عامله VM وهستخدمه فى اكتر من service 
            //CreateMap<CreateTrainerViewModel, Trainer>()
            //    .ForMember(dest => dest.Address, options => options.MapFrom(src =>src));
            //CreateMap<CreateTrainerViewModel, Address>()
            //    .ForMember(dest => dest.BuildingNumber, options => options.MapFrom(src => src.BuildingNumber))
            //     .ForMember(dest => dest.Street, options => options.MapFrom(src => src.Street))
            //      .ForMember(dest => dest.City, options => options.MapFrom(src => src.City));
            CreateMap<Trainer, TrainerViewModel>();//لوحده هيفهم الحتة دى Speciality.ToString() مش لازم اعرفه 
            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(dest => dest.DateOfBirth, options => options.MapFrom(src => src.DateofBirth.ToShortDateString()))
                .ForMember(dest => dest.Address, options => options.MapFrom(src => $"{src.Address.BuildingNumber}-{src.Address.Street}-{src.Address.City}"));
            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber, options => options.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.Street, options => options.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, options => options.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Specialities,options => options.MapFrom(src => src.Speciality.ToString()));
            CreateMap<TrainerToUpdateViewModel, Trainer>()
                .ForMember(dest => dest.Name, options => options.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.Street = src.Street;
                    dest.Address.City = src.City;
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.UpdatedAt= DateTime.Now;
                });
        }
        //==========================================
        private void MapPlan()
        {
            CreateMap<Plan,PlanVM>();
            CreateMap<Plan, PlanToUpdateVM>();
            CreateMap<PlanToUpdateVM, Plan>()
                .ForMember(dest => dest.Name, Options => Options.Ignore())
                .ForMember(dest => dest.UpdatedAt, Options => Options.MapFrom(src => DateTime.Now));
        }
        //==========================================
        private void MapMembership()
        {
            //For GetAll + GetById
            CreateMap<MemberShip, MembershipVM>();
            //For Creation
            CreateMap<CreateMembershipVM, MemberShip>();
        }
        //==========================================
        private void MapMemberSession()
        {
            CreateMap<MemberSession,MemberSessionVM>();
            //For Create
            CreateMap<MemberSessionVM, MemberSession>();
        }
    }
}
