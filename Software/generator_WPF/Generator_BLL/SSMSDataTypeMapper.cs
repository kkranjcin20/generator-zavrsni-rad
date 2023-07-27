using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace generator.Generator_BLL
{
    public class SSMSDataTypeMapper : IDatabaseDataTypeMapper
    {
        public string MapDatabaseDataTypeToCSharpType(string dataType)
        {
            switch (dataType.ToLower())
            {
                case "bigint":
                    return "long";
                case "binary":
                case "varbinary":
                    return "byte[]";
                case "bit":
                    return "bool";
                case "char":
                case "nchar":
                case "text":
                case "ntext":
                case "varchar":
                case "nvarchar":
                    return "string";
                case "date":
                case "datetime":
                case "datetime2":
                    return "DateTime";
                case "decimal":
                case "numeric":
                    return "decimal";
                case "float":
                    return "float";
                case "int":
                    return "int";
                case "real":
                    return "float";
                case "smallint":
                    return "short";
                case "time":
                    return "TimeSpan";
                case "tinyint":
                    return "byte";
                case "uniqueidentifier":
                    return "Guid";
                default:
                    return "";
            }
        }
    }
}
