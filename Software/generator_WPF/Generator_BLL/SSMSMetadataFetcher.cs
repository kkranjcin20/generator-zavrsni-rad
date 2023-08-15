using System.Collections.Generic;
using System.Data.SqlClient;

namespace generator.Generator_BLL
{
    public class SSMSMetadataFetcher : IDatabaseMetadataFetcher
    {
        List<TableMetadata> tables = new List<TableMetadata>();
        ColumnMetadata column;
        SqlCommand command;
        SqlDataReader reader;
        SSMSDataTypeMapper dataTypeMapper = new SSMSDataTypeMapper();

        public List<TableMetadata> FetchTables(string connectionString)
        {
            string query = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                command = new SqlCommand(query, connection);
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var table = new TableMetadata();
                    table.Columns = new List<ColumnMetadata>();
                    table.Name = reader["TABLE_NAME"].ToString();
                    tables.Add(table);
                }
                reader.Close();

                foreach (var table in tables)
                {
                    FetchColumns(table);
                }

                connection.Close();
            }
            return tables;
        }

        private void FetchColumns(TableMetadata table)
        {
            command.CommandText = $"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{table.Name}'";
            reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                column = new ColumnMetadata();
                column.Name = reader["COLUMN_NAME"].ToString();

                column.DataType = dataTypeMapper.MapDatabaseDataTypeToCSharpType(reader["DATA_TYPE"].ToString());

                table.Columns.Add(column);
            }
            reader.Close();
        }
    }
}