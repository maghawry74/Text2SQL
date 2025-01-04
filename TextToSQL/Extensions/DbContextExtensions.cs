using System.Text;
using Microsoft.EntityFrameworkCore;

namespace TextToSQL.Extensions;

public static class DbContextExtensions
{
    public static string ToSql(this DbContext context)
    {
        var model = context.Model;
        var sb = new StringBuilder();

        foreach (var entityType in model.GetEntityTypes())
        {
            // Table Name and Schema
            var tableName = entityType.GetTableName();
            var schemaName = entityType.GetSchema();
            sb.AppendLine($"Table: {schemaName ?? "dbo"}.{tableName}");

            // Columns
            sb.AppendLine("  Columns:");
            foreach (var property in entityType.GetProperties())
            {
                var columnName = property.GetColumnName();
                var columnType = property.ClrType.Name;
                sb.AppendLine($"    {columnName} ({columnType})");
            }

            // Relationships (One-to-Many and Many-to-One)
            sb.AppendLine("  Relationships:");
            foreach (var foreignKey in entityType.GetForeignKeys())
            {
                var principalEntityName = foreignKey.PrincipalEntityType.GetTableName();
                var navigationProperty = foreignKey.DependentToPrincipal?.Name ?? "(No navigation property)";
                sb.AppendLine($"    Related to: {principalEntityName} via {navigationProperty}");
            }

            // Many-to-Many Relationships
            sb.AppendLine("  Many-to-Many Relationships:");
            foreach (var skipNavigation in entityType.GetSkipNavigations())
            {
                var targetEntityName = skipNavigation.TargetEntityType.GetTableName();
                var navigationProperty = skipNavigation.Name;
                sb.AppendLine($"    Related to: {targetEntityName} via {navigationProperty}");
            }

            sb.AppendLine(); // Add a blank line between tables
        }


        return sb.ToString();
    }
}