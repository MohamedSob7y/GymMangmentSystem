using GymMangmentsystemBLL.Services.Interface;
using GymMangmentsystemBLL.View_Models.Member_View_Model;
using GymMangmentSystemDAL.Entities;
using GymMangmentSystemDAL.Repository.Generic_Repository.Interface;
using GymMangmentSystemDAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.Services.Implementation
{
    public class MemberService : IMemberServices
    {
        #region Object From IGenricRepository Before unite Of Work
        private readonly IGenericRepository<Member> _genericRepository;
        private readonly IPlanRepository _planRepository;

        public IGenericRepository<MemberShip> _MembershipRepository { get; }
        public IGenericRepository<HealthRecord> _HealthRecordRepository { get; }

        //Ask CLr to inject object in Runtime from Any class implement interface IGeneric Repository
        //علم الClr in Main.cs
        public MemberService(IGenericRepository<Member> genericRepository
            , IGenericRepository<MemberShip> Membership
            , IPlanRepository planRepository,
            IGenericRepository<HealthRecord> HealthRecord)
        {
            _genericRepository = genericRepository;
            _MembershipRepository = Membership;
            _planRepository = planRepository;
            _HealthRecordRepository = HealthRecord;
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
            try
            {
                //Convert from CreateMember ViewModel To Member To Add it in Database
                //=====================================================
                #region Before Helper Method
                //Buisness Validation For Email and Phone Without Helper Method  
                //لازم يكون الEmail اللى بتدخله مش موجود عشان اقدر اضيف فى Database وكمان اضيف الMember
                //var EmailIsExsiste = _genericRepository.GetAll(T => T.Email == Member.Email).Any();//Must Change GetAll To Take FunC 
                //var PhoneIsExsiste = _genericRepository.GetAll(T => T.Phone == Member.Phone).Any();
                //if (EmailIsExsiste || PhoneIsExsiste) return false;
                #endregion
                //=====================================================
                #region After Helper Method
                if(IsEmailExist(Member.Email)||IsPhoneExist(Member.Phone))
                    return false;
                #endregion
                //=====================================================
                #region Manual Mapping
                var memberviewmodel = new Member()
                {
                    Name = Member.Name,
                    Email = Member.Email,
                    Phone = Member.Phone,
                    Gender = Member.Gender,
                    DateofBirth = Member.DateOfBirth,
                    //Address + HealthRecord are Navigation Property
                    Address = new Address()
                    {
                        BuildingNumber = Member.BuildingNumber,
                        City = Member.City,
                        Street = Member.Street,
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Height = Member.HealthRecord.Height,
                        Weight = Member.HealthRecord.Weight,
                        Note = Member.HealthRecord.Note,
                        BloodType = Member.HealthRecord.BloodType,
                    }
                };
                #endregion
                //=====================================================
                return _genericRepository.Add(memberviewmodel) > 0;
                //as Add return Int عشان كدة عملتها >0
            }
            catch (Exception)
            {

                return false;
            } 
        }
        //=====================================================
        public MemberViewModel? GetMemberDetails(int MemberId)
        {
            var member = _genericRepository.GetById(MemberId);
            if (member is null) return null;
            var memberviewmodel = new MemberViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateofBirth.ToShortDateString(),
                Phone = member.Phone,
                Photo = member.Photo,
                Address = $"{member.Address.BuildingNumber}-{member.Address.City}-{member.Address.Street}",
                //عملت الaddress بالطريقة دى عشان انا اصلا عامله string Property 
                //لو عامله Navigation Property or Any Type ساعتها لازم اعمله by this Way
                /*
                  Address=new Address()
                {
                    BuildingNumber=Member.BuildingNumber,
                    City=Member.City,
                    Street=Member.Street,   
                },
                 */
                //==========================================
                #region Problem 
                //دول كدة عرفت ارجعهم عشان موجودين فى نفس classMember but 
                //PlanName + MembershipStartDate+MembershipEndate مش موجودين فى class Member بالتالى مش هعرف ارجعهم بالطريقة دى لازم اعمل Query on Table Membership+Plan To Get Data From This Table And return it 

                // MembershipStartDate = member.Memberships.FirstOrDefault(T=>T.Status=="Active")?
                //.CreatedAt.ToShortDateString()


                //مشكلة الطريقة دى اية؟
                //Data Not Loaded => حتى لو عملت include 
                // include with Igger Loading شغالى مع Querable
                //So عشان كدة لازم اعمل change in GetAll + GetById احط include انى احمل Membership



                #endregion
                //==========================================
            };
            //==========================================
            #region Solving Problem With PlanName+Memnership
            //عشان اجيب MembershipstartDate+EndDate لازم اعمل Query على table Membership
            //عشان اجيب plan لازم اعمل Query on Table plan
            //لان plan Name مش موجودة غير فى table Plan


            //عايز اجيب active Membership
            var membership = _MembershipRepository.GetAll(T => T.MemberId == MemberId && T.Status == "Active").FirstOrDefault();//Get All Membership for this Member
                                                                                                                                //كدة انا جبت كل Membership for This Member اللى انا بدور عليه 
                                                                                                                                //انا بقا هبدا اعمل Filteration For Memberships عايز بقا اللActive

            if (membership is not null)
            {
                memberviewmodel.MembershipStartDate = membership.CreatedAt.ToShortDateString();
                memberviewmodel.MembershipEndDate = membership.EndDate.ToShortDateString();
                var plan = _planRepository.GetById(membership.PlanId);
                memberviewmodel.PlanName = plan.Name;
            }

            //To Get Plan Name Must Make Query For plan => Get PlanName by Membership
            //by Plan Id in Class Memebrship => هقدر اوصل PlanName in Table Plan لان فى علاقة بينهم
            #endregion
            //==========================================
            return memberviewmodel;
        }
        //=====================================================
        public HealthRecordViewmodel? GetMemberHealthDetails(int MemberId)
        {
            #region With GenericRepository of Member
            //var member = _genericRepository.GetById(MemberId);
            //if (member is null) return null;
            //var healthrecordviewmodel = new HealthRecordViewmodel()
            //{
            //    Height = member.HealthRecord.Height,
            //    Weight = member.HealthRecord.Weight,
            //};
            //return healthrecordviewmodel; 
            #endregion
            //=====================================
            #region With Generic Repository Of HealthRecord 
            //لانى لو كلمت MemberRepository كدة انا برجع Data انا مش عايزها انا بس عايز Data خاصة بالHealthRecord
            var memberhealthrecord = _HealthRecordRepository.GetById(MemberId);
            if (memberhealthrecord is null) return null;
            //=====================================
            #region Manual Mapping
            return new HealthRecordViewmodel()
            {
                Height = memberhealthrecord.Height,
                Weight = memberhealthrecord.Weight,
                BloodType = memberhealthrecord.BloodType,
                Note = memberhealthrecord.Note,
            };
            #endregion
            //=====================================
            #region Automatic Mapping


            #endregion
            //=====================================
            #endregion
            //=====================================
        }
        //=====================================================
        public MemberToUpdateViewModel? GetMemberDetailsToUpdate(int MemberId)
        {
            var member = _genericRepository.GetById(MemberId);
            if (member is null) return null;
            #region Manual Mapping
            return new MemberToUpdateViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                BuildingNumber = member.Address.BuildingNumber,
                City = member.Address.City,
                Street = member.Address.Street,
            };
            #endregion
        }

        public bool UpdateMember(int MemberId, MemberToUpdateViewModel memberToUpdateViewModel)
        {
            try
            {
                //Make Validation for Email + Phone So make Helper Method 
                #region Before Helper Method
                //var EmailExsist = _genericRepository.GetAll(T => T.Email == memberToUpdateViewModel.Email).Any();
                //var PhoneExsist = _genericRepository.GetAll(T => T.Phone == memberToUpdateViewModel.Phone).Any();
                //if (EmailExsist && PhoneExsist) return false; 
                #endregion
                //==============================================
                #region After Helper Method
                if(IsEmailExist(memberToUpdateViewModel.Email)||
                    IsPhoneExist(memberToUpdateViewModel.Phone))
                    return false;
                //مش عايز يدخلى اى email موجود or Phone موجود

                #endregion
                //==============================================
                //Get Member 
                var member = _genericRepository.GetById(MemberId);
                if (member is null) return false;
                member.Name = memberToUpdateViewModel.Name;
                //والله لو عدل على Name خلاص save changes 
                //لو معملشى اى تعديل خلاص هات القيمة القديمة من database 
                member.Email = memberToUpdateViewModel.Email;
                member.Phone = memberToUpdateViewModel.Phone;
                member.Photo = memberToUpdateViewModel.Photo;
                member.Address.BuildingNumber = memberToUpdateViewModel.BuildingNumber;
                member.Address.City = memberToUpdateViewModel.City;
                member.Address.Street = memberToUpdateViewModel.Street;
                member.UpdatedAt = DateTime.Now;
                return _genericRepository.Update(member) > 0;
                //as Update return int عشان كدة عملتها >0
                //لو عدد الRow حصلها affected >0 return true
                //For Any Transcation => Must Make TryCatch 
            }
            catch (Exception)
            {

                return false;
            }
        }
        //=====================================================



        //=====================================================
        #endregion
        //=====================================================
        #region Helper Methods
        private bool IsEmailExist(string email)
        {
            return _genericRepository.GetAll(T => T.Email == email).Any();
        }
        private bool IsPhoneExist(string phone)
        {
            return _genericRepository.GetAll(T => T.Phone == phone).Any();
        }

        #endregion
    }
}
