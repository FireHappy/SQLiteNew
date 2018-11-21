using System.Collections.Generic;
using Assets.SQLite.Scripts.Manager;
using Assets.SQLite.Scripts.Model;
using Assets.SQLite.Scripts.Repository;
using UnityEngine;

namespace Assets.SQLite.Scripts
{
    /// <summary>
    /// SQLite的demo
    /// </summary>
    public class SQLiteTest : MonoBehaviour
    {
        private BaseRepository<User> repository = null;

        private void Awake()
        {
            repository = SQLiteManager.Instance.GetRepository<User>();
        }

        private void Start()
        {           
            //DeleteAll();
            //InsertTest();
            //InsertList();
            //UpdateTest();
            //DeleteTest();
            //SelectTest(); 
            SelectListTest();
        }

        private void DeleteAll()
        {
            repository.DeleteAll();
        }

        private void InsertTest()
        {
            User user = new User
            {
                Id = 1,
                UserName = "test",
                Age = 10,
                PassWord = 123456789
            };     
            repository.Insert(user);
        }


        private void InsertList()
        {
            for (int i = 0; i < 10; i++)
            {
                User user = new User
                {
                    Id = i,
                    UserName = null,
                    Age = 10,
                    PassWord = 123456789
                };
                repository.Insert(user);
            }
        }

       

        private void DeleteTest()
        {
            repository.DeleteByPrimaryKey(1);
        }

        private void UpdateTest()
        {
            User user = new User
            {
                Id = 1,
                UserName = "test--update" ,
                Age = 10,
                PassWord = 123456789
            };
            repository.UpdateByPrimaryKey(user);
        }

        private void SelectTest()
        {
            Dictionary<string, object> user = repository.SelectByPrimaryKey(2);            
        }

        private void SelectListTest()
        {
            List<Dictionary<string, object>> userList = repository.SelectListByColumn("10", "Age");
            Debug.Log("user.count:"+userList.Count);
        }
    }
}
