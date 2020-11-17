using System;
using System.Data.SqlClient;
using System.Linq;
using ORMProject.Framework;
using ORMProject.Framework.MappingAttribute;
using ORMProject.Model;

namespace ORMProject.DAL
{
    /// <summary>
    /// 数据库查询关键类
    /// </summary>
    public class SqlHelper
    {
        public T FindDataList<T>(string Id) where T : BaseModel     //因为我们在SQL语句中的约束条件是id.所以我们需要对类进行约束
        {
            #region 以反射的方式，每一次调用都通过反射获取当前的数据库表名和列名
            //Type type = typeof(T);  //获取当前实体对象的数据类型

            ////var columns = string.Join(",", type.GetProperties().Select(p => $"[{p.Name}]"));    //获取反射所得出的类的所有的列，并将它们的的列名加上[]
            //////version 1.0
            ////var columns = string.Join(",", type.GetProperties().Select(p => $"[{p.GetColumnsName()}]"));  
            ////version 2.0
            //var columns = string.Join(",", type.GetProperties().Select(p => $"[{p.GetName()}]"));


            ////string sql = $"Select {columns} From [{type.GetTableName()}] Where Id='{Id}'";  //拼接连接字符串

            ////version 2.0
            //string sql = $"Select {columns} From [{type.GetName()}] Where Id='{Id}'";  //拼接连接字符串 
            #endregion

            //以静态类静态方法的形式将反射获取数据库表名和列名的SQL缓存
            Type type = typeof(T);  //获取当前实体对象的数据类型
            var sql = $" {SqlBuilder<T>.GetSelectSql()}{Id}";

            using (var connection = new SqlConnection(ConfigurationManager.SqlConnectionString))   //此处为连接数据库字符串
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                connection.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    T t = (T)Activator.CreateInstance(type);
                    foreach (var property in type.GetProperties())
                    {
                        //property.SetValue(t, reader[property.Name].ToString() ?? null);
                        property.SetValue(t, reader[property.GetColumnsName()].ToString() ?? null);

                    }

                    return t;
                }
                return default;
            }
        }


        public bool Insert<T>(T t) where T : BaseModel
        {
            Type type = typeof(T); //获取当前实体对象的数据类型

            #region 以反射的方式，每一次调用通过反射的形式获取当前的数据库表名和列名
            //string columnsString = string.Join(",", type.GetProperties().Select(p => $"[{p.GetName()}]"));  //获取列名
            //string valueString = string.Join(",", type.GetProperties().Select(p => $"@{p.GetName()}")); //拼接字符串，以参数形式展现@Name、@Introduction          
            ////version 1.0
            ////string columnsString = string.Join(",", type.GetPropertiesWithoutKey().Select(p => $"[{p.GetName()}]"));  //获取列名
            ////string valueString = string.Join(",", type.GetPropertiesWithoutKey().Select(p => $"@{p.GetName()}")); //拼接字符串，以参数形式展现@Name、@Introduction          

            //string sql = $"Insert into [{type.GetName()}] ({columnsString}) Values ({valueString})";    //这个地方不能直接拼接字符串，防止SQL注入 
            #endregion

            string sql = SqlBuilder<T>.GetInsertSql();
            var paraArray = type.GetProperties()
                                            .Select(p => new SqlParameter($"@{p.GetName()}", p.GetValue(t) ?? DBNull.Value))
                                            .ToArray();

            using (var connection = new SqlConnection(ConfigurationManager.SqlConnectionString))   //此处为连接数据库字符串
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddRange(paraArray); //向sql中的参数注入数值
                connection.Open();
                var resultQuery = cmd.ExecuteNonQuery();
                return resultQuery == 1;
            }
        }

    }
}
