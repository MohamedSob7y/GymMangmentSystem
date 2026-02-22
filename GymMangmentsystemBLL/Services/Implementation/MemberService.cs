using GymMangmentsystemBLL.Services.Interface;
using GymMangmentsystemBLL.View_Models.Member_View_Model;
using GymMangmentSystemDAL.Entities;
using GymMangmentSystemDAL.Repository.Generic_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.Services.Implementation
{
    public class MemberService : IMemberServices
    {
        #region Object From IGenricRepository Before unite Of Work
        private readonly IGenericRepository<Member> _genericRepository;
        //Ask CLr to inject object in Runtime from Any class implement interface IGeneric Repository
        //علم الClr in Main.cs
        public MemberService(IGenericRepository<Member> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        #endregion
        //=====================================================
        #region ALL Services
        public IEnumerable<MemberViewModel> GetallMembers()
        {
            var members = _genericRepository.GetAll();
            if (members is null || !members.Any())
                //return Enumerable.Empty<MemberViewModel>();//Way one
                return [];//way two
            //===========================================
            //Mapping لانه عايز احول من Ienumrable of member to Ienumbrabl of Member view Model  لان دا اللى هيتعرض للuser
            #region Manual Mapping
            #region Way One For Manual Mapping
            //Manual Mapping
            //var emptylistofmembers=new List<MemberViewModel>();
            //foreach(var member in members)
            //{
            //    var memberviewmodel = new MemberViewModel()
            //    {
            //        Id= member.Id,
            //        Name= member.Name,
            //        Email= member.Email,
            //        Phone= member.Phone,
            //        Photo=member.Photo,
            //        Gender=member.Gender.ToString(),
            //    };
            //    emptylistofmembers.Add(memberviewmodel);
            //}
            //return emptylistofmembers;
            #endregion
            //====================================
            #region Way Two For Manual Mapping
            //Manual Mapping  
            var memberviewmodel = members.Select(T => new MemberViewModel
            {
                Id = T.Id,
                Name = T.Name,
                Email = T.Email,
                Phone = T.Phone,
                Photo = T.Photo,
                Gender = T.Gender.ToString()
            });
            return memberviewmodel;

            #endregion
            //====================================
            #endregion
            //===========================================
        }
        //=====================================================
        public bool Create(CreateMember Member)
        {
            //Convert from CreateMember ViewModel To Member To Add it in Database
            //=====================================================
            #region Before Helper Method
            //Buisness Validation For Email and Phone Without Helper Method  
            //لازم يكون الEmail اللى بتدخله مش موجود عشان اقدر اضيف فى Database وكمان اضيف الMember
            var EmailIsExsiste =_genericRepository.GetAll(T=>T.Email==Member.Email).Any();//Must Change GetAll To Take FunC 
            var PhoneIsExsiste =_genericRepository.GetAll(T=>T.Phone==Member.Phone).Any();
            if (EmailIsExsiste || PhoneIsExsiste) return false;
            var memberviewmodel = new Member()
            {
                Name= Member.Name,
                Email= Member.Email,
                Phone= Member.Phone,
                Gender=Member.Gender,
                DateofBirth=Member.DateOfBirth,
                //Address + HealthRecord are Navigation Property
                Address=new Address()
                {
                    BuildingNumber=Member.BuildingNumber,
                    City=Member.City,
                    Street=Member.Street,   
                },
                HealthRecord=new HealthRecord()
                {
                    Height=Member.HealthRecord.Height,
                    Weight=Member.HealthRecord.Weight,
                   Note=Member.HealthRecord.Note,
                   BloodType=Member.HealthRecord.BloodType,
                }
            };
            return _genericRepository.Add(memberviewmodel)>0;
          //as Add return Int عشان كدة عملتها >0

            #endregion
            //=====================================================
            #region After Helper Method

            #endregion

        }
        //=====================================================
        #endregion
        //=====================================================
        #region Helper Methods



        #endregion
    }
}
