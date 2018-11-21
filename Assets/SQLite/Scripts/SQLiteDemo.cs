using System.Collections.Generic;
using Assets.SQLite.Scripts.Manager;
using Assets.SQLite.Scripts.Model;
using UnityEngine;

namespace Assets.SQLite.Scripts
{
    /// <summary>
    /// SQLite的demo
    /// </summary>
    public class SQLiteDemo : MonoBehaviour
    {
        private void Start()
        {
            InsertTest();
            UpdateTest();
            DeleteTest();
            SelectTest();            
        }

        private void InsertList()
        {
            List<UserInfo>userInfos=new List<UserInfo>();
            for (int i = 0; i < 10; i++)
            {
                UserInfo user = new UserInfo
                {
                    Id = i+1,
                    EffectVoiceState = "test",
                    InfoState = "test",
                    PassWord = "test",
                    UserName = "test",
                    VoiceState = "test"
                };
                userInfos.Add(user);
            }
            UserSQLiteManager.Instance().InsertList(userInfos);
        }

        private void InsertTest()
        {
            //插入数据测试
            UserInfo user = new UserInfo
            {
                Id = 1,
                EffectVoiceState = "test",
                InfoState = "test",
                PassWord = "test",
                UserName = "test",
                VoiceState = "test"
            };
            UserSQLiteManager.Instance().Insert(user);             
        }

        private void DeleteTest()
        {
            UserSQLiteManager.Instance().DeleteById(1);
        }

        private void UpdateTest()
        {
            UserInfo user = new UserInfo
            {
                Id = 1,
                EffectVoiceState = "testUpdate",
                InfoState = "testUpdate",
                PassWord = "testUpdate",
                UserName = "testUpdate",
                VoiceState = "testUpdate"
            };
            UserSQLiteManager.Instance().Update(user);
        }

        private void SelectTest()
        {
            UserInfo user= UserSQLiteManager.Instance().SelectById(1);
            Debug.Log("user.username:"+user.UserName);
        }
    }
}
