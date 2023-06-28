using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace generator_WPF
{
    public class TableMetadata
    {
        public string Name { get; set; }
        public List<ColumnMetadata> Columns { get; set; }
        public string Namespace { get; set; }
    }

    public class ColumnMetadata
    {
        public string AccessModifier { get; set; }
        public string DataType { get; set; }
        public string Name { get; set; }
    }
}
