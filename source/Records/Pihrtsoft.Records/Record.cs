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
            Properties = new Dictionary<string, object>() { [PropertyDefinition.TagsName] = Tags };

            if (id != null)
                Properties[PropertyDefinition.IdName] = id;
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
                if (TryGetValue(PropertyDefinition.IdName, out object value))
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

        public string[] GetTags()
        {
            return Tags.ToArray();
        }

        internal TagCollection Tags { get; }

        public bool ContainsProperty(string propertyName)
        {
            return Properties.ContainsKey(propertyName);
        }

        internal bool TryGetValue(string propertyName, out object value)
        {
            return Properties.TryGetValue(propertyName, out value);
        }

        internal bool TryGetCollection(string propertyName, out List<object> values)
        {
            if (Properties.TryGetValue(propertyName, out object value))
            {
                values = (List<object>)value;
                return true;
            }

            values = null;
            return false;
        }

        internal List<object> GetOrAddCollection(string propertyName)
        {
            if (TryGetValue(propertyName, out object value))
            {
                return (List<object>)value;
            }
            else
            {
                var items = new List<object>();
                this[propertyName] = items;
                return items;
            }
        }

        private string DebuggerDisplay
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append(Entity.Name);

                if (Id != null)
                {
                    sb.Append(" Id: ");
                    sb.Append(Id);
                }

                if (Tags.Count > 0)
                {
                    sb.Append(" Tags: ");
                    sb.Append(string.Join(", ", Tags.OrderBy(f => f)));
                }

                IEnumerable<string> properties = Properties
                    .Where(f => !PropertyDefinition.IsReservedName(f.Key))
                    .Select(f =>
                    {
                        if (f.Value is List<object> list)
                        {
                            return $"{f.Key} = {{{string.Join(", ", list)}}}";
                        }
                        else
                        {
                            return $"{f.Key} = {f.Value}";
                        }
                    });

                string s = string.Join(", ", properties);

                if (s.Length > 0)
                {
                    sb.Append(" ");
                    sb.Append(s);
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
            if (Properties.TryGetValue(propertyName, out object value))
            {
                return (string)value;
            }

            return defaultValue;
        }

        public bool GetBooleanOrDefault(string propertyName, bool defaultValue = default(bool))
        {
            if (Properties.TryGetValue(propertyName, out object value))
            {
                return bool.Parse((string)value);
            }

            return defaultValue;
        }

        public decimal GetDecimalOrDefault(string propertyName, decimal defaultValue = default(decimal))
        {
            if (Properties.TryGetValue(propertyName, out object value))
            {
                return decimal.Parse((string)value);
            }

            return defaultValue;
        }

        public double GetDoubleOrDefault(string propertyName, double defaultValue = default(double))
        {
            if (Properties.TryGetValue(propertyName, out object value))
            {
                return double.Parse((string)value);
            }

            return defaultValue;
        }

        public float GetFloatOrDefault(string propertyName, float defaultValue = default(float))
        {
            if (Properties.TryGetValue(propertyName, out object value))
            {
                return float.Parse((string)value);
            }

            return defaultValue;
        }

        public int GetIntOrDefault(string propertyName, int defaultValue = default(int))
        {
            if (Properties.TryGetValue(propertyName, out object value))
            {
                return int.Parse((string)value);
            }

            return defaultValue;
        }

        public long GetLongOrDefault(string propertyName, long defaultValue = default(long))
        {
            if (Properties.TryGetValue(propertyName, out object value))
            {
                return long.Parse((string)value);
            }

            return defaultValue;
        }

        public DateTime GetDateTimeOrDefault(string propertyName, DateTime defaultValue = default(DateTime))
        {
            if (Properties.TryGetValue(propertyName, out object value))
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
            if (Properties.TryGetValue(propertyName, out object value)
                && (value is List<object> list))
            {
                var items = new TItem[list.Count];

                for (int i = 0; i < list.Count; i++)
                    items[i] = (TItem)list[i];

                return items;
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

        string IKey<string>.GetKey()
        {
            return Id;
        }
    }
}
