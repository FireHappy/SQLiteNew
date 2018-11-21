using System;
using System.Collections.Generic;
using Assets.SQLite.Scripts.Model;
using Assets.SQLite.Scripts.Singleton;
using Mono.Data.Sqlite;
using UnityEngine;

namespace Assets.SQLite.Scripts.Manager
{
    public class UserSQLiteManager : Singleton<UserSQLiteManager>,SQLiteManager<UserInfo>
    {

        private SQLiteHelper sql;
        public void InsertList(List<UserInfo> modelList)
        {
            //throw new NotImplementedException();
            sql = new SQLiteHelper();
            try
            {
                //创建名为User的数据表
                sql.CreateTable("User", new[] { "UserId", "UserName", "PassWord", "VoiceState", "EffectVoiceState", "InfoState" },
                    new[] { "interger PRIMARY KEY", "text", "text", "text", "text", "text" });
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
            for (int i = 0; i < modelList.Count; i++)
            {                
                string id = "'" + modelList[i].Id + "'";
                string name = "'" + modelList[i].UserName + "'";
                string word = "'" + modelList[i].PassWord + "'";
                string voice = "'" + modelList[i].VoiceState + "'";
                string effectVoice = "'" + modelList[i].EffectVoiceState + "'";
                string info = "'" + modelList[i].InfoState + "'";
                sql.InsertValues("User", new[] { id, name, word, voice, effectVoice, info });
            }
            sql.CloseConnection();
        }

        public void Insert(UserInfo model)
        {
            sql = new SQLiteHelper();
            try
            {
                //创建名为User的数据表
                sql.CreateTable("User", new[] { "UserId", "UserName", "PassWord", "VoiceState", "EffectVoiceState", "InfoState" },
                    new[] { "interger PRIMARY KEY", "text", "text", "text", "text", "text" });
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
            string id = "'" + model.Id + "'";
            string name = "'" + model.UserName + "'";
            string word = "'" + model.PassWord + "'";
            string voice = "'" + model.VoiceState + "'";
            string effectVoice = "'" + model.EffectVoiceState + "'";
            string info = "'" + model.InfoState + "'";
            //插入数据
            sql.InsertValues("User", new[] { id, name, word, voice, effectVoice, info });
            sql.CloseConnection();
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        public void DeleteById(int id)
        {
            sql = new SQLiteHelper();
            sql.DeleteValuesAND("User", new string[] { "UserId" }, new string[] { "=" }, new string[] { string.Format("'{0}'", id) });
            sql.CloseConnection();
        }

        /// <summary>
        /// 更新用户的信息
        /// </summary>
        public void Update(UserInfo model)
        {
            sql = new SQLiteHelper();
            string[] colValues = new string[]
            {
                string.Format("'{0}'", model.UserName), 
                string.Format("'{0}'", model.PassWord),
                string.Format("'{0}'", model.VoiceState), 
                string.Format("'{0}'", model.EffectVoiceState),
                string.Format("'{0}'", model.InfoState)
            };
            sql = new SQLiteHelper();
            sql.UpdateValues("User", new [] { "UserName", "PassWord", "VoiceState", "EffectVoiceState", "InfoState" }, colValues, "UserId", "1");
            sql.CloseConnection();
        }


        /// <summary>
        /// 通过用户id查询信息
        /// </summary>
        /// <param name="id"></param>
        public UserInfo SelectById(int id)
        {
            UserInfo info = new UserInfo();
            sql=new SQLiteHelper();            
            SqliteDataReader reader = sql.ReadTable("User",
                new[] {"UserId", "UserName", "PassWord", "VoiceState", "EffectVoiceState", "InfoState"},
                new[] {"UserId"}, new[] {"="}, new[] {id.ToString()});
            while (reader.Read())
            {
                info.Id = reader.GetInt32(reader.GetOrdinal("UserId"));
                info.UserName = reader.GetString(reader.GetOrdinal("UserName"));
                info.PassWord = reader.GetString(reader.GetOrdinal("PassWord"));
                info.VoiceState = reader.GetString(reader.GetOrdinal("VoiceState"));
                info.EffectVoiceState = reader.GetString(reader.GetOrdinal("EffectVoiceState"));
                info.InfoState = reader.GetString(reader.GetOrdinal("InfoState"));
            }
            return info;
        }

        public List<UserInfo> SelectByIds(List<int> ids)
        {
            //throw new NotImplementedException();
            return null;
        }
    }
}
