using System;
using ORMProject.Framework.MappingAttribute;

namespace ORMProject.Model
{
    public class BaseModel
    {
        [Table_PK_ID_Filter]
        public string Id { get; set; }
    }
}
