using System.Collections.Generic;
using Assets.SQLite.Scripts.Model;

namespace Assets.SQLite.Scripts.Manager
{
    public interface SQLiteManager<T> where T : ModelBase
    {        
        void InsertList(List<T> modelList);
        void Insert(T model);
        void DeleteById(int id);
        void Update(T model);
        T SelectById(int id);
        List<T> SelectByIds(List<int> ids);
    }
}