﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EasyMigrator.Parsing.Model;
using EasyMigrator.Tests.TableTest;


namespace EasyMigrator.Tests.Schemas
{
    public class Fk_AddToExisting : TableTestCase
    {
        [MigrationOrder(1)]
        class Select
        {
            class Poco
            {
                [Medium] string Description { get; set; }
            }

            static Table Model = new Table {
                Name = "Select",
                Columns = new[] {
                    new Column {
                        Name = "Id",
                        Type = DbType.Int32,
                        IsPrimaryKey = true,
                        AutoIncrement = new AutoIncAttribute()
                    },
                    new Column {
                        Name = "Description",
                        Type = DbType.String,
                        Length = 255,
                        IsNullable = true,
                    },
                }
            };
        }

        [MigrationOrder(2)]
        public class Alias
        {
            class Poco
            {
                [Medium] string Description { get; set; }
            }

            static Table Model = new Table {
                Name = "Alias",
                Columns = new[] {
                    new Column {
                        Name = "Id",
                        Type = DbType.Int32,
                        IsPrimaryKey = true,
                        AutoIncrement = new AutoIncAttribute()
                    },
                    new Column {
                        Name = "Description",
                        Type = DbType.String,
                        Length = 255,
                        IsNullable = true,
                    },
                    new Column {
                        Name = "SelectId",
                        Type = DbType.Int32,
                        ForeignKey = new FkAttribute("Select") { Column = "Id", OnDelete = Rule.Cascade, OnUpdate = Rule.Cascade },
                    },
                },
                Indices = new List<IIndex> {
                    new Index {
                        Name = "IX_Alias_SelectId",
                        Unique = false,
                        Clustered = false,
                        Columns = new [] { new IndexColumn("SelectId") }
                    },
                },
            };
        }

        public class ColumnsToAdd
        {
            [MigrationOrder(3)]
            public class Alias
            {
                public class Poco
                {
                    [Fk("Select", OnDelete = Rule.Cascade, OnUpdate = Rule.Cascade)] public int SelectId { get; set; }
                }
            }
        }
    }
}
