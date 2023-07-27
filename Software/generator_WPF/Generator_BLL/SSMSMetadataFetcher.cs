using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Xml.Linq;

namespace generator.Generator_BLL
{
    public class SSMSMetadataFetcher : IDatabaseMetadataFetcher
    {
        List<TableMetadata> tables = new List<TableMetadata>();
        ColumnMetadata column;
        SqlCommand command;
        SqlDataReader reader;

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

        public void FetchColumns(TableMetadata table)
        {
            command.CommandText = $"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{table.Name}'";
            reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                column = new ColumnMetadata();
                column.Name = reader["COLUMN_NAME"].ToString();
                if (reader["DATA_TYPE"].ToString() == "varchar")
                {
                    column.DataType = "string";
                }
                else if (reader["DATA_TYPE"].ToString() == "date")
                {
                    column.DataType = "DateTime";
                }
                else
                {
                    column.DataType = reader["DATA_TYPE"].ToString();
                }
                table.Columns.Add(column);
            }
            reader.Close();
        }
    }
}