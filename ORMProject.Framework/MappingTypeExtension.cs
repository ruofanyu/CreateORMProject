/************************************************************************************
* Copyright (c) 2020 All Rights Reserved.
*命名空间：ORMProject.Framework
*文件名： MappingTypeExtension
*创建人： yedong Chen
*创建时间：2020/11/13 12:41:44
*描述
*=====================================================================
*修改标记
*修改时间：2020/11/13 12:41:44
*修改人：yedong Chen
*描述：
************************************************************************************/

using System;
using System.Reflection;

namespace ORMProject.Framework.MappingAttribute
{
    public static class MappingTypeExtension
    {

        /// <summary>
        /// 基类父方法
        /// </summary>
        /// <param name="type">可以是Type,也可以是Property</param>
        /// <returns></returns>
        public static string GetName(this MemberInfo type)
        {
            if (type.IsDefined(typeof(BaseMappingAttribute), true))
            {
                var attribute = type.GetCustomAttribute<BaseMappingAttribute>();

                return attribute.ReturnName();
            }
            else
            {
                return type.Name;
            }
        }


        public static string GetTableName(this Type type)
        {
            if (type.IsDefined(typeof(EntityMappingAttribute), true))
            {
                EntityMappingAttribute attribute = type.GetCustomAttribute<EntityMappingAttribute>();

                return attribute.ReturnName();
            }
            else
            {
                return type.Name;
            }
        }


        public static string GetColumnsName(this PropertyInfo type)
        {
            if (type.IsDefined(typeof(ColumnsMappingAttribute), true))
            {
                ColumnsMappingAttribute attribute = type.GetCustomAttribute<ColumnsMappingAttribute>();

                return attribute.ReturnName();
            }
            else
            {
                return type.Name;
            }
        }

    }
}
