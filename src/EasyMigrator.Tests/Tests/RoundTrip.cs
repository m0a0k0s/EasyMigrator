﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyMigrator.Tests.Integration;
using NUnit.Framework;
using Migration = FluentMigrator.Migration;


namespace EasyMigrator.Tests
{
    abstract public class RoundTrip : IntegrationTestBase
    {
        protected RoundTrip(Func<string, IMigrator> getMigrator) : base(getMigrator) { }

        [Test] public void Table1() { Test<Data.Table1>(); }
        [Test] public void Fk1() { Test<Data.Fk1>(); }

        override protected void Test(ITableTestCase testCase)
        {
            foreach (var data in testCase.Datum)
                Migrator.Up(data.Poco);

            foreach (var data in testCase.Datum)
                AssertEx.AreEqual(data.Model, GetTableModelFromDb(data.Model.Name));

            foreach (var data in testCase.Datum.Reverse())
                Migrator.Down(data.Poco);
        }
    }
}


namespace EasyMigrator.Tests.FluentMigrator
{

    [TestFixture]
    public class RoundTrip : Tests.RoundTrip
    {
        public RoundTrip() : base(s => new Integration.Migrators.FluentMigrator(s)) { }

        [Test]
        public void RemoveFkCol()
        {
            (Migrator as Integration.Migrators.FluentMigrator).Up((Action<Migration>)(m => {
                m.Create.Table<FkStuff>();
                m.Create.Table<FkCol>();
                m.Create.Columns<FkColTable>();
            }));

            (Migrator as Integration.Migrators.FluentMigrator).Down((Action<Migration>)(m => {
                m.Delete.Columns<FkColTable>();
            }));
        }

        public class FkStuff
        {
            string Desc;
        }

        public class FkCol
        {
            string Name;
        }

        public class FkColTable
        {
            [Fk("FkStuff")] int StuffId;
        }
    }
}

namespace EasyMigrator.Tests.MigratorDotNet
{
    //[TestFixture]
    //public class RoundTrip : Tests.RoundTrip
    //{
    //    public RoundTrip() : base(s => new Integration.Migrators.MigratorDotNet(s)) { }
    //}
}