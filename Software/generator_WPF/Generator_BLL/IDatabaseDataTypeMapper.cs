using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace generator.Generator_BLL
{
    public interface IDatabaseDataTypeMapper
    {
        string MapDatabaseDataTypeToCSharpType(string databaseType);
    }
}
