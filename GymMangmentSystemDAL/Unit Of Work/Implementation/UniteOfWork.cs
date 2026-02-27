using GymMangmentSystemDAL.Data.Context;
using GymMangmentSystemDAL.Entities;
using GymMangmentSystemDAL.Repository.Generic_Repository.Implementation;
using GymMangmentSystemDAL.Repository.Generic_Repository.Interface;
using GymMangmentSystemDAL.Unit_Of_Work.Interface;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Unit_Of_Work.Implementation
{
    public class UniteOfWork : IUniteOfWork
    {

        #region Old Implementation
        //كدة دى مشكلة انك فرضت عليا كل دا انا عايز لما اطلب منك النوع تديهونى 
        //private readonly GymDbContext _gymDbContext;
        //public UniteOfWork(GymDbContext gymDbContext)
        //{
        //    _gymDbContext = gymDbContext;
        //}
        //public IGenericRepository<Member> MemberRepo { set => new GenericRepository<Member>(_gymDbContext); }
        //public IGenericRepository<Session> SessionRepo { set => new GenericRepository<Session>(_gymDbContext); }

        #endregion
        //================================
        #region New Implementation
        private readonly Dictionary<Type,object> _Repository = new Dictionary<Type,object>();//Enhance ممكن اشيل اللى بعد New
        //Key is Type  like <Member>
        //Value => New Generic Repository اللى هو object  => Values From Type Object
        private readonly GymDbContext _gymDbContext;
        //انا معلم CLr To inject object from DbContext
        public UniteOfWork(GymDbContext gymDbContext)
        {
            _gymDbContext = gymDbContext;
        }
        public IGenericRepository<T> GetRepository<T>() where T : BaseEntity, new()
        {
            #region Way 01
            //return new GenericRepository<T>(_gymDbContext); 
            //دى بتسبب مشكلة انى لو عملت مرتين فى نفس ال service IGeneric Reposityr<Member> هيعملى Two Object in Heap مع انه نفس الObject المفروض هو هو لاء انا كل مانادى عليها يتعمل object 
            #endregion
            //=========================================
            #region Way With Optimized Performance using Dictionary
            var entitytype = typeof(T);//Get Name Of Entity From Namespace like Member
             
            //First 
            //if(_Repository.ContainsKey(entitytype))
            //{
            //    //معنى كدة انه موجود اصلا 
            //    return (IGenericRepository<T>)_Repository[entitytype];
            //}

            //Second=>Try Get Value  لو لاقيته حطه فى Variable دا ولو لاء خلاص 
            if(_Repository.TryGetValue(entitytype,out var Repository))
            {
                return (IGenericRepository<T>)_Repository[entitytype];
            }
            //طب فى حاة انه مش موجود 
            var newRepo=new GenericRepository<T>(_gymDbContext);//لو مش موجود اعمله 
            _Repository[entitytype]=newRepo;    //Save This object in Dictionary عشان لو عايزه تانى يجيلى ويكون متخرن على طول 
            return newRepo;
            #endregion
        }

        public int SaveChanges()
        {
           return _gymDbContext.SaveChanges();
            //لازم بقا اشيل Savechanges from Generic Repo عشان الChanges عايزها يتم من دى مش من هنالك هنالك هيتم Local 
        }

        #endregion

    }
}
