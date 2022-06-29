using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EasyMigrator.Parsing.Model;


namespace EasyMigrator.Tests.Schemas
{
    public class ChangeColumns
    {
        public class Content
        { 
            [NPoco.TableName("Content")]
            public class Poco
            {
                [Max] public string Title { get; set; }
                [Medium] public string ExternalId { get; set; }
                public DateTime DisplayStartDate;
                [Default("GETUTCDATE()")] public DateTime? DisplayEndDate { get; set; }
                [Default("GETUTCDATE()")] public DateTime CreatedOn { get; set; }
            }

            [Name("Content")]
            public class ColumnsToChange
            {
                [Precision(2), Default("GETUTCDATE()")] public DateTime DisplayStartDate { get; set; }
                [Precision(2)] public DateTime? DisplayEndDate { get; set; }
                [Precision(2), Default("GETUTCDATE()")] public DateTime CreatedOn { get; set; }
            }

            [Name("Content")]
            public class OriginalColumns
            {
                public DateTime DisplayStartDate;
                public DateTime? DisplayEndDate;
            }

            static Table Model = new Table {
                Name = "Content",
                Columns = new[] {
                    new Column {
                        Name = "Id",
                        Type = DbType.Int32,
                        IsPrimaryKey = true,
                        AutoIncrement = new AutoIncAttribute()
                    },
                    new Column {
                        Name = "Title",
                        Type = DbType.String,
                        Length = int.MaxValue,
                        IsNullable = true,
                    },
                    new Column {
                        Name = "ExternalId",
                        Type = DbType.String,
                        Length = 255,
                        IsNullable = true,
                    },
                    new Column {
                        Name = "DisplayStartDate",
                        Type = DbType.DateTime2,
                        Precision = new PrecisionAttribute(2),
                        DefaultValue = "GETUTCDATE()",
                    },
                    new Column {
                        Name = "DisplayEndDate",
                        IsNullable = true,
                        Type = DbType.DateTime2,
                        Precision = new PrecisionAttribute(2),
                    },
                    new Column {
                        Name = "CreatedOn",
                        Type = DbType.DateTime2,
                        Precision = new PrecisionAttribute(2),
                        DefaultValue = "GETUTCDATE()",
                    },
                }
            };
        }
    }
}
