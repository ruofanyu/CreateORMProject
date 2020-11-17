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
    [AttributeUsage(AttributeTargets.All)]
    public abstract class BaseMappingAttribute : Attribute
    {
        private readonly string _Name;

        protected BaseMappingAttribute(string Name)
        {
            _Name = Name;
        }

        public string ReturnName()
        {
            return this._Name;
        }
    }
}
