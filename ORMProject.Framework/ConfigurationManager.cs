using System;
using System.IO;
using Microsoft.Extensions.Configuration;


namespace ORMProject.Framework
{
    public class ConfigurationManager
    {
        public static string SqlConnectionString
        {
            get;
            set;
        }

        //暂时还未IOC注入
        //通过静态构造函数来调用
        static ConfigurationManager()
        {
            //这里读取的appsettings配置文件总是在程序的根目录下

            var builder = new ConfigurationBuilder()    //需要导入Configuration包
                .SetBasePath(Directory.GetCurrentDirectory())   //需要导入Configuration.FileExtension包
                .AddJsonFile("appsettings.json");   //需要导入Configuration.Json包   
                                                    

            IConfiguration configuration = builder.Build();

            SqlConnectionString = configuration["connectionString"];
        }
    }
}
