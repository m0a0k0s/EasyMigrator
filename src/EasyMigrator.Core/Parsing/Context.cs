using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using EasyMigrator.Parsing.Model;


namespace EasyMigrator.Parsing
{
    public class Context
    {
        public Conventions Conventions { get; set; }
        public Table Table { get; set; }
        public Type ModelType { get; set; }
        public IEnumerable<PropertyInfo> ColumnFields { get; set; }
        public IDictionary<PropertyInfo, Column> Columns { get; set; } = new Dictionary<PropertyInfo, Column>();
        public object Model { get; set; }
    }
}
