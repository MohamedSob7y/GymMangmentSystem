using GymMangmentSystemDAL.Entities;
using GymMangmentSystemDAL.Repository.Generic_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Unit_Of_Work.Interface
{
    public interface IUniteOfWork
    {
        //Has Only Two Method
        //1:GetRepository   2: Save Changes
        //تعالج مشكلة انى كل شوية عشان اعمل Query اروح اكلم IGeneric Repository of Any Table عملى شكل كبير على الCode 
        #region Old Implemntation 
        //public IGenericRepository<Member> MemberRepo { set;  }
        //public IGenericRepository<Session> SessionRepo { set;  }
        //كدة مش Generic خالص وحاجة صعبة 

        #endregion
        //========================================
        #region New Implementation
        //نفس Constrain InGEneric Repository 
        IGenericRepository<T> GetRepository<T>()where T:BaseEntity,new();
        public int SaveChanges(); 
        //عشان اعمل Save Changes مرة واحدة فقط
        //كدة لما انادى على اى Genric Repository يعمل Save changes local 
        //ولما اخلص خالص بنادى على  Save Changes الكبيرة 
        #endregion

    }
}
