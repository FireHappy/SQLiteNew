using System.Collections.Generic;
using System.IO;
using Assets.SQLite.Scripts.Config;
using Assets.SQLite.Scripts.Model;
using Assets.SQLite.Scripts.Repository;
using Assets.SQLite.Scripts.Singleton;
using UnityEngine;

namespace Assets.SQLite.Scripts.Manager
{
    /// <summary>
    /// SQLite管理类
    /// </summary>
    public class SQLiteManager:SingleMono<SQLiteManager>
    {

        private readonly Dictionary<string,object>repositories=new Dictionary<string, object>();


        /// <summary>
        /// 初始化配置文件
        /// </summary>
        private void InitConfig()
        {
            if (!Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath);
            }
            DBConfig.PC_CONNECT_STRING = "data source=" + Application.streamingAssetsPath + "/zps_db.sqlite";
            DBConfig.ANDROID_CONNECT_STRING = "URI=file:" + Application.persistentDataPath + "/zps_db.sqlite";
            DBConfig.IOS_CONNECT_STRING = "data source=" + Application.persistentDataPath + "/zps_db.sqlite";
        }

        void Awake()    
        {
            InitConfig();
            AddResository(typeof(User).Name,new UserRepository());            
        }

        public void AddResository(string key, object value)
        {
            repositories.Add(key,value);
        }

        public BaseRepository<T> GetRepository<T>() where T : BaseModel
        {
            object respoitory = null;
            if (repositories.TryGetValue(typeof (T).Name, out respoitory))
            {
                return (BaseRepository<T>) respoitory;
            }
            return default(BaseRepository<T>);
        }
    }
}