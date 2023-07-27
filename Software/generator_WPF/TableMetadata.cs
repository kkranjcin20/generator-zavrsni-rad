using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
