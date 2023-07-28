using System.Collections.Generic;

namespace generator.Generator_BLL
{
    public interface IDatabaseMetadataFetcher
    {
        List<TableMetadata> FetchTables(string connectionString);
    }
}
