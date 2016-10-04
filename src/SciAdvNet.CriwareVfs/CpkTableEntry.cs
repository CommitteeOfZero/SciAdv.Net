using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace SciAdvNet.CriwareVfs
{
    public abstract class CpkTableEntry
    {
        protected readonly ImmutableDictionary<string, object> _extraFields;

        internal CpkTableEntry(ImmutableDictionary<string, object> entryFields)
        {
            var bldExtraFields = ImmutableDictionary.CreateBuilder<string, object>();
            var typeInfo = GetType().GetTypeInfo();
            var map = (from prop in typeInfo.DeclaredProperties
                       let propAttribute = prop.GetCustomAttribute(typeof(CpkTableFieldAttribute)) as CpkTableFieldAttribute
                       let tocFieldName = propAttribute?.CpkTableFieldName ?? prop.Name
                       select new { Property = prop, TocFieldName = tocFieldName })
                       .ToDictionary(x => x.TocFieldName, x => x.Property);

            foreach (var field in entryFields)
            {
                PropertyInfo prop;
                if (map.TryGetValue(field.Key, out prop))
                {
                    prop.SetValue(this, Convert.ChangeType(field.Value, prop.PropertyType));
                }
                else
                {
                    bldExtraFields.Add(field.Key, field.Value);
                }
            }

            _extraFields = bldExtraFields.ToImmutable();
        }
    }
}
