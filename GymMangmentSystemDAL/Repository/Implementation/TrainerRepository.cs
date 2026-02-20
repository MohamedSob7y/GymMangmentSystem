using GymMangmentSystemDAL.Data.Context;
using GymMangmentSystemDAL.Entities;
using GymMangmentSystemDAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Repository.Implementation
{
    public class TrainerRepository : ITrainerRepository
    {

        #region Problem With Open Connection with DbContext
        //private readonly GymDbContext _gymDbContext = new GymDbContext();
        //طالما عملت Object To Open Connection with Database مش هعمل اى تعديل عليها عشان كدة هعملها Private / Readonly
        //هنا عملته Manual واا مش عاريز كدة عشان عملى مشكلة Dependency injection
        #endregion
        //===================================================
        #region Solving Problem With Dependency injection
        private readonly GymDbContext _gymDbContext;
        public TrainerRepository(GymDbContext gymDbContext)
        {
            _gymDbContext = gymDbContext;
        }
        #endregion
        //===================================================
        public int Add(Trainer trainer)
        {
            _gymDbContext.Add(trainer);
            return _gymDbContext.SaveChanges();
        }
        public int Delete(int id)
        {
            var trainer = _gymDbContext.Trainers.Find(id);
            if (trainer != null) return 0;
            _gymDbContext.Trainers.Remove(trainer);
            return _gymDbContext.SaveChanges();
        }

        public IEnumerable<Trainer> GetAllTrainers() => _gymDbContext.Trainers.ToList();
       

        public Trainer? GetbyId(int id) => _gymDbContext.Trainers.Find(id);
        public int Update(Trainer trainer)
        {
            _gymDbContext.Trainers.Update(trainer);
            return _gymDbContext.SaveChanges();
        }
    }
}
