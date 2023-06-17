using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace generator_zavrsni_rad
{
    public class TableMetadata
    {
        public string TableName { get; set; }
        public List<ColumnMetadata> Columns { get; set; }
        public List<IndexMetadata> Indexes { get; set; }
        public List<ConstraintMetadata> Constraints { get; set; }
        public List<RelationshipMetadata> Relationships { get; set; }
        public int RowCount { get; set; }
        public int StorageSize { get; set; }
        public string TableSchema { get; set; }
    }

    public class ColumnMetadata
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public bool IsNullable { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsForeignKey { get; set; }
        public bool IsUnique { get; set; }
    }

    public class IndexMetadata
    {
        public string IndexName { get; set; }
        public bool IsUnique { get; set; }
        public List<string> Columns { get; set; }
    }

    public class ConstraintMetadata
    {
        public string ConstraintName { get; set; }
        public string ConstraintType { get; set; }
        public List<string> Columns { get; set; }
    }

    public class RelationshipMetadata
    {
        public string RelationshipName { get; set; }
        public string RelatedTableName { get; set; }
        public string RelatedColumnName { get; set; }
        public string LocalColumnName { get; set; }
    }
}
