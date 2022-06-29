using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;


namespace EasyMigrator.Parsing
{
    public interface ITypeMap
    {
        DbType this[PropertyInfo field] { get; }
        ITypeMap Add(Type underlyingType, DbType dbType);
        ITypeMap Add(IEnumerable<Type> underlyingTypes, DbType dbType);
        ITypeMap Add(IDictionary<Type, DbType> map);
        ITypeMap Add(IDictionary<IEnumerable<Type>, DbType> map);
        ITypeMap Add(Type underlyingType, Func<PropertyInfo, DbType> dbTypeProvider);
        ITypeMap Add(IEnumerable<Type> underlyingTypes, Func<PropertyInfo, DbType> dbTypeProvider);
        ITypeMap Add(IDictionary<Type, Func<PropertyInfo, DbType>> providerMap);
        ITypeMap Add(IDictionary<IEnumerable<Type>, Func<PropertyInfo, DbType>> providerMap);

        ITypeMap Clone();

        ITypeMap Remove(Type underlyingType);

        ITypeMap Replace(Type underlyingType, DbType dbType);
        ITypeMap Replace(Type underlyingType, Func<PropertyInfo, DbType> dbTypeProvider);
    }

    public class TypeMap : ITypeMap
    {
        private class ProviderPair
        {
            private readonly DbType _dbType;
            private readonly Func<PropertyInfo, DbType> _dbTypeProvider = null;

            public ProviderPair(DbType dbType) { _dbType = dbType; }
            public ProviderPair(Func<PropertyInfo, DbType> dbTypeProvider) { _dbTypeProvider = dbTypeProvider; }

            public DbType GetDbType(PropertyInfo field) { return _dbTypeProvider == null ? _dbType : _dbTypeProvider(field); }
        }

        private readonly Dictionary<Type, ProviderPair> _map;

        public TypeMap() : this(new Dictionary<Type, ProviderPair>()) { }
        private TypeMap(Dictionary<Type, ProviderPair> map) { _map = map; }

        public DbType this[PropertyInfo field]
        {
            get {
                var type = Nullable.GetUnderlyingType(field.PropertyType) ?? field.PropertyType;
                if (!_map.ContainsKey(type))
                    throw new Exception("No DbType mapped to native type " + type.Name);

                return _map[type].GetDbType(field);
            }
        }

        public ITypeMap Clone() => new TypeMap(new Dictionary<Type, ProviderPair>(_map));

        public ITypeMap Add(Type underlyingType, DbType dbType) { Add(underlyingType, new ProviderPair(dbType)); return this; }
        public ITypeMap Add(IEnumerable<Type> underlyingTypes, DbType dbType) { foreach (var underlyingType in underlyingTypes) Add(underlyingType, dbType); return this; }
        public ITypeMap Add(IDictionary<Type, DbType> map) { foreach (var kv in map) Add(kv.Key, kv.Value); return this; }
        public ITypeMap Add(IDictionary<IEnumerable<Type>, DbType> map) { foreach (var kv in map) Add(kv.Key, kv.Value); return this; }
        public ITypeMap Add(Type underlyingType, Func<PropertyInfo, DbType> dbTypeProvider) { Add(underlyingType, new ProviderPair(dbTypeProvider)); return this; }
        public ITypeMap Add(IEnumerable<Type> underlyingTypes, Func<PropertyInfo, DbType> dbTypeProvider) { foreach (var underlyingType in underlyingTypes) Add(underlyingType, dbTypeProvider); return this; }
        public ITypeMap Add(IDictionary<Type, Func<PropertyInfo, DbType>> providerMap) { foreach (var kv in providerMap) Add(kv.Key, kv.Value); return this; }
        public ITypeMap Add(IDictionary<IEnumerable<Type>, Func<PropertyInfo, DbType>> providerMap) { foreach (var kv in providerMap) Add(kv.Key, kv.Value); return this; }

        private void Add(Type underlyingType, ProviderPair providerPair)
        {
            if (_map.ContainsKey(underlyingType))
                throw new Exception("Native type '" + underlyingType.Name + "' is already mapped.");
            _map.Add(underlyingType, providerPair);
        }


        public ITypeMap Remove(Type underlyingType)
        {
            if (_map.ContainsKey(underlyingType))
                _map.Remove(underlyingType);
            return this;
        }

        public ITypeMap Replace(Type underlyingType, DbType dbType) { Replace(underlyingType, new ProviderPair(dbType)); return this; }
        public ITypeMap Replace(Type underlyingType, Func<PropertyInfo, DbType> dbTypeProvider) { Replace(underlyingType, new ProviderPair(dbTypeProvider)); return this; }

        private void Replace(Type underlyingType, ProviderPair providerPair)
        {
            if (_map.ContainsKey(underlyingType))
                _map[underlyingType] = providerPair;
            else
                _map.Add(underlyingType, providerPair);
        }
    }
}
