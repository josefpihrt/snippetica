using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Pihrtsoft.Snippets.Xml
{
    public class ProjectDocument
    {
        public const string CSharpProjectExtension = "csproj";
        public const string VisualBasicProjectExtension = "vbproj";

        public ProjectDocument(string filePath)
        {
            FilePath = filePath;

            Document = XDocument.Load(filePath);
        }

        public XElement Project
        {
            get
            {
                return Document
                    .Elements()
                    .FirstOrDefault(f => f.LocalName() == "Project");
            }
        }

        public XElement LastItemGroupOrDefault
        {
            get
            {
                return Project
                    .Elements()
                    .LastOrDefault(f => f.LocalName() == "ItemGroup");
            }
        }

        public string FilePath { get; }

        public XDocument Document { get; }

        public void Save()
        {
            Document.Save(FilePath);
        }

        public IEnumerable<XElement> ItemGroups()
        {
            return Project
                .Elements()
                .Where(f => f.LocalName() == "ItemGroup");
        }

        public IEnumerable<XElement> GetReferencedSnippetFiles(XElement itemGroup)
        {
            foreach (XElement element in itemGroup.Elements())
            {
                if (element.LocalName() == "Content"
                    && element
                        .Attributes()
                        .Any(f => f.LocalName() == "Include" && f.Value.EndsWith(".snippet")))
                {
                    yield return element;
                }
            }
        }

        public void RemoveSnippetFiles()
        {
            foreach (XElement itemGroup in ItemGroups())
            {
                XElement[] items = GetReferencedSnippetFiles(itemGroup).ToArray();

                for (int i = 0; i < items.Length; i++)
                    items[i].Remove();

                if (!itemGroup.HasElements)
                    itemGroup.Remove();
            }
        }

        public XElement AddItemGroup()
        {
            var itemGroup = new XElement(Project.Name.Namespace + "ItemGroup");

            XElement lastItemGroup = LastItemGroupOrDefault;

            if (lastItemGroup != null)
            {
                lastItemGroup.AddAfterSelf(itemGroup);
            }
            else
            {
                Project.Add(itemGroup);
            }

            return itemGroup;
        }

        public void AddSnippetFiles(IEnumerable<string> paths, XElement itemGroup)
        {
            foreach (string path in paths)
                AddSnippetFile(path, itemGroup);
        }

        public void AddSnippetFile(string path, XElement itemGroup)
        {
            string relativePath = path
                .Replace(Path.GetDirectoryName(FilePath), string.Empty)
                .TrimStart(Path.DirectorySeparatorChar);

            XNamespace ns = itemGroup.Name.Namespace;

            itemGroup.Add(new XElement(ns + "Content",
                new XAttribute("Include", relativePath),
                new XElement(ns + "IncludeInVSIX", "true")));
        }
    }
}
