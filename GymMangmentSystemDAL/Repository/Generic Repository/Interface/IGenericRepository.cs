using GymMangmentSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Repository.Generic_Repository.Interface
{
    //دا بيتعامل مع اى Entity اتحولت الى Table in Database
    //حطيت new ليه as BaseEntityt+Gymuser متحولوش ولو حطيتها من غير New كان الContrsin هيتطبق عليهم 
    //new معناها عنده Construcotr ولان Abstract class not has Constraucotr 
    //يبقى انا عايز اعمل Constrain on any class will be converted to table so make inheritance from Base entityt 
    //طب دا هينطبق برضو على Baseentity+Gymuser اللى هما اصلا abtrsct class 
    //عشان كدة نعمل Constrain كمان new معناها انا عايز اى Class will be implement interface Baseentity وكمان يكون عنده Constrauctur ولان abstract class Not has Constrauctur 
    public interface IGenericRepository<T> where T:BaseEntity,new()
    {
        int Add(T entity);
        int Update(T entity);
        int Delete(T entity);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(Func<T,bool>? Condition);//To Take Func To Fiter Data
        T? GetById(int id);
    }
}
