using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EasyMigrator.Parsing.Model;


namespace EasyMigrator.Tests.Schemas
{
    public class RemoveColumns
    {
        public class Pulse
        {
            [NPoco.TableName("Pulse")]
            public class Poco
            {
                public int? AccountId;
                public DateTime CreateDate;
                [Short] public string Category { get; set; }
                [Long] public string Link { get; set; }
                public int? RelatedObjectId { get; set; }
                [Short] public string Type { get; set; }
                [Medium] public string WhoPulsed { get; set; }
            }

            [Name("Pulse")]
            public class ColumnsToRemove
            {
                [Long] public string Link { get; set; }
                //[Short] public string Type;
                public DateTime CreateDate;
            }

            static Table Model = new Table {
                Name = "Pulse",
                Columns = new[] {
                    new Column {
                        Name = "Id",
                        Type = DbType.Int32,
                        IsPrimaryKey = true,
                        AutoIncrement = new AutoIncAttribute()
                    },
                    new Column {
                        Name = "AccountId",
                        Type = DbType.Int32,
                        IsNullable = true,
                    },
                    //new Column {
                    //    Name = "CreateDate",
                    //    Type = DbType.DateTime,
                    //},
                    new Column {
                        Name = "Category",
                        Type = DbType.String,
                        Length = 50,
                        IsNullable = true,
                    },
                    //new Column {
                    //    Name = "Link",
                    //    Type = DbType.String,
                    //    Length = 4000,
                    //    IsNullable = true,
                    //},
                    new Column {
                        Name = "RelatedObjectId",
                        Type = DbType.Int32,
                        IsNullable = true,
                    },
                    new Column {
                        Name = "Type",
                        Type = DbType.String,
                        Length = 50,
                        IsNullable = true,
                    },
                    new Column {
                        Name = "WhoPulsed",
                        Type = DbType.String,
                        Length = 255,
                        IsNullable = true,
                    },
                }
            };
        }
    }
}
