// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Xml;
using static Snippetica.CodeGeneration.CodeGenerationUtility;
using static Snippetica.KnownNames;
using static Snippetica.KnownPaths;

namespace Snippetica.CodeGeneration.VisualStudio
{
    public static class HtmlGenerator
    {
        public static string GenerateVisualStudioMarketplaceDescription(IEnumerable<SnippetGeneratorResult> results)
        {
            using (var sw = new StringWriter())
            {
                var xmlWriterSettings = new XmlWriterSettings()
                {
                    Indent = true,
                    IndentChars = "  ",
                    ConformanceLevel = ConformanceLevel.Fragment
                };

                using (XmlWriter x = XmlWriter.Create(sw, xmlWriterSettings))
                {
                    x.WriteElementString("h3", ProductName);
                    x.WriteElementString("p", GetProjectSubtitle(results));

                    x.WriteElementString("h3", "Links");

                    x.WriteStartElement("ul");

                    x.WriteStartElement("li");
                    x.WriteStartElement("a");
                    x.WriteAttributeString("href", GitHubUrl);
                    x.WriteString("Project Website");
                    x.WriteEndElement();
                    x.WriteEndElement();

                    x.WriteStartElement("li");
                    x.WriteStartElement("a");
                    x.WriteAttributeString("href", $"{MasterGitHubUrl}/{ChangeLogFileName}");
                    x.WriteString("Release Notes");
                    x.WriteEndElement();
                    x.WriteEndElement();

                    x.WriteStartElement("li");
                    x.WriteString("Browse all available snippets with ");
                    x.WriteStartElement("a");
                    x.WriteAttributeString("href", GetSnippetBrowserUrl(EnvironmentKind.VisualStudio));
                    x.WriteString("Snippet Browser");
                    x.WriteEndElement();
                    x.WriteEndElement();

                    x.WriteEndElement();

                    x.WriteElementString("h3", "Snippets");
                    x.WriteStartElement("ul");

                    foreach (SnippetGeneratorResult result in results)
                    {
                        string directoryName = result.DirectoryName;

                        x.WriteStartElement("li");

                        x.WriteStartElement("a");
                        x.WriteAttributeString("href", $"{VisualStudioExtensionGitHubUrl}/{directoryName}/{ReadMeFileName}");
                        x.WriteString(directoryName);
                        x.WriteEndElement();
                        x.WriteString($" ({result.Snippets.Count} snippets)");

                        x.WriteString(" (");
                        x.WriteStartElement("a");
                        x.WriteAttributeString("href", GetSnippetBrowserUrl(EnvironmentKind.VisualStudio, result.Language));
                        x.WriteString("full list");
                        x.WriteEndElement();
                        x.WriteString(")");

                        x.WriteEndElement();
                    }

                    x.WriteEndElement();
                }

                return sw.ToString();
            }
        }
    }
}
