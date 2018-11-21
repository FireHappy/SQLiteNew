using System;
using System.Collections.Generic;
using System.Reflection;
using Assets.SQLite.Scripts.Attribute;
using Assets.SQLite.Scripts.Base;
using Assets.SQLite.Scripts.Model;
using Mono.Data.Sqlite;
using UnityEngine;

namespace Assets.SQLite.Scripts.Repository
{
    public abstract class BaseRepository<T> where T : BaseModel
    {
        private SQLiteHelper sql;
        private readonly PropertyInfo[] propertys;//属性信息
        private readonly string[] propertyNames;//属性名,数据库字段名(一致)
        private readonly string[] columnTypes;//数据库字段名类型
        private readonly string table;//表名
        private readonly string primaryKey;//主键名
        private int primaryIndex;//主键所对应的index

        protected BaseRepository()
        {
            table = typeof(T).Name;
            propertys = typeof(T).GetProperties();
            propertyNames = getPropertyNames(propertys);
            var typeNames = getTypeNames(propertys);
            columnTypes = getColumnTypes(typeNames);
            primaryKey = getPrimaryKey();
            createTable();//初始化就创建表
        }

        /// <summary>
        /// 创建表
        /// </summary>
        private void createTable()
        {
            //创建名为User的数据表
            sql=new SQLiteHelper();
            try
            {
                sql.CreateTable(table, propertyNames, columnTypes);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
            sql.CloseConnection();            
        }

        /// <summary>
        /// 获得表的主键名
        /// </summary>
        /// <returns></returns>
        private string getPrimaryKey()
        {            
            for (int i = 0; i < propertys.Length; i++)
            {
                object[] objects= propertys[i].GetCustomAttributes(true);
                for (int j = 0; j < objects.Length; j++)
                {
                    if ((PrimaryKey)objects[i]!=null)
                    {
                        primaryIndex = i;//保存主键所对应的index
                        return propertys[i].Name;                        
                    }
                }                 
            }           
            return null;
        }

        /// <summary>
        /// 得到类型名集合
        /// </summary>
        /// <param name="propertyInfos"></param>
        /// <returns></returns>
        private string[] getTypeNames(PropertyInfo[] propertyInfos)
        {
            string[] tempStrings = new string[propertyInfos.Length];
            for (int i = 0; i < propertyInfos.Length; i++)
            {
                tempStrings[i] = propertyInfos[i].PropertyType.Name;
            }
            return tempStrings;
        }

        /// <summary>
        /// 得到属性名
        /// </summary>
        /// <returns></returns>
        private string[] getPropertyNames(PropertyInfo[] propertyInfos)
        {
            string[] tempStrings = new string[propertyInfos.Length];
            for (int i = 0; i < propertyInfos.Length; i++)
            {
                tempStrings[i] = propertyInfos[i].Name;                
            }
            return tempStrings;
        }

        /// <summary>
        /// 得到数据库字段名
        /// </summary>
        /// <returns></returns>
        private string[] getColumnTypes(string[] names)
        {
            string[] columnTemps = new string[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                //Debug.Log("属性名:"+names[i]);
                switch (names[i])
                {                
                    case "Int16":
                        columnTemps[i] = "INTEGER";
                        break;
                    case "Int32":
                        columnTemps[i] = "INTEGER";
                        break;
                    case "Int64":
                        columnTemps[i] = "INTEGER";
                        break;
                    case "String":
                        columnTemps[i] = "TEXT";
                        break;                   
                    case "Single":
                        columnTemps[i] = "REAL";
                        break;
                    case "Byte":
                        columnTemps[i] = "INTEGER";
                        break;
                    case "Double":
                        columnTemps[i] = "REAL";
                        break;                   
                    default:
                        Debug.LogWarning("Respository未加入该类型的转换:"+names[i]);
                        break;
                }
            }
            return columnTemps;
        }

        /// <summary>
        /// 插入数据信息
        /// </summary>
        /// <param name="modelList"></param>
        public void InsertList(List<T> modelList)
        {
            sql = new SQLiteHelper();
            try
            {
                for (int i = 0; i < modelList.Count; i++)
                {
                    //插入数据信息
                    sql.InsertValues(table, getValues(modelList[i]));
                }   
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }                             
            sql.CloseConnection();
        }

        private string[] getValues(T model)
        {
            string[] values = new string[propertys.Length];
            for (int i = 0; i < propertys.Length; i++)
            {
                values[i] = "'" + propertys[i].GetValue(model, null) + "'";
            }
            return values;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="model"></param>
        public void Insert(T model)
        {
            sql = new SQLiteHelper();
            try
            {              
                sql.InsertValues(table, getValues(model));
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
            sql.CloseConnection();
        }

        /// <summary>
        /// 通过所给的字段名删除用户信息
        /// </summary>
        /// <param name="value"></param>        
        public void DeleteByPrimaryKey(object value)
        {
            sql = new SQLiteHelper();
            try
            {
                sql.DeleteValuesAND(table, new string[] { propertyNames[primaryIndex] }, new string[] { "=" }, new string[] { string.Format("'{0}'", value) });
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }          
            sql.CloseConnection();
        }

        public void DeleteAll()
        {
            sql=new SQLiteHelper();
            try
            {
                sql.ExecuteQuery("DELETE FROM " + table);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }                  
            sql.CloseConnection();
        }

        /// <summary>
        /// 根据id更新用户信息
        /// </summary>
        /// <param name="model"></param>
        public void UpdateByPrimaryKey(T model)         
        {
            sql = new SQLiteHelper();
            string[] values = new string[propertys.Length];
            for (int i = 0; i < propertys.Length; i++)
            {
                values[i] = "'" + propertys[i].GetValue(model, null) + "'";
            }
            sql.UpdateValues(table, propertyNames, values, primaryKey, getPrimaryKeyValue(model));
            sql.CloseConnection();
        }

        /// <summary>
        /// 得到主键的值
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string getPrimaryKeyValue(T model)
        {
            return "'"+Convert.ToString(propertys[primaryIndex].GetValue(model, null))+"'";
        }

        /// <summary>
        /// 通过id筛选数据,查询有点问题
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Dictionary<string,object> SelectByPrimaryKey(object value)
        {          
            Dictionary<string,object>map=new Dictionary<string, object>();
            sql = new SQLiteHelper();
            SqliteDataReader reader = sql.ReadTable(table,
               propertyNames,new[] { primaryKey }, new[] { "=" }, new[] { value.ToString() });
            while (reader.Read())
            {
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    map.Add(propertyNames[i],reader.GetOrdinal(propertyNames[i]));
                }              
            }
            sql.CloseConnection();
            return map;
        }             
    }
}