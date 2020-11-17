using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using ORMProject.Framework;
using ORMProject.Framework.MappingAttribute;


namespace ORMProject.Model
{
    [EntityMapping("Companies")]
    public class CompaniesModel : BaseModel
    {

        //[Column("Name")]    //暂时其实还没有用
        [ColumnsMapping("Name")]
        public string Name { get; set; }//此处可以和数据库不对应,但是上面的标签中的必须与数据库对应,否则出错


        //[Column("Introduction")]    //暂时还没有
        [ColumnsMapping("Introduction")]
        public string Introduction { get; set; }



        //     
    }
}