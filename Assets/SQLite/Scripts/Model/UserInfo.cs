namespace Assets.SQLite.Scripts.Model
{
    /// <summary>
    /// 用户信息表
    /// </summary>
    public class UserInfo:ModelBase {

        public int Id { get; set; }

        public string UserName { get; set; }

        public string PassWord { get; set; }

        public string VoiceState { get; set; }

        public string EffectVoiceState { get; set; }

        public string InfoState { get; set; }
    }
}
