using GymMangmentSystemDAL.Data.Context;
using GymMangmentSystemDAL.Entities;
using GymMangmentSystemDAL.Repository.Generic_Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Repository.Generic_Repository.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
    {
        private readonly GymDbContext _gymDbContext;

        public GenericRepository(GymDbContext gymDbContext)
        {
            _gymDbContext = gymDbContext;
        }
        //==================================================
        #region Before UniteOfWOrk
        //public int Add(T entity)
        //{
        //    _gymDbContext.Set<T>().Add(entity);
        //    return _gymDbContext.SaveChanges();
        //}

        //public int Delete(T entity)
        //{
        //    _gymDbContext.Set<T>().Remove(entity);
        //    return _gymDbContext.SaveChanges();
        //}

        //public IEnumerable<T> GetAll() => _gymDbContext.Set<T>().ToList();

        //public IEnumerable<T> GetAll(Func<T, bool>? Condition = null)
        //{
        //    if (Condition is null)
        //    {
        //        return _gymDbContext.Set<T>().AsNoTracking()
        //            .ToList();
        //    }
        //    else
        //    {
        //        return _gymDbContext.Set<T>().AsNoTracking().Where(Condition)
        //            .ToList();
        //        //Use .AsNoTracking() عشان مش محتاج اى حاجة فى Database انا Get From Dstabase عشان كدة مش محتاج Keep Tracking 
        //        //AsNoTracking() استخدمها عشان لو هعمل اى حاجة على Database like Create Update Delete كدة لازم اعمل tracking 
        //    }
        //}

        //public T? GetById(int id) => _gymDbContext.Set<T>().Find(id);


        //public int Update(T entity)
        //{
        //    _gymDbContext.Set<T>().Update(entity);
        //    return _gymDbContext.SaveChanges();
        //}
        #endregion
        //==================================================
        #region Refactor After UnitOfWork
        //Remove Save changes لان هعلمها من Unite ofWork مش من هنا 
        //هجمع كل Operations مرة واحدة واروح اكلم الDatabase مرة واحدة عشان Save Chnages
        //Add + Update + Delete بتتم Local 
        //After Call Save change in UnitOfWork كل الOperations الل حصلت Local هتتعمل بقا فى database 
        public void Add(T entity)
        {
             _gymDbContext.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _gymDbContext.Set<T>().Remove(entity);
          
        }

        public IEnumerable<T> GetAll() => _gymDbContext.Set<T>().ToList();

        //محتاج اعدل على GetAll عشان كل مرة عايز اجيب session must get TrainerName+CategoryName
        public IEnumerable<T> GetAll(Func<T, bool>? Condition = null)
        {
            //====================================================
            #region After Igger Loading
            //كل مرة بجيب فيها الsession => Get TrainerName+CategoryName
            //if (typeof(T) is Session)
            //{
            //    return (IEnumerable<T>)_gymDbContext.Sessions.Include(T => T.Category).Include(T => T.Trainer).ToList();
            //}
            //if (Condition is null)
            //{

            //    //لو عملتها بالطريقة دى عشان اشتغل على Igger Loading > مش هينفع لانى لازم احدد Type of iclude وانا اصلا عاملها Generic
            //    //return _gymDbContext.Set<T>().Include().AsNoTracking()
            //    //   .ToList();
            //    return _gymDbContext.Set<T>().AsNoTracking()
            //        .ToList();
            //}
            //==========================================================
            //right Answers
             //Make Specicif Repository=> ISessionRepository هى بقا اللى بتحتوى على Logic الجديد بتاع GetAll بس مش هينفع اعملها فى Gendeneric لانه هيكون عنده logic ومش هينفع 

            //==========================================================
            #endregion
            //====================================================
            #region Before Igger Loading
            //دا حل مش كويس ومش احسن حاجة 

            if (Condition is null)
            {
                return _gymDbContext.Set<T>().AsNoTracking()
                    .ToList();
            }
            else
            {
                return _gymDbContext.Set<T>().AsNoTracking().Where(Condition)
                    .ToList();
                //Use .AsNoTracking() عشان مش محتاج اى حاجة فى Database انا Get From Dstabase عشان كدة مش محتاج Keep Tracking 
                //AsNoTracking() استخدمها عشان لو هعمل اى حاجة على Database like Create Update Delete كدة لازم اعمل tracking 
            } 
            #endregion
        }

        public T? GetById(int id) => _gymDbContext.Set<T>().Find(id);


        public void Update(T entity)
        {
            _gymDbContext.Set<T>().Update(entity);
            
        }


        #endregion
    }
}
