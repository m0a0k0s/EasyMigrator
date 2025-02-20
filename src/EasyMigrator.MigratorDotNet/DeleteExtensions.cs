﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyMigrator.Extensions;
using EasyMigrator.Parsing.Model;
using Migrator.Framework;


namespace EasyMigrator
{
    static public class DeleteExtensions
    {
        static public void RemoveTable<T>(this ITransformationProvider Database, string alternateTableName = null) => Database.RemoveTable(typeof(T), alternateTableName);
        static public void RemoveTable(this ITransformationProvider Database, Type tableType, string alternateTableName = null)
        {
            var table = tableType.ParseTable().Table;
            if (alternateTableName != null)
                table.Name = alternateTableName;

            Database.RemoveForeignKeys(table);
            Database.RemoveIndices(table);
            Database.RemoveTable(table.Name.SqlQuote());
        }

        static public void RemoveColumns<T>(this ITransformationProvider Database) => Database.RemoveColumns(typeof(T));
        static public void RemoveColumns(this ITransformationProvider Database, Type tableType)
        {
            var table = tableType.ParseTable().Table;
            Database.RemoveForeignKeys(table);
            Database.RemoveIndices(table);
            Database.RemoveColumns(table);
        }

        static private void RemoveColumns(this ITransformationProvider Database, Table table)
        {
            var defined = table.Columns.DefinedInPoco().ToArray();
            foreach (var c in defined)
                Database.RemoveColumn(table.Name.SqlQuote(), c.Name.SqlQuote());
        }

        static private void RemoveForeignKeys(this ITransformationProvider Database, Table table)
        {
            foreach (var c in table.Columns.DefinedInPoco()) {
                var f = c.ForeignKey;
                if (f != null)
                    Database.RemoveForeignKey(table.Name.SqlQuote(), f.Name.SqlQuote());
            }
        }

        static private void RemoveIndices(this ITransformationProvider Database, Table table)
        {
            foreach (var ci in table.Indices)
                Database.RemoveIndexByName(table.Name, ci.Name);
        }
    }
}
