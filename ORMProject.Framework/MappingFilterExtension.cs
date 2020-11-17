/************************************************************************************
* Copyright (c) 2020 All Rights Reserved.
*命名空间：ORMProject.Framework
*文件名： MappingFilterExtension
*创建人： yedong Chen
*创建时间：2020/11/15 13:01:45
*描述
*=====================================================================
*修改标记
*修改时间：2020/11/15 13:01:45
*修改人：yedong Chen
*描述：
************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ORMProject.Framework.MappingAttribute;

namespace ORMProject.Framework
{
    public static class MappingFilterExtension
    {
        /// <summary>
        ///通过Table_PK_ID_Filter特性，将实体类中打上这个标签的属性去除
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertiesWithoutKey(this Type type)
        {
            return type.GetProperties().Where(p => !p.IsDefined(typeof(Table_PK_ID_FilterAttribute), true));
        }
    }
}
