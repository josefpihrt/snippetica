// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
            get
            {
                object value;
                if (TryGetValue(PropertyDefinition.Id.Name, out value))
                {
                    return (string)value;
                }
                else
                {
                    return null;
                }
            }
        }

        private Dictionary<string, object> Properties { get; }

#if DEBUG
        public IEnumerable<KeyValuePair<string, object>> GetProperties()
        {
            foreach (KeyValuePair<string, object> kvp in Properties)
                yield return kvp;
        }
#endif

        public TagCollection Tags { get; }

        public bool ContainsProperty(string propertyName)
        {
            return Properties.ContainsKey(propertyName);
        }

        internal object GetValue(string propertyName)
        {
            return Properties[propertyName];
        }

        internal bool TryGetValue(string propertyName, out object value)
        {
            return Properties.TryGetValue(propertyName, out value);
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

        internal object this[string propertyName]
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

        public float GetFloatOrDefault(string propertyName, float defaultValue = default(float))
        {
            object value;
            if (Properties.TryGetValue(propertyName, out value))
            {
                return float.Parse((string)value);
            }

            return defaultValue;
        }

        public int GetIntOrDefault(string propertyName, int defaultValue = default(int))
        {
            object value;
            if (Properties.TryGetValue(propertyName, out value))
            {
                return int.Parse((string)value);
            }

            return defaultValue;
        }

        public long GetLongOrDefault(string propertyName, long defaultValue = default(long))
        {
            object value;
            if (Properties.TryGetValue(propertyName, out value))
            {
                return long.Parse((string)value);
            }

            return defaultValue;
        }

        public DateTime GetDateTimeOrDefault(string propertyName, DateTime defaultValue = default(DateTime))
        {
            object value;
            if (Properties.TryGetValue(propertyName, out value))
            {
                return DateTime.Parse((string)value);
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

        public decimal GetDecimal(string propertyName)
        {
            return decimal.Parse((string)Properties[propertyName]);
        }

        public double GetDouble(string propertyName)
        {
            return double.Parse((string)Properties[propertyName]);
        }

        public float GetFloat(string propertyName)
        {
            return float.Parse((string)Properties[propertyName]);
        }

        public int GetInt(string propertyName)
        {
            return int.Parse((string)Properties[propertyName]);
        }

        public long GetLong(string propertyName)
        {
            return long.Parse((string)Properties[propertyName]);
        }

        public DateTime GetDateTime(string propertyName)
        {
            return DateTime.Parse((string)Properties[propertyName]);
        }

        public string[] GetItems(string propertyName)
        {
            return GetItems<string>(propertyName);
        }

        private TItem[] GetItems<TItem>(string propertyName)
        {
            object value = Properties[propertyName];

            var list = (List<object>)value;

            var items = new TItem[list.Count];

            for (int i = 0; i < list.Count; i++)
                items[i] = (TItem)list[i];

            return items;
        }

        public string[] GetItemsOrDefault(string propertyName, string[] defaultValue = default(string[]))
        {
            return GetItemsOrDefault<string>(propertyName, defaultValue);
        }

        private TItem[] GetItemsOrDefault<TItem>(string propertyName, TItem[] defaultValue = default(TItem[]))
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
