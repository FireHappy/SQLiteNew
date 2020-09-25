using Assets.SQLite.Scripts.Attribute;

namespace Assets.SQLite.Scripts.Model
{
    /// <summary>
    /// 用户信息表
    /// </summary>
    public class User:BaseModel {

        [PrimaryKey]
        public int Id { get; set; }
        public string UserName { get; set; }
        public long PassWord { get; set; }
        public int Age { get; set; }
    }
}
