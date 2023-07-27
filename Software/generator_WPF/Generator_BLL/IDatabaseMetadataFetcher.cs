using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace generator.Generator_BLL
{
    public interface IDatabaseMetadataFetcher
    {
        List<TableMetadata> FetchTables(string connectionString);
    }
}
