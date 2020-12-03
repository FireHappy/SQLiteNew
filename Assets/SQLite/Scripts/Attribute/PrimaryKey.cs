using System;

namespace Assets.SQLite.Scripts.Attribute
{
    /// <summary>
    /// 用户对主键的标志
    /// </summary>
    [AttributeUsage(AttributeTargets.Property,AllowMultiple = false)]
    public class PrimaryKey:System.Attribute
    {
         
    }
}