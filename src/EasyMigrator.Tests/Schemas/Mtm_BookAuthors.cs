using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EasyMigrator.Parsing.Model;
using EasyMigrator.Tests.TableTest;


namespace EasyMigrator.Tests.Schemas
{
    public class Mtm_BookAuthors : TableTestCase
    {
        [MigrationOrder(1)]
        public class Author
        {
            public class Poco
            {
                [Medium, NotNull] public string FirstName { get; set; }
                [Medium, NotNull] public string LastName { get; set; }
            }

            static public Table Model = new Table {
                Name = "Author",
                Columns = new[] {
                    new Column {
                        Name = "Id",
                        Type = DbType.Int32,
                        IsPrimaryKey = true,
                        AutoIncrement = new AutoIncAttribute()
                    },
                    new Column {
                        Name = "FirstName",
                        Type = DbType.String,
                        Length = 255
                    },
                    new Column {
                        Name = "LastName",
                        Type = DbType.String,
                        Length = 255
                    }
                }
            };
        }

        [MigrationOrder(2)]
        public class Book
        {
            public class Poco
            {
                [Medium, NotNull] public string Title { get; set; }
                [Short, NotNull] public string Isbn { get; set; }
            }

            static public Table Model = new Table
            {
                Name = "Book",
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
                        Length = 255
                    },
                    new Column {
                        Name = "Isbn",
                        Type = DbType.String,
                        Length = 50
                    }
                }
            };
        }

        [MigrationOrder(3)]
        public class BookAuthor
        {
            public class Poco
            {
                [Pk, Fk(typeof(Book.Poco))] public int BookId { get; set; }
                [Pk, Fk(typeof(Author.Poco))] public int AuthorId { get; set; }
            }

            static public Table Model = new Table
            {
                Name = "BookAuthor",
                Columns = new[] {
                    new Column {
                        Name = "BookId",
                        Type = DbType.Int32,
                        IsPrimaryKey = true,
                        ForeignKey = new FkAttribute("Book") { Column = "Id" },
                    },
                    new Column {
                        Name = "AuthorId",
                        Type = DbType.Int32,
                        IsPrimaryKey = true,
                        ForeignKey = new FkAttribute("Author") { Column = "Id" },
                    },
                },
                Indices = new List<IIndex> {
                    new Index {
                        Name = "IX_BookAuthor_BookId",
                        Unique = false,
                        Clustered = false,
                        Columns = new [] { new IndexColumn("BookId") }
                    },
                    new Index {
                        Name = "IX_BookAuthor_AuthorId",
                        Unique = false,
                        Clustered = false,
                        Columns = new [] { new IndexColumn("AuthorId") }
                    },
                },
            };
        }
    }
}
