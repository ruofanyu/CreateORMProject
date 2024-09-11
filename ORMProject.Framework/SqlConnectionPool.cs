/************************************************************************************
* Copyright (c) 2020 All Rights Reserved.
*命名空间：ORMProject.Framework
*文件名： SqlConnectionPool
*创建人： yedong Chen
*创建时间：2020/11/19 9:22:44
*描述
*=====================================================================
*修改标记
*修改时间：2020/11/19 9:22:44
*修改人：yedong Chen
*描述：
************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace ORMProject.Framework
{
    public class SqlConnectionPool
    {

        public static string GetConnectionString(SqlConnectionType type)
        {
            string conn;
            switch (type)
            {
                case SqlConnectionType.Read:
                    conn = Dispatcher(ConfigurationManager.SqlConnectionStringRead);
                    break;
                case SqlConnectionType.Write:
                    conn = ConfigurationManager.SqlConnectionStringWrite;
                    break;
                default:
                    throw new Exception("ConnectionString Wrong");
            }
            return conn;
        }


        /// <summary>
        /// 调度策略
        /// </summary>
        /// <param name="connectionstrings"></param>
        /// <returns></returns>
        private static string Dispatcher(string[] connectionstrings)
        {
            //随即调度
            //return connectionstrings[new Random(_seed++).Next(0, connectionstrings.Length)];

            //轮询调度
                //在轮询的时候要注意给种子数据加一个锁，否则多线程时造成争抢，从而无法真正的达到轮询
            return connectionstrings[_seed++ % connectionstrings.Length];

            //权重调度
            //根据每一个数据库的权重，分配数据库连接，最简单的方式是根据每一个数据库的比例，创建一个数组，然后根据数组中的取出来的数，进行分配
                //假设A、B、Ｃ三个数据库，权重比例是２：３：４，则可以定义一个【1，1，2，2，2，3，3，3，3】的数组，然后随机取值


        }

        //最为随机数的种子
        private static int _seed = 0;

        //调用数据库连接字符串的类型
        public enum SqlConnectionType
        {
            Read,
            Write
        }
    }
}
