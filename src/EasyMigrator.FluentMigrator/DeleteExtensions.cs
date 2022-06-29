﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyMigrator.Extensions;
using EasyMigrator.Parsing.Model;
using FluentMigrator.Builders.Delete;


namespace EasyMigrator
{
    static public class DeleteExtensions
    {
        static public void Table<T>(this IDeleteExpressionRoot Delete) => Delete.Table(typeof(T));
        static public void Table(this IDeleteExpressionRoot Delete, Type tableType)
        {
            var table = tableType.ParseTable().Table;
            Delete.ForeignKeys(table);
            Delete.Indexes(table);
            Delete.Table(table.Name);
        }

        static public void Columns<T>(this IDeleteExpressionRoot Delete) => Delete.Columns(typeof(T));
        static public void Columns(this IDeleteExpressionRoot Delete, Type tableType)
        {
            var table = tableType.ParseTable().Table;
            Delete.ForeignKeys(table);
            Delete.Indexes(table);
            Delete.Columns(table);
        }

        static private void Columns(this IDeleteExpressionRoot Delete, Table table)
        {
            foreach (var c in table.Columns.DefinedInPoco())
                Delete.Column(c.Name).FromTable(table.Name);
        }

        static private void ForeignKeys(this IDeleteExpressionRoot Delete, Table table)
        {
            foreach (var c in table.Columns.DefinedInPoco()) {
                var f = c.ForeignKey;
                if (f == null)
                    continue;

                if (f.Name != null)
                    Delete.ForeignKey(f.Name).OnTable(table.Name).InSchema(table.Schema);
                else
                    Delete.ForeignKey()
                          .FromTable(table.Name)
                          .ForeignColumn(c.Name)
                          .ToTable(f.Table)
                          .PrimaryColumn(f.Column);
            }
        }

        static private void Indexes(this IDeleteExpressionRoot Delete, Table table)
        {
            foreach (var ci in table.Indices) {
                if (ci.Name != null)
                    Delete.Index(ci.Name).OnTable(table.Name).InSchema(table.Schema);
                else
                    Delete.Index()
                          .OnTable(table.Name).InSchema(table.Schema)
                          .OnColumns(ci.Columns.Select(c => c.ColumnName).ToArray());
            }
        }
    }
}
