using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using ORMProject.Framework;
using ORMProject.Framework.MappingAttribute;
using ORMProject.Model;

namespace ORMProject.DAL
{
    /// <summary>
    /// 延迟提交模式，模拟DBContext
    /// </summary>
    public class SqlHelperDelay
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
            var sql = $" {SqlBuilder<T>.GetSelectSql()}'{Id}'";


            //通过连接池获取不同操作的连接字符串
            var conn = SqlConnectionPool.GetConnectionString(SqlConnectionPool.SqlConnectionType.Read);

            //using (var connection = new SqlConnection(ConfigurationManager.SqlConnectionStringWrite))   //此处为连接数据库字符串
            using (var connection = new SqlConnection(conn))   //此处为连接数据库字符串
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
                        
                        //TODO 后期需要判断property的类型进行转换
                        property.SetValue(t, reader[property.GetColumnsName()].ToString() ?? null);

                    }

                    return t;
                }

                return default;
            }
        }

        private IList<SqlCommand> _commands = new List<SqlCommand>();



        public void Insert<T>(T t) where T : BaseModel
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

            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddRange(paraArray); //向sql中的参数注入数值
            this._commands.Add(cmd);

            //通过连接池获取不同操作的连接字符串
            var conn = SqlConnectionPool.GetConnectionString(SqlConnectionPool.SqlConnectionType.Write);
        }

        public int Update<T>(T t) where T : BaseModel, new()
        {
            Type type = typeof(T);

            string sql = SqlBuilder<T>.GetUpdateSql();
            var paramArray = type.GetProperties()
                .Select(p => new SqlParameter($"@{p.GetName()}", p.GetValue(t) ?? DBNull.Value)).ToArray();

            var conn = SqlConnectionPool.GetConnectionString(SqlConnectionPool.SqlConnectionType.Write);

            using (var connection = new SqlConnection(conn))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddRange(paramArray);
                connection.Open();
                var result = cmd.ExecuteNonQuery();

                return result;
            }
        }


        /// <summary>
        ///保存操作
        /// </summary>
        public void SaveChange()
        {
            // 因为只有写入数据库的才有保存操作，直接从数据库连接池中提取写连接字符出啊
            var conn = SqlConnectionPool.GetConnectionString(SqlConnectionPool.SqlConnectionType.Write);
            if (this._commands.Count > 0)
            {

                using (var connection = new SqlConnection(conn))
                {
                    connection.Open();
                    using (SqlTransaction trans = connection.BeginTransaction())
                    {
                        try
                        {
                            foreach (var cmd in this._commands)
                            {
                                cmd.Connection = connection;
                                cmd.Transaction = trans;
                                cmd.ExecuteNonQuery();
                            }

                            trans.Commit();
                        }
                        catch (Exception)
                        {
                            trans.Rollback();
                            throw;
                        }
                        finally
                        {
                            this._commands?.Clear();
                        }
                    }
                }
            }
        }
    }
}
