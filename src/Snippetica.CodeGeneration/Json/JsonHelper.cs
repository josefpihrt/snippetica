// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Snippetica.CodeGeneration.Json
{
    internal static class JsonHelper
    {
        public static void AddProperty(JObject json, string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
                json[name] = value;
        }

        public static void AddProperty(JObject json, JProperty property)
        {
            if (property != null)
                json.Add(property);
        }

        public static void AddList<T>(JObject json, string name, List<T> list)
        {
            if (list?.Count > 0)
                json[name] = new JArray(list.ToArray());
        }
    }
}
