using UnityEngine;

namespace Assets.SQLite.Scripts.Config
{
    public class DBConfig
    {       
        public static string PC_CONNECT_STRING = "data source=" + Application.dataPath + "/zps_db.sqlite";
        public static string ANDROID_CONNECT_STRING = "URI=file:" + Application.persistentDataPath + "/zps_db.sqlite";
        public static string IOS_CONNECT_STRING = "data source=" + Application.persistentDataPath + "/zps_db.sqlite";
    }
}
