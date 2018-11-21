using System;
using System.Runtime.CompilerServices;
using Assets.SQLite.Scripts.Attribute;

namespace Assets.SQLite.Scripts.Model
{
    /// <summary>
    /// 用户信息表
    /// </summary>
    public class UserInfo:ModelBase {

        [PrimaryKey]
        public int Id { get; set; }
        public Byte UserName { get; set; }

        public long PassWord { get; set; }

        public float VoiceState { get; set; }

        public string EffectVoiceState { get; set; }

        public string InfoState { get; set; }
    }
}
