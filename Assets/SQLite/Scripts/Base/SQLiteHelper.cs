﻿using System;
using Assets.SQLite.Scripts.Config;
using Mono.Data.Sqlite;
using UnityEngine;

namespace Assets.SQLite.Scripts.Base
{
    /// <summary>
    /// 功能：Sqlite嵌入式数据库的操作类
    /// </summary>
    public class SQLiteHelper
    {
        /// <summary>
        /// 数据库连接定义
        /// </summary>
        public SqliteConnection dbConnection;

        /// <summary>
        /// SQL命令定义
        /// </summary>
        public SqliteCommand dbCommand;

        /// <summary>
        /// 数据读取定义
        /// </summary>
        public SqliteDataReader dataReader;

        /// <summary>
        /// 构造函数    
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        public SQLiteHelper(string connectionString)
        {
            try
            {
                //构造数据库连接
                dbConnection = new SqliteConnection(connectionString);
                //打开数据库
                dbConnection.Open();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }            
        }

        /// <summary>
        /// 无参的构造函数
        /// </summary>
        public SQLiteHelper()
        {
            try
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    //构造数据库连接
                    dbConnection = new SqliteConnection(DBConfig.ANDROID_CONNECT_STRING);
                }
                else if(Application.platform==RuntimePlatform.WindowsPlayer)
                {
                    //构造数据库连接
                    dbConnection = new SqliteConnection(DBConfig.PC_CONNECT_STRING);
                }
                else if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    //构造数据库连接
                    dbConnection = new SqliteConnection(DBConfig.PC_CONNECT_STRING);
                }
                else 
                {
                    dbConnection=new SqliteConnection(DBConfig.ANDROID_CONNECT_STRING);
                }
                //打开数据库
                dbConnection.Open();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }    
        }

        

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void CloseConnection()
        {
            //销毁Command
            if (dbCommand != null)
            {
                dbCommand.Cancel();
                dbCommand.Dispose();
            }

            //销毁Reader
            if (dataReader != null)
            {
                dataReader.Close();     
                dataReader.Dispose();
            }

            //销毁Connection
            if (dbConnection != null)
            {
                dbConnection.Close();  
                dbConnection.Dispose();               
            }
            SqliteConnection.ClearAllPools();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// 执行SQL命令
        /// </summary>
        /// <returns>The query.</returns>
        /// <param name="queryString">SQL命令字符串</param>
        public SqliteDataReader ExecuteQuery(string queryString)
        {
            Debug.Log("执行的SQL语句是:" + queryString);
            dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = queryString;
            dataReader = dbCommand.ExecuteReader();
            return dataReader;
        }

        /// <summary>
        /// 读取整张数据表
        /// </summary>
        /// <returns>The full table.</returns>
        /// <param name="tableName">数据表名称</param>
        public SqliteDataReader ReadFullTable(string tableName)
        {
            string queryString = "SELECT * FROM " + tableName;
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// 向指定数据表中插入数据
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="tableName">数据表名称</param>
        /// <param name="values">插入的数值</param>
        public SqliteDataReader InsertValues(string tableName, string[] values)
        {
           
            string queryString = "INSERT INTO " + tableName + " VALUES (" + values[0];
            for (int i = 1; i < values.Length; i++)
            {
                queryString += "," + values[i];
            }
            queryString += " )";
            return ExecuteQuery(queryString);
        }


        /// <summary>
        /// 更新指定数据表内的数据
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="tableName">数据表名称</param>
        /// <param name="colNames">字段名</param>
        /// <param name="colValues">字段名对应的数据</param>
        /// <param name="selectkey">关键字</param>
        /// <param name="selectValue">关键字对应的值</param>
        public SqliteDataReader UpdateValues(string tableName, string[] colNames, string[] colValues, string selectkey, string selectValue)
        {
            //当字段名称和字段数值不对应时引发异常
            if (colNames.Length != colValues.Length)
            {
                throw new SqliteException("colNames.Length!=colValues.Length");
            }
            string queryString = "UPDATE " + tableName + " SET " + colNames[0] + "=" + colValues[0];
            for (int i = 1; i < colValues.Length; i++)
            {
                queryString += ", " + colNames[i] + "=" + colValues[i];
            }
            queryString += " WHERE " + selectkey + "=" + selectValue;//SQl 语句    
            return ExecuteQuery(queryString);
        }


        /// <summary>
        /// 删除指定数据表内的数据
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="tableName">数据表名称</param>
        /// <param name="colNames">字段名</param>
        /// <param name="colValues">字段名对应的数据</param>
        public SqliteDataReader DeleteValuesOR(string tableName, string[] colNames, string[] operations, string[] colValues)
        {
            //当字段名称和字段数值不对应时引发异常
            if (colNames.Length != colValues.Length || operations.Length != colNames.Length || operations.Length != colValues.Length)
            {
                throw new SqliteException("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");
            }

            string queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + colValues[0];
            for (int i = 1; i < colValues.Length; i++)
            {
                queryString += "OR " + colNames[i] + operations[0] + colValues[i];
            }
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// 删除指定数据表内的数据
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="tableName">数据表名称</param>
        /// <param name="colNames">字段名</param>
        /// <param name="operations">操作</param>
        /// <param name="colValues">字段名对应的数据</param>
        public SqliteDataReader DeleteValuesAND(string tableName, string[] colNames, string[] operations, string[] colValues)
        {
            //当字段名称和字段数值不对应时引发异常
            if (colNames.Length != colValues.Length || operations.Length != colNames.Length || operations.Length != colValues.Length)
            {
                throw new SqliteException("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");
            }

            string queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + colValues[0];
            for (int i = 1; i < colValues.Length; i++)
            {
                queryString += " AND " + colNames[i] + operations[i] + colValues[i];
            }
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// 创建数据表
        /// </summary> +
        /// <returns>The table.</returns>
        /// <param name="tableName">数据表名</param>
        /// <param name="colNames">字段名</param>
        /// <param name="colTypes">字段名类型</param>
        public SqliteDataReader CreateTable(string tableName, string[] colNames, string[] colTypes)
        {
            string queryString = "CREATE TABLE IF NOT EXISTS " + tableName + "( " + colNames[0] + " " + colTypes[0];
            for (int i = 1; i < colNames.Length; i++)
            {
                queryString += ", " + colNames[i] + " " + colTypes[i];
            }
            queryString += " ) ";
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// Reads the table.
        /// </summary>
        /// <returns>The table.</returns>
        /// <param name="tableName">Table name.</param>
        /// <param name="items">Items.</param>
        /// <param name="colNames">Col names.</param>
        /// <param name="operations">Operations.</param>
        /// <param name="colValues">Col values.</param>
        public SqliteDataReader ReadTable(string tableName, string[] items, string[] colNames, string[] operations, string[] colValues)
        {
            string queryString = "SELECT " + items[0];
            for (int i = 1; i < items.Length; i++)
            {
                queryString += ", " + items[i];
            }
            queryString += " FROM " + tableName + " WHERE " + colNames[0] + " " + operations[0] + " " + colValues[0];
            for (int i = 1; i < colNames.Length; i++)
            {
                queryString += " AND " + colNames[i] + " " + operations[i] + " " + colValues[0] + " ";
            }
            return ExecuteQuery(queryString);
        }
    }   
}