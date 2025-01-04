namespace TextToSQL.Queries;

public class Queries
{
    public const string DatabaseInfoQuery =
        $"""
         SELECT 
         -- Table and Column Information
         'Table: ' + TABLE_SCHEMA + '.' + TABLE_NAME + ' (' +
         STRING_AGG(COLUMN_NAME + ' ' + DATA_TYPE COLLATE Latin1_General_CI_AS, ', ') 
         WITHIN GROUP (ORDER BY ORDINAL_POSITION) + ')' AS TableInfo
         FROM 
         INFORMATION_SCHEMA.COLUMNS
         GROUP BY 
         TABLE_SCHEMA, TABLE_NAME
         UNION ALL
         SELECT 
         -- Table and Relationship Information
         'Table: ' + SCHEMA_NAME(tp.schema_id) + '.' + tp.name + ' (' +
         STRING_AGG(
         'Related to: ' + SCHEMA_NAME(tr.schema_id) + '.' + tr.name + 
         ' via ' + fk.name + 
         ' (' + fk.delete_referential_action_desc + ', ' + fk.update_referential_action_desc + ')',
         ', ' COLLATE Latin1_General_CI_AS
         ) WITHIN GROUP (ORDER BY fk.name) + ')' AS TableInfo
         FROM 
         sys.foreign_keys AS fk
         INNER JOIN 
         sys.foreign_key_columns AS fkc ON fk.object_id = fkc.constraint_object_id
         INNER JOIN 
         sys.tables AS tp ON fkc.parent_object_id = tp.object_id
         INNER JOIN 
         sys.tables AS tr ON fkc.referenced_object_id = tr.object_id
         GROUP BY 
         tp.schema_id, tp.name
         ORDER BY 
         TableInfo;
         """;
}