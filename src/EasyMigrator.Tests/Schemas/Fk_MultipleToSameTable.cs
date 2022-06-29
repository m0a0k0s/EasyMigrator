﻿using System.Collections.Generic;
using System.Data;
using EasyMigrator.Parsing.Model;
using EasyMigrator.Tests.TableTest;


namespace EasyMigrator.Tests.Schemas
{
    public class Fk_MultipleToSameTable_Int32 : TableTestCase
    {
        [MigrationOrder(1)]
        class Stuff
        {
            class Poco
            {
                [Medium] string Description { get; set; }
            }

            static Table Model = new Table {
                Name = "Stuff",
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
        class Assoc
        {
            class Poco
            {
                [Fk("Stuff")] int StuffId { get; set; }
                [Fk("Stuff")] int AltStuffId { get; set; }
                [Fk("Stuff")] int Desc { get; set; }
            }

            static Table Model = new Table {
                Name = "Assoc",
                Columns = new[] {
                    new Column {
                        Name = "Id",
                        Type = DbType.Int32,
                        IsPrimaryKey = true,
                        AutoIncrement = new AutoIncAttribute()
                    },
                    new Column {
                        Name = "StuffId",
                        Type = DbType.Int32,
                        ForeignKey = new FkAttribute("Stuff") { Column = "Id" },
                    },
                    new Column {
                        Name = "AltStuffId",
                        Type = DbType.Int32,
                        ForeignKey = new FkAttribute("Stuff") { Column = "Id" },
                    },
                    new Column {
                        Name = "Desc",
                        Type = DbType.Int32,
                        ForeignKey = new FkAttribute("Stuff") { Column = "Id" },
                    }
                },
                Indices = new[] {
                    new Index {
                        Name = "IX_Assoc_StuffId",
                        Unique = false,
                        Clustered = false,
                        Columns = new [] { new IndexColumn("StuffId") }
                    },
                    new Index {
                        Name = "IX_Assoc_AltStuffId",
                        Unique = false,
                        Clustered = false,
                        Columns = new [] { new IndexColumn("AltStuffId") }
                    },
                    new Index {
                        Name = "IX_Assoc_Desc",
                        Unique = false,
                        Clustered = false,
                        Columns = new [] { new IndexColumn("Desc") }
                    },
                },
            };
        }
    }
}
