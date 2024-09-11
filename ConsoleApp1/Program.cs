using System;
using ORMProject.DAL;
using ORMProject.Model;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlHelper helper = new SqlHelper();

            //Companies注意实体类需要自己在Model或者其他的地方创建出来
            var result = helper.FindDataList<CompaniesModel>("47b901a4-83c4-414a-bcf2-1faa42f90807");


            Console.WriteLine(result.Id);
            Console.WriteLine(result.Name);
            Console.WriteLine(result.Introduction);

            result.Id = Guid.NewGuid().ToString();

            var isSuccess = helper.Insert(result);

            result.Introduction = "这是一个新的介绍！";
            var isUpdate = helper.Update(result);


            Console.ReadKey();
        }
    }
}
