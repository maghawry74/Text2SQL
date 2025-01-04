using System.ComponentModel;
using System.Text;
using System.Text.Json;
using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using TextToSQL.Data;
using TextToSQL.Data.Models;
using TextToSQL.Extensions;

namespace TextToSQL.Tools;

public class Text2SqlTool : IDisposable, IAsyncDisposable
{
    private readonly AppDbContext _context;
    private readonly Lazy<Task<string>> _databaseInfo;
    public Text2SqlTool(IServiceScopeFactory serviceScopeFactory)
    {
        _context = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
        _databaseInfo = new Lazy<Task<string>>(async () =>
        {
            var connection = _context.Database.GetDbConnection();

            var command = connection.CreateCommand();

            command.CommandText = Queries.Queries.DatabaseInfoQuery;

            await connection.OpenAsync();
            var reader = command.ExecuteReader();
            var dataBaseInfo = new StringBuilder();
            while (reader.Read())
            {
                dataBaseInfo.Append(reader.GetString(0));
            }

            connection.Close();

            return dataBaseInfo.ToString();
        });
    }

    [KernelFunction,Description("Use this function to get the database schema and tables relations.")]
    public async Task<string> GetDataBaseInfo()
    {
        var dataBaseInfo = await _databaseInfo.Value;
        return $"""
                Assume a database with the following tables and columns exists: {dataBaseInfo}. 
                Based on provided database info compose a SQL Query based on semantic meaning and context of user query.
                Select only the columns that are relevant to the user query.
                """; 
    }
    
    [KernelFunction, Description("Use this tool when you need to query  the database.")]
    public async Task<object> QueryDatabase( [Description("The query to execute.")] string query )
    {
        try
        {
            var connection = _context.Database.GetDbConnection();

            var command = connection.CreateCommand();

            command.CommandText = query;

            await connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();
            var result = new List<Dictionary<string,object>>();
            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    row.Add(reader.GetName(i), reader.GetValue(i));
                } 
                result.Add(row);
            }
            await connection.CloseAsync();
            return result;
        }
        catch (Exception e)
        {
            return $"Your query is not valid dut to error {e.Message} {e.InnerException?.Message}. Please check the query and try again.";
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}