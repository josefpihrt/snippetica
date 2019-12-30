// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Newtonsoft.Json.Linq;
using static Snippetica.CodeGeneration.Json.JsonHelper;

namespace Snippetica.CodeGeneration.Json.Package
{
    public class BugInfo
    {
        public string Url { get; set; }

        public string Email { get; set; }

        public JProperty ToJProperty()
        {
            var o = new JObject();

            AddProperty(o, "url", Url);
            AddProperty(o, "email", Email);

            return new JProperty("bugs", o);
        }
    }
}
