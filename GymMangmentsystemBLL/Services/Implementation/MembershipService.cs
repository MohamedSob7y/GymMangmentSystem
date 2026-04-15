using AutoMapper;
using GymMangmentsystemBLL.Services.Interface;
using GymMangmentsystemBLL.View_Models.MembershipViewModel;
using GymMangmentSystemDAL.Entities;
using GymMangmentSystemDAL.Unit_Of_Work.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.Services.Implementation
{
    public class MembershipService: IMembershipService
    {
        #region Constructor
        private readonly IUniteOfWork _uniteOfWork;
        public IMapper _Mapper { get; }

        public MembershipService(IUniteOfWork uniteOfWork, IMapper mapper)
        {
            _uniteOfWork = uniteOfWork;
            _Mapper = mapper;
        }


        #endregion
        //==============================================
        public IEnumerable<MembershipVM> GetAll()
        {
           var memberships=_uniteOfWork.GetRepository<MemberShip>()
                .GetAll();
            if (memberships is null || !memberships.Any())
                return [];
           return _Mapper.Map<IEnumerable<MemberShip>,IEnumerable<MembershipVM>>(memberships);
           
        }

        public MembershipVM? GetById(int id)
        {
            var membership = _uniteOfWork.GetRepository<MemberShip>()
                 .GetById(id);
                
            if (membership is null)
                return null;
            return _Mapper?.Map<MembershipVM>(membership);  
        }

        public bool Create(CreateMembershipVM createMembership)
        {
            //1: Check If Plan is Exsit and Member is Exsist
            var member=_uniteOfWork.GetRepository<Member>()
                .GetById(createMembership.MemberId);
            var plan= _uniteOfWork.GetRepository<Plan>()
                .GetById(createMembership.PlanId);
            if(plan is null || member is null)
                return false;
           
            //2: Check If Memebr has Active Members if Yes مش هينفع اعمله Plan
            //if No Create Plan
            var Activemembership=_uniteOfWork.GetRepository<MemberShip>()
                .GetAll(m => m.MemberId == createMembership.MemberId
              && m.EndDate > DateTime.Now).Any();
            if (Activemembership)
                return false;

            var mapping = _Mapper.Map<CreateMembershipVM,MemberShip>(createMembership);
            
           
            _uniteOfWork.GetRepository<MemberShip>().Add(mapping);
            return _uniteOfWork.SaveChanges()>0;
        }

        public bool Cancel(int memberId, int planId)
        {
            /*
             7.	Cancellation Delete Memberships For Member On This Plan 
             8.	A membership can only be deleted if it is Active.
            1) هات الـ Membership (MemberId + PlanId)
            2) لو مش موجود → Fail
            3) لو Expired → Fail
            4) غير كده → Delete
             */
            var repo = _uniteOfWork.GetRepository<MemberShip>();

            // 1) Get Membership
            var membership = repo
                .GetAll()
                .FirstOrDefault(m => m.MemberId == memberId && m.PlanId == planId);

            if (membership is null)
                return false;

            // 2) Check if Active
            if (membership.EndDate <= DateTime.Now)
                return false;

            // 3) Delete
            repo.Delete(membership);

            return _uniteOfWork.SaveChanges() > 0;

        }

    }
}
