using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


namespace EasyMigrator
{
    [Obsolete("This attribute is for legacy code that is migrating. Use object types or nullable value types for a nullable field, and use NotNullAttribute to override object types for a non-nullable field")]
    [AttributeUsage(AttributeTargets.Property)] public class NullAttribute : Attribute { }
    [AttributeUsage(AttributeTargets.Property)] public class NotNullAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class NameAttribute : Attribute
    {
        public string Name { get; }
        public NameAttribute(string name) { Name = name; }
    }
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class SchemaAttribute : Attribute
    {
        public string Name { get; }
        public SchemaAttribute(string name) { Name = name; }
    }    

    [AttributeUsage(AttributeTargets.Property)]
    public class DbTypeAttribute : Attribute
    {
        public string CustomType { get; }
        public DbType? DbType { get; }
        public DbTypeAttribute(string customType) { CustomType = customType; }
        public DbTypeAttribute(DbType dbType) { DbType = dbType; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class NoPkAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class PkAttribute : Attribute
    {
        public string Name { get; }
        public bool Clustered { get; set; } = true;

        public PkAttribute() { }
        public PkAttribute(string name) { Name = name; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class AutoIncAttribute : Attribute, Parsing.Model.IAutoIncrement
    {
        public long Seed { get; }
        public long Step { get; }

        public AutoIncAttribute() : this(1) { }
        public AutoIncAttribute(long seed) : this(seed, 1) { }
        public AutoIncAttribute(long seed, long step) { Seed = seed; Step = step; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class PrecisionAttribute : Attribute, Parsing.Model.IPrecision
    {
        internal Length? DefinedPrecision { get; }
        public int Precision { get; }
        internal Length? DefinedScale { get; }
        public int Scale { get; }

        public PrecisionAttribute(int precision, int scale)
            : this(precision) { Scale = scale; }

        public PrecisionAttribute(Length precision, int scale)
            : this(precision) { Scale = scale; }

        public PrecisionAttribute(Length precision, Length scale)
            : this(precision) { DefinedScale = scale; }

        public PrecisionAttribute(int precision) { Precision = precision; }

        public PrecisionAttribute(Length precision) { DefinedPrecision = precision; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class FkAttribute : Attribute, Parsing.Model.IForeignKey
    {
        public string Name { get; set; }
        public string Table { get; }
        public Type TableType { get; }
        public string Column { get; set; }

        internal bool OnDeleteWasSet { get; set; }
        private Rule? _onDelete;
        public Rule OnDelete {
            get => _onDelete ?? Rule.None;
            set {
                _onDelete = value;
                OnDeleteWasSet = true;
            }
        }
        Rule? Parsing.Model.IForeignKey.OnDelete {
            get => _onDelete;
            set {
                _onDelete = value;
                OnDeleteWasSet = true;
            }
        }

        internal bool OnUpdateWasSet { get; set; }
        private Rule? _onUpdate;
        public Rule OnUpdate {
            get => _onUpdate ?? Rule.None;
            set {
                _onUpdate = value;
                OnUpdateWasSet = true;
            }
        }
        Rule? Parsing.Model.IForeignKey.OnUpdate {
            get => _onUpdate;
            set {
                _onUpdate = value;
                OnUpdateWasSet = true;
            }
        }

        internal bool IndexedWasSet { get; set; }
        private bool _indexed;
        public bool Indexed {
            get => _indexed;
            set {
                _indexed = value;
                IndexedWasSet = true;
            }
        }

        public FkAttribute(string table) { Table = table; }
        public FkAttribute(Type tableType) { TableType = tableType; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class IndexAttribute : Attribute
    {
        public string Name { get; set; }
        public bool Unique { get; protected set; }
        public bool Clustered { get; protected set; }
        public string Where { get; set; }
        public string With { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class UniqueAttribute : IndexAttribute
    {
        public UniqueAttribute() { Unique = true; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ClusteredAttribute : UniqueAttribute
    {
        public ClusteredAttribute() { Clustered = true; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class AnsiAttribute : Attribute { }

    public enum Length { Default, Short, Medium, Long, Max }

    [AttributeUsage(AttributeTargets.Property)]
    public class LengthAttribute : Attribute
    {
        internal Length? DefinedLength { get; }
        public int Length { get; }

        public LengthAttribute(int length) { Length = length; }
        public LengthAttribute(Length length) { DefinedLength = length; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ShortAttribute : LengthAttribute { public ShortAttribute() : base(EasyMigrator.Length.Short) { } }
    [AttributeUsage(AttributeTargets.Property)]
    public class MediumAttribute : LengthAttribute { public MediumAttribute() : base(EasyMigrator.Length.Medium) { } }
    [AttributeUsage(AttributeTargets.Property)]
    public class LongAttribute : LengthAttribute { public LongAttribute() : base(EasyMigrator.Length.Long) { } }
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxAttribute : LengthAttribute { public MaxAttribute() : base(EasyMigrator.Length.Max) { } }

    [AttributeUsage(AttributeTargets.Property)]
    public class FixedAttribute : LengthAttribute
    {
        public FixedAttribute(int length) : base(length) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultAttribute : Attribute
    {
        public string Expression { get; }
        public DefaultAttribute(string expression) { Expression = expression; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class SparseAttribute : Attribute { }
}
