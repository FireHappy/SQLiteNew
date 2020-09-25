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
        public string dbName = "lin_db";

        private readonly Dictionary<string,object>repositories=new Dictionary<string, object>();


        /// <summary>
        /// 初始化配置文件
        /// </summary>
        private void InitConfig()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor|| Application.platform == RuntimePlatform.WindowsPlayer)
            {
                if (!Directory.Exists(Application.streamingAssetsPath))
                {
                    Debug.Log("+++++++++++++++Init Application streamingAssetsPath++++++++++++++++");
                    Directory.CreateDirectory(Application.streamingAssetsPath);
                }
            }            
            DBConfig.PC_CONNECT_STRING = "data source=" + Application.streamingAssetsPath +string.Format("/{0}.sqlite",dbName);
            DBConfig.ANDROID_CONNECT_STRING = "URI=file:" + Application.persistentDataPath + string.Format("/{0}.sqlite", dbName);
            DBConfig.IOS_CONNECT_STRING = "data source=" + Application.persistentDataPath + string.Format("/{0}.sqlite", dbName);
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