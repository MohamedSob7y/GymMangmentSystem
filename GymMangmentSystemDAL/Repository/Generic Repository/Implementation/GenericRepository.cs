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
        public int Add(T entity)
        {
            _gymDbContext.Set<T>().Add(entity);
            return _gymDbContext.SaveChanges();
        }

        public int Delete(T entity)
        {
            _gymDbContext.Set<T>().Remove(entity);
            return _gymDbContext.SaveChanges();
        }

        public IEnumerable<T> GetAll() => _gymDbContext.Set<T>().ToList();

        public IEnumerable<T> GetAll(Func<T, bool>? Condition)
        {
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
        }

        public T? GetById(int id)=> _gymDbContext.Set<T>().Find(id);


        public int Update(T entity)
        {
            _gymDbContext.Set<T>().Update(entity);
            return _gymDbContext.SaveChanges();
        }
    }
}
