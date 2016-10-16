using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Pihrtsoft.Records
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Record : IKey<string>
    {
        internal Record(EntityDefinition entity, string id = null)
        {
            Entity = entity;
            Tags = new TagCollection();
            Properties = new Dictionary<string, object>();

            if (id != null)
                Properties[PropertyDefinition.Id.Name] = id;
        }

        public EntityDefinition Entity { get; }

        public string EntityName
        {
            get { return Entity?.Name; }
        }

        public string Id
        {
            get { return (string)FindValue(PropertyDefinition.Id.Name); }
        }

        public Dictionary<string, object> Properties { get; }

        public TagCollection Tags { get; }

        public bool ContainsProperty(string propertyName)
        {
            return Properties.ContainsKey(propertyName);
        }

        public object FindValue(string propertyName)
        {
            object value;
            if (Properties.TryGetValue(propertyName, out value))
                return value;

            return null;
        }

        private string DebuggerDisplay
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append(Entity.Name);

                if (Id != null)
                    sb.Append($" Id: {Id}");

                if (Tags.Count > 0)
                    sb.Append($" Tags: {string.Join(", ", Tags.OrderBy(f => f))}");

                if (Properties.Count > 0)
                {
                    sb.Append(" ");

                    IEnumerable<string> properties = Properties.Select(f =>
                    {
                        var list = f.Value as List<object>;

                        if (list != null)
                        {
                            return $"{f.Key} = {{{string.Join(", ", list)}}}";
                        }

                        return $"{f.Key} = {f.Value}";
                    });

                    sb.Append(string.Join(", ", properties));
                }

                return sb.ToString();
            }
        }

        public object this[string propertyName]
        {
            get { return Properties[propertyName]; }
            set { Properties[propertyName] = value; }
        }

        public bool HasTag(string tag)
        {
            return Tags.Contains(tag);
        }

        public bool HasTag(params string[] tags)
        {
            return Tags.ContainsAny(tags);
        }

        public bool HasTags(params string[] tags)
        {
            return Tags.ContainsAll(tags);
        }

        public string GetStringOrDefault(string propertyName, string defaultValue = default(string))
        {
            object value;
            if (Properties.TryGetValue(propertyName, out value))
            {
                return (string)value;
            }

            return defaultValue;
        }

        public bool GetBooleanOrDefault(string propertyName, bool defaultValue = default(bool))
        {
            object value;
            if (Properties.TryGetValue(propertyName, out value))
            {
                return bool.Parse((string)value);
            }

            return defaultValue;
        }

        public byte GetByteOrDefault(string propertyName, byte defaultValue = default(byte))
        {
            object value;
            if (Properties.TryGetValue(propertyName, out value))
            {
                return byte.Parse((string)value);
            }

            return defaultValue;
        }

        public sbyte GetSByteOrDefault(string propertyName, sbyte defaultValue = default(sbyte))
        {
            object value;
            if (Properties.TryGetValue(propertyName, out value))
            {
                return sbyte.Parse((string)value);
            }

            return defaultValue;
        }

        public decimal GetDecimalOrDefault(string propertyName, decimal defaultValue = default(decimal))
        {
            object value;
            if (Properties.TryGetValue(propertyName, out value))
            {
                return decimal.Parse((string)value);
            }

            return defaultValue;
        }

        public double GetDoubleOrDefault(string propertyName, double defaultValue = default(double))
        {
            object value;
            if (Properties.TryGetValue(propertyName, out value))
            {
                return double.Parse((string)value);
            }

            return defaultValue;
        }

        public float GetSingleOrDefault(string propertyName, float defaultValue = default(float))
        {
            object value;
            if (Properties.TryGetValue(propertyName, out value))
            {
                return float.Parse((string)value);
            }

            return defaultValue;
        }

        public int GetInt32OrDefault(string propertyName, int defaultValue = default(int))
        {
            object value;
            if (Properties.TryGetValue(propertyName, out value))
            {
                return int.Parse((string)value);
            }

            return defaultValue;
        }

        public uint GetUInt32OrDefault(string propertyName, uint defaultValue = default(uint))
        {
            object value;
            if (Properties.TryGetValue(propertyName, out value))
            {
                return uint.Parse((string)value);
            }

            return defaultValue;
        }

        public long GetInt64OrDefault(string propertyName, long defaultValue = default(long))
        {
            object value;
            if (Properties.TryGetValue(propertyName, out value))
            {
                return long.Parse((string)value);
            }

            return defaultValue;
        }

        public ulong GetUInt64OrDefault(string propertyName, ulong defaultValue = default(ulong))
        {
            object value;
            if (Properties.TryGetValue(propertyName, out value))
            {
                return ulong.Parse((string)value);
            }

            return defaultValue;
        }

        public short GetInt16OrDefault(string propertyName, short defaultValue = default(short))
        {
            object value;
            if (Properties.TryGetValue(propertyName, out value))
            {
                return short.Parse((string)value);
            }

            return defaultValue;
        }

        public ushort GetUInt16OrDefault(string propertyName, ushort defaultValue = default(ushort))
        {
            object value;
            if (Properties.TryGetValue(propertyName, out value))
            {
                return ushort.Parse((string)value);
            }

            return defaultValue;
        }

        public string GetString(string propertyName)
        {
            return (string)Properties[propertyName];
        }

        public bool GetBoolean(string propertyName)
        {
            return bool.Parse((string)Properties[propertyName]);
        }

        public byte GetByte(string propertyName)
        {
            return byte.Parse((string)Properties[propertyName]);
        }

        public sbyte GetSByte(string propertyName)
        {
            return sbyte.Parse((string)Properties[propertyName]);
        }

        public decimal GetDecimal(string propertyName)
        {
            return decimal.Parse((string)Properties[propertyName]);
        }

        public double GetDouble(string propertyName)
        {
            return double.Parse((string)Properties[propertyName]);
        }

        public float GetSingle(string propertyName)
        {
            return float.Parse((string)Properties[propertyName]);
        }

        public int GetInt32(string propertyName)
        {
            return int.Parse((string)Properties[propertyName]);
        }

        public uint GetUInt32(string propertyName)
        {
            return uint.Parse((string)Properties[propertyName]);
        }

        public long GetInt64(string propertyName)
        {
            return long.Parse((string)Properties[propertyName]);
        }

        public ulong GetUInt64(string propertyName)
        {
            return ulong.Parse((string)Properties[propertyName]);
        }

        public short GetInt16(string propertyName)
        {
            return short.Parse((string)Properties[propertyName]);
        }

        public ushort GetUInt16(string propertyName)
        {
            return ushort.Parse((string)Properties[propertyName]);
        }

        public object[] GetItems(string propertyName)
        {
            object value = Properties[propertyName];

            var list = (List<object>)value;

            return list.ToArray();
        }

        public TItem[] GetItems<TItem>(string propertyName, TItem[] defaultValue = default(TItem[]))
        {
            object value = Properties[propertyName];

            var list = (List<object>)value;

            var items = new TItem[list.Count];

            for (int i = 0; i < list.Count; i++)
                items[i] = (TItem)list[i];

            return items;
        }

        public object[] GetItemsOrDefault(string propertyName)
        {
            return GetItemsOrDefault<object>(propertyName);
        }

        public TItem[] GetItemsOrDefault<TItem>(string propertyName, TItem[] defaultValue = default(TItem[]))
        {
            object value;
            if (Properties.TryGetValue(propertyName, out value))
            {
                var list = value as List<object>;

                if (list != null)
                {
                    var items = new TItem[list.Count];

                    for (int i = 0; i < list.Count; i++)
                        items[i] = (TItem)list[i];

                    return items;
                }
            }

            return defaultValue;
        }

        public TEnum GetEnum<TEnum>(string propertyName) where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), GetString(propertyName));
        }

        public TEnum GetEnumOrDefault<TEnum>(string propertyName, TEnum defaultValue = default(TEnum)) where TEnum : struct
        {
            string value = GetStringOrDefault(propertyName);

            if (value != null)
                return (TEnum)Enum.Parse(typeof(TEnum), value);

            return defaultValue;
        }

        internal Record WithEntity(EntityDefinition entity)
        {
            var record = new Record(entity, Id);

            foreach (KeyValuePair<string, object> pair in Properties)
                record[pair.Key] = pair.Value;

            foreach (string tag in Tags)
                record.Tags.Add(tag);

            return record;
        }

        public string GetKey()
        {
            return Id;
        }
    }
}
