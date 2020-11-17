/************************************************************************************
* Copyright (c) 2020 All Rights Reserved.
*命名空间：ORMProject.Framework
*文件名： EntityAttribute
*创建人： yedong Chen
*创建时间：2020/11/13 11:30:56
*描述
*=====================================================================
*修改标记
*修改时间：2020/11/13 11:30:56
*修改人：yedong Chen
*描述：
************************************************************************************/

using System;

namespace ORMProject.Framework.MappingAttribute
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityMappingAttribute : BaseMappingAttribute
    {
        //private readonly string _tableName;

        //public EntityMappingAttribute(string tableName)
        //{
        //    _tableName = tableName;
        //}

        //public string ReturnTableName()
        //{
        //    return this._tableName;
        //}
        public EntityMappingAttribute(string tableName) : base(tableName)
        {
        }
    }
}
