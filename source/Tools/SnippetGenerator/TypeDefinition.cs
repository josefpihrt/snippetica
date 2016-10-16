using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Pihrtsoft.Snippets.CodeGeneration
{
    [DebuggerDisplay("{Name,nq}")]
    public  class TypeDefinition
    {
        public TypeDefinition(
            string name,
            string title,
            string keyword,
            string shortcut,
            string defaultValue,
            string defaultIdentifier,
            string @namespace,
            string[] tags)
        {
            Name = name;
            Title = title;
            Keyword = keyword;
            Shortcut = shortcut;
            DefaultValue = defaultValue;
            DefaultIdentifier = defaultIdentifier;
            Namespace = @namespace;
            Tags = new ReadOnlyCollection<string>(tags);
        }

        public string Name { get; }
        public string Title { get; }
        public string Keyword { get; }
        public string Shortcut { get; }
        public string DefaultValue { get; }
        public string DefaultIdentifier { get; }
        public string Namespace { get; }
        public ReadOnlyCollection<string> Tags { get; }
    }
}
