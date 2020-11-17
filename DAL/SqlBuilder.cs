/************************************************************************************
* Copyright (c) 2020 All Rights Reserved.
*命名空间：ORMProject.DAL
*文件名： SqlBuilder
*创建人： yedong Chen
*创建时间：2020/11/15 19:16:59
*描述
*=====================================================================
*修改标记
*修改时间：2020/11/15 19:16:59
*修改人：yedong Chen
*描述：
************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORMProject.Framework.MappingAttribute;

namespace ORMProject.DAL
{
    /// <summary>
    /// 缓存从Framework中拼接出来的SQL语句
    /// </summary>
    public class SqlBuilder<T>
    {
        private static string _selectSql;
        private static string _InsertSql;
        static SqlBuilder()
        {
            Type type = typeof(T);  //获取当前实体对象的数据类型

            {
                //var columns = string.Join(",", type.GetProperties().Select(p => $"[{p.Name}]"));    //获取反射所得出的类的所有的列，并将它们的的列名加上[]
                ////version 1.0
                //var columns = string.Join(",", type.GetProperties().Select(p => $"[{p.GetColumnsName()}]"));  
                //version 2.0
                var columns = string.Join(",", type.GetProperties().Select(p => $"[{p.GetName()}]"));

                //string sql = $"Select {columns} From [{type.GetTableName()}] Where Id='{Id}'";  //拼接连接字符串

                //version 2.0
                _selectSql = $"Select {columns} From [{type.GetName()}] Where Id=";  //拼接连接字符串
            }

            {
                string columnsString = string.Join(",", type.GetProperties().Select(p => $"[{p.GetName()}]"));  //获取列名
                string valueString = string.Join(",", type.GetProperties().Select(p => $"@{p.GetName()}")); //拼接字符串，以参数形式展现@Name、@Introduction          
                //version 1.0
                //string columnsString = string.Join(",", type.GetPropertiesWithoutKey().Select(p => $"[{p.GetName()}]"));  //获取列名
                //string valueString = string.Join(",", type.GetPropertiesWithoutKey().Select(p => $"@{p.GetName()}")); //拼接字符串，以参数形式展现@Name、@Introduction          

               _InsertSql = $"Insert into [{type.GetName()}] ({columnsString}) Values ({valueString})";    //这个地方不能直接拼接字符串，防止SQL注入
            }
        }

        /// <summary>
        ///获取以Id为参数的查询语句
        /// </summary>
        /// <returns></returns>
        public static string GetSelectSql()
        {
            return _selectSql;
        }

        /// <summary>
        ///获取以泛型形式插入数据库表中数据的SQL语句
        /// </summary>
        /// <returns></returns>
        public static string GetInsertSql()
        {
            return _InsertSql;
        }
    }
}
