using System;
using System.Collections.Generic;
using System.Text;

namespace ORMProject.Framework.MappingAttribute
{
    /// <summary>
    /// 定义一个特性，用来过滤数据库表中主键为自增的ID
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Table_PK_ID_FilterAttribute : Attribute
    {
    }
}
