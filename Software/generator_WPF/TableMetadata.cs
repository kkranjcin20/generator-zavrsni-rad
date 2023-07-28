using System.Collections.Generic;

namespace generator
{
    public class TableMetadata
    {
        public string Name { get; set; }
        public List<ColumnMetadata> Columns { get; set; }
        public string Namespace { get; set; }
    }

    public class ColumnMetadata
    {
        public string Name { get; set; }
        public string AccessModifier { get; set; }
        public string DataType { get; set; }
    }
}
