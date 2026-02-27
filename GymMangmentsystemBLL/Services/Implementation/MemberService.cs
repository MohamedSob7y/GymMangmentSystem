using GymMangmentsystemBLL.Services.Interface;
using GymMangmentsystemBLL.View_Models.Member_View_Model;
using GymMangmentSystemDAL.Entities;
using GymMangmentSystemDAL.Repository.Generic_Repository.Implementation;
using GymMangmentSystemDAL.Repository.Generic_Repository.Interface;
using GymMangmentSystemDAL.Repository.Implementation;
using GymMangmentSystemDAL.Repository.Interface;
using GymMangmentSystemDAL.Unit_Of_Work.Interface;
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
        //private readonly IGenericRepository<Member> _genericRepository;
        //private readonly IPlanRepository _planRepository;
        //private readonly IGenericRepository<MemberSession> _membersession;

        //public IGenericRepository<MemberShip> _MembershipRepository { get; }
        //public IGenericRepository<HealthRecord> _HealthRecordRepository { get; }
        //public IGenericRepository<Session> _SessionRepo { get; }

        ////Ask CLr to inject object in Runtime from Any class implement interface IGeneric Repository
        ////علم الClr in Main.cs
        //public MemberService(IGenericRepository<Member> genericRepository
        //    , IGenericRepository<MemberShip> Membership
        //    , IPlanRepository planRepository,
        //    IGenericRepository<HealthRecord> HealthRecord,
        //    IGenericRepository<MemberSession> Membersession,
        //    IGenericRepository<Session> SessionRepo)
        //{
        //    _genericRepository = genericRepository;
        //    _MembershipRepository = Membership;
        //    _planRepository = planRepository;
        //    _HealthRecordRepository = HealthRecord;
        //    _membersession = Membersession;
        //    _SessionRepo = SessionRepo;
        //}
        #endregion
        //=====================================================
        #region  Object From  unite Of Work
        private readonly IUniteOfWork _uniteOfWork;
        //Ask CLR To inject object in Runtime from any class implement interface Iuniteofwork
        public MemberService(IUniteOfWork uniteOfWork)
        {
            _uniteOfWork = uniteOfWork;
        }
        #endregion
        //=====================================================
        #region ALL Services
        public IEnumerable<MemberViewModel> GetallMembers()
        {
            //var members = _genericRepository.GetAll();//Before UniteOfWork
            var members=_uniteOfWork.GetRepository<Member>()
                .GetAll();
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
                //var EmailIsExsiste = _genericRepository.GetAll(T => T.Email == Member.Email).Any();//Must Change GetAll To Take FunC 
                //var PhoneIsExsiste = _genericRepository.GetAll(T => T.Phone == Member.Phone).Any();
                //if (EmailIsExsiste || PhoneIsExsiste) return false;
                #endregion
                //=====================================================
                #region After Helper Method
                if(IsEmailExist(Member.Email)||IsPhoneExist(Member.Phone)||Member is null)
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
            //return _genericRepository.Add(memberviewmodel) > 0; //Before unite ofWork
            //as Add return Int عشان كدة عملتها >0

            //After Unite Of Work
            try
            {
                _uniteOfWork.GetRepository<Member>()
                           .Add(memberviewmodel);
                return _uniteOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
            
           
        }
        //=====================================================
        public MemberViewModel? GetMemberDetails(int MemberId)
        {
            //var member = _genericRepository.GetById(MemberId);//Before UnitfWork
            var member = _uniteOfWork.GetRepository<Member>().GetById(MemberId);
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
            //var membership = _MembershipRepository.GetAll(T => T.MemberId == MemberId && T.Status == "Active").FirstOrDefault();//Get All Membership for this Member => Before UnitOfWork
            //كدة انا جبت كل Membership for This Member اللى انا بدور عليه 
            //انا بقا هبدا اعمل Filteration For Memberships عايز بقا اللActive
            var membership = _uniteOfWork.GetRepository<MemberShip>()
                .GetAll(T => T.MemberId == MemberId && T.Status == "Active")
                .FirstOrDefault();


            if (membership is not null)
            {
                memberviewmodel.MembershipStartDate = membership.CreatedAt.ToShortDateString();
                memberviewmodel.MembershipEndDate = membership.EndDate.ToShortDateString();
                //var plan = _planRepository.GetById(membership.PlanId);//Before UnitOfWork

                var plan = _uniteOfWork.GetRepository<Plan>().GetById(membership.PlanId);//If Use InuitOfWork Of Plan انما لو هستخدم Repository الخاصة بالPlan لازم اعمل Inject من فوق 
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
            //var memberhealthrecord = _HealthRecordRepository.GetById(MemberId);//Before UnitOfWork
            var memberhealthrecord = _uniteOfWork.GetRepository<HealthRecord>().GetById(MemberId);
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
            //var member = _genericRepository.GetById(MemberId);//Before UnitOfWork
            var member = _uniteOfWork.GetRepository<Member>().GetById(MemberId);
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
                //var member = _genericRepository.GetById(MemberId);//Before UnitOfWork
                var member = _uniteOfWork.GetRepository<Member>().GetById(MemberId);
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
                //return _genericRepository.Update(member) > 0;//Before UnitOfWork
                //as Update return int عشان كدة عملتها >0
                //لو عدد الRow حصلها affected >0 return true
                //For Any Transcation => Must Make TryCatch
                
                _uniteOfWork.GetRepository<Member>()
                    .Update(member);
                return _uniteOfWork.SaveChanges()>0;

            }
            catch (Exception)
            {

                return false;
            }
        }
        //=====================================================
        public bool RemoveMember(int MemberId)
        {
            try
            {
                //BuisnessRule  مش هينفع امسح اى Member has Active Booking يعنى حاجز session لسة معادها مجاش in Table MemberSession  
                //var member = _genericRepository.GetById(MemberId);//Before UnitOfWork
                var member = _uniteOfWork.GetRepository<Member>().GetById(MemberId);
                if (member is null) return false;
                //كدة انا خلاص جبته عايز بقا اشوف هل عنده Active Session Booking 
                //عشان لو عنده session لسة معادها مجاش يبقى مش همسحه
                //var MemberSessionIds = _membersession.
                //    GetAll(T => T.MemberId == MemberId)//Before UnitOfWork
                //    .Select(T => T.SessionId);//Get All MemberSession For This Member

                var MemberSessionIds = _uniteOfWork.GetRepository<MemberSession>().
                   GetAll(T => T.MemberId == MemberId)//After UnitOfWork
                   .Select(T => T.SessionId);

                //انا مش عايزه يرجعلى كل المعلومات انا فقط عايز SessionId عشان هعدى على كل Session واشوفها هل معادها لسة مجاش ولا لاء 
                //var HasFutureSessions = _SessionRepo.//Before UnitOfWork
                //  GetAll(S => MemberSessionIds
                //  .Contains(S.Id) && S.StartDate > DateTime.Now)
                //  .Any();//هات كل sessions اللى لسة معادها مجاش

                var HasFutureSessions = _uniteOfWork.GetRepository<Session>().
                  GetAll(S => MemberSessionIds
                  .Contains(S.Id) && S.StartDate > DateTime.Now)
                  .Any();//هات كل sessions اللى لسة معادها مجاش


                if (HasFutureSessions)
                    return false;//مش هينفع اسمحه لانه عنده Future Session


                //مش هبنفع اسمح الMember على طول لازم امسح الاول All Memberships اللى عليه وبعد كدة امسحه
                //var Memebrships = _MembershipRepository.GetAll(T => T.MemberId == MemberId);//Get All Memberships for This Member   => Before UnitOfWork
                var Repo03 = _uniteOfWork.GetRepository<MemberShip>();
                var Memebrships = Repo03.GetAll(T => T.MemberId == MemberId);
                if (Memebrships.Any())
                {
                    //Iterate For Each Memberships واسمحها 
                    foreach (var Memebrship in Memebrships)
                    {
                        //_MembershipRepository.Delete(Memebrship);//First Trancsation //Before UnitOfWork
                        Repo03.Delete(Memebrship);   
                    }
                }
                //return _genericRepository.Delete(member) > 0;//Second Transcation    //Before UnitOfWork
                _uniteOfWork.GetRepository<Member>()
                   .Delete(member);
                return _uniteOfWork.SaveChanges()>0;
            }
            catch (Exception)
            {

                return false;
            }
            #region Problem Before UniteOfWork
            //المشكلة هنا ان Service دى كلمت وعملت Two Tracnscation in Database المفروض انى اكلم الDatabase 
            //As Each Tracnscation Make Save Changes on Database انا عايز اعلمها مرة واحدة فقط 
            //يعنى عايز كل Service تروح تكلم الDatabase مرة واحدة فقط وتعمل Save Chganges مرة واحدة ايا كان عدد الOperations اللى بمعلها فى service 
            //Use Unite Of Work Pattern حل المشكلة دى انه Collect All Transcation Of Each Service  وراح كلم Database مرة واحدة فقط ومجمع كل الOpertaiton and Save chages مرة واحدة فقط 
            //عشان مش Beteer Performnace انى كس شوية اكلم Databas 
            //Unite of work use Single automic transcation يعنى كل Operations اللى موجودة فى نفس service جمعها كلها مرة واحدة ويروح يكلم الdatabase مرة واحدة بس لو فى Operation واحدة على الاقل مش شغالة كدة مش هينفع يرو يكلم الDataabse لازم كله شكون شغال 
            #endregion

        }
        //=====================================================
        #endregion
        //=====================================================
        #region Helper Methods
        private bool IsEmailExist(string email)
        {
            //return _genericRepository.GetAll(T => T.Email == email).Any();//Before UnitOfWork
            return _uniteOfWork.GetRepository<Member>()
                 .GetAll(T => T.Email == email).Any();
        }
        private bool IsPhoneExist(string phone)
        {
            //return _genericRepository.GetAll(T => T.Phone == phone).Any();//Before Unit Of Work
            return _uniteOfWork.GetRepository<Member>()
               .GetAll(T => T.Phone == phone).Any();
        }

      

        #endregion
    }
}
