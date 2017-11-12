// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Snippetica.CodeGeneration.Json.JsonHelper;

namespace Snippetica.CodeGeneration.Json.Package
{
    // http://docs.npmjs.com/files/package.json
    // http://code.visualstudio.com/docs/extensionAPI/extension-manifest

    public class PackageInfo
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Version { get; set; }

        public string Publisher { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public string EngineVersion { get; set; }

        public string Icon { get; set; }

        public string License { get; set; }

        public List<string> Categories { get; } = new List<string>();

        public List<string> Keywords { get; } = new List<string>();

        public string Homepage { get; set; }

        public RepositoryInfo Repository { get; set; }

        public BugInfo Bugs { get; set; }

        public List<SnippetInfo> Snippets { get; set; } = new List<SnippetInfo>();

        public JObject ToJson()
        {
            var o = new JObject();

            AddProperty(o, "name", Name);
            AddProperty(o, "publisher", Publisher);
            AddProperty(o, "displayName", DisplayName);
            AddProperty(o, "description", Description);
            AddProperty(o, "icon", Icon);
            AddProperty(o, "version", Version);
            AddProperty(o, "author", Author);
            AddProperty(o, "license", License);
            AddProperty(o, "homepage", Homepage);

            AddProperty(o, Repository?.ToJProperty());
            AddProperty(o, Bugs?.ToJProperty());

            AddList(o, "categories", Categories);
            AddList(o, "keywords", Keywords);

            if (!string.IsNullOrEmpty(EngineVersion))
            {
                o["engines"] = new JObject(new JProperty("vscode", EngineVersion));
            }

            if (Snippets.Count > 0)
            {
                var snippets = new JArray();

                foreach (SnippetInfo snippetInfo in Snippets)
                {
                    snippets.Add(
                        new JObject(
                            new JProperty("language", snippetInfo.Language),
                            new JProperty("path", snippetInfo.Path)));
                }

                o["contributes"] = new JObject() { ["snippets"] = snippets };
            }

            return o;
        }

        public override string ToString()
        {
            return ToJson().ToString(Formatting.Indented);
        }
    }
}
