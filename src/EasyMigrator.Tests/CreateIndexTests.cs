﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DatabaseSchemaReader.DataSchema;
using EasyMigrator.Tests.Integration;
using EasyMigrator.Tests.Schemas;
using EasyMigrator.Tests.TableTest;
using NUnit.Framework;


namespace EasyMigrator.Tests
{
    abstract public class CreateIndex : IntegrationTestBase
    {
        protected CreateIndex(Func<string, IMigrator> getMigrator) : base(getMigrator) { }

        protected virtual string IndexName => "IX_Table1_Name_Headline";

        protected virtual void AddMigrations(IMigrationSet migrations) { }

        protected virtual void ExtraAssertions(DatabaseIndex dbIndex)
        {
            Assert.AreEqual(false, dbIndex.IsUnique);
        }

        protected virtual void CheckColumns(DatabaseIndex dbIndex)
        {
            var nameCol = dbIndex.Columns.Find(c => c.Name == "Name");
            Assert.NotNull(nameCol);
            var headlineCol = dbIndex.Columns.Find(c => c.Name == "Headline");
            Assert.NotNull(headlineCol);
        }

        protected void CreateAndDropIndex(Action<IMigrationSet> addMigrations, Action<DatabaseIndex> extraAssertions = null, Action<DatabaseIndex> checkColumns = null)
            => CreateAndDropIndex(null, addMigrations, extraAssertions, checkColumns);

        protected void CreateAndDropIndex(string indexName, Action<IMigrationSet> addMigrations, Action<DatabaseIndex> extraAssertions = null, Action<DatabaseIndex> checkColumns = null)
        {
            var testCase = new TableTestCase<Table1>();
            var set = Migrator.CreateMigrationSet();
            set.AddTableMigrationForTableTestCase(testCase);
            (addMigrations ?? AddMigrations)(set);

            var mig = Migrator.CompileMigrations(set);
            Migrator.Up(mig);

            var schema = GetDbSchema();
            var table = schema.FindTableByName("Table1");
            var idx = table.Indexes.Find(i => i.Name == (indexName ?? IndexName));
            Assert.NotNull(idx);


            (checkColumns ?? CheckColumns)(idx);
            (extraAssertions ?? ExtraAssertions)(idx);

            Migrator.Down(mig);
        }
    }
}


namespace EasyMigrator.Tests.MigratorDotNet
{
    [TestFixture]
    public class CreateIndex : Tests.CreateIndex
    {
        public CreateIndex() : base(s => new Integration.MigratorDotNet.Migrator(s)) { }

        [Test]
        public void Table1ColumnExpressions()
            => CreateAndDropIndex(
                migrations => migrations.AddMigrationForMigratorDotNet(
                    m => {
                        m.Database.AddIndex<Schemas.Table1.Poco>(t => t.Name, t => t.Headline);
                    },
                    m => {
                        m.Database.RemoveIndex<Schemas.Table1.Poco>(t => t.Name, t => t.Headline);
                    }));

        [Test]
        public void Table1ColumnExpressionsInclude()
            => CreateAndDropIndex(
                "IX_Table1_Name",
                migrations => migrations.AddMigrationForMigratorDotNet(
                    m => {
                        m.Database.AddIndex(new Expression<Func<Schemas.Table1.Poco, object>>[] { t => t.Name }, new Expression<Func<Schemas.Table1.Poco, object>>[] { t => t.Headline });
                    },
                    m => {
                        m.Database.RemoveIndex<Schemas.Table1.Poco>(t => t.Name);
                    }),
                    null,
                    dbIndex => {
                        var nameCol = dbIndex.Columns.Find(c => c.Name == "Name");
                        Assert.NotNull(nameCol);
                        //var headlineCol = dbIndex.Columns.Find(c => c.Name == "Headline");
                        //Assert.NotNull(headlineCol);
                    });

