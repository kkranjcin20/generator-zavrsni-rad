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
                case "bool":
                    return "bool";
                case "char":
                case "character":
                    return "char";
                case "nchar":
                case "text":
                case "ntext":
                case "varchar":
                case "nvarchar":
                case "string":
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
                case "double":
                    return "double";
                case "int":
                case "integer":
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