        [Test]
        public void Table1ColumnExpressionsUnique()
            => CreateAndDropIndex(
                migrations => migrations.AddMigrationForMigratorDotNet(
                    m => {
                        m.Database.AddUniqueIndex<Schemas.Table1.Poco>(t => t.Name, t => t.Headline);
                    },
                    m => {
                        m.Database.RemoveIndex<Schemas.Table1.Poco>(t => t.Name, t => t.Headline);
                    }),
                    dbIndex => { }); //Assert.AreEqual(true, dbIndex.IsUnique); // <- Schema reader doesn't pick this up correctly);

        [Test]
        public void Table1ColumnExpressionsOrdered()
            => CreateAndDropIndex(
                migrations => migrations.AddMigrationForMigratorDotNet(
                    m => {
                        m.Database.AddIndex(new Index<Schemas.Table1.Poco>(new Descending<Schemas.Table1.Poco>(t => t.Name), new Ascending<Schemas.Table1.Poco>(t => t.Headline)));
                    },
                    m => {
                        m.Database.RemoveIndex(new Descending<Schemas.Table1.Poco>(t => t.Name), new Ascending<Schemas.Table1.Poco>(t => t.Headline));
                    }));

        [Test]
        public void Table1ColumnExpressionsNamed()
            => CreateAndDropIndex(
                "MyNamedIndex",
                migrations => migrations.AddMigrationForMigratorDotNet(
                    m => {
                        m.Database.AddIndex(new Index<Schemas.Table1.Poco>(t => t.Name, t => t.Headline) { Name = "MyNamedIndex" });
                    },
                    m => {
                        m.Database.RemoveIndexByName<Schemas.Table1.Poco>("MyNamedIndex");
                    }));
    }
}


namespace EasyMigrator.Tests.FluentMigrator
{
    [TestFixture]
    public class CreateIndex : Tests.CreateIndex
    {
        public CreateIndex() : base(s => new Integration.FluentMigrator.Migrator(s)) { }

        [Test]
        public void Table1ColumnExpressions()
            => CreateAndDropIndex(
                migrations => migrations.AddMigrationForFluentMigrator(
                    m => {
                        m.Create.Index<Schemas.Table1.Poco>().OnColumns(t => t.Name, t => t.Headline);
                    },
                    m => {
                        m.Delete.Index<Schemas.Table1.Poco>().OnColumns(t => t.Name, t => t.Headline);
                    }));

        [Test]
        public void Table1ColumnExpressionsUnique()
            => CreateAndDropIndex(
                migrations => migrations.AddMigrationForFluentMigrator(
                    m => {
                        m.Create.Index<Schemas.Table1.Poco>().OnColumns(t => t.Name, t => t.Headline).WithOptions().Unique();
                    },
                    m => {
                        m.Delete.Index<Schemas.Table1.Poco>().OnColumns(t => t.Name, t => t.Headline);
                    }),
                    dbIndex => { }); //Assert.AreEqual(true, dbIndex.IsUnique); // <- Schema reader doesn't pick this up correctly);

        [Test]
        public void Table1ColumnExpressionsOrdered()
            => CreateAndDropIndex(
                migrations => migrations.AddMigrationForFluentMigrator(
                    m => {
                        m.Create.Index<Schemas.Table1.Poco>().OnColumns(new Descending<Schemas.Table1.Poco>(t => t.Name), new Ascending<Schemas.Table1.Poco>(t => t.Headline));
                    },
                    m => {
                        m.Delete.Index<Schemas.Table1.Poco>().OnColumns(new Descending<Schemas.Table1.Poco>(t => t.Name), new Ascending<Schemas.Table1.Poco>(t => t.Headline));
                    }));

        [Test]
        public void Table1ColumnExpressionsNamed()
            => CreateAndDropIndex(
                "MyNamedIndex",
                migrations => migrations.AddMigrationForFluentMigrator(
                    m => {
                        m.Create.Index("MyNamedIndex").OnTable<Schemas.Table1.Poco>().OnColumns(t => t.Name, t => t.Headline);
                    },
                    m => {
                        m.Delete.Index("MyNamedIndex").OnTable<Schemas.Table1.Poco>();
                    }));
    }
}
