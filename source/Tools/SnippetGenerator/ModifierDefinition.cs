using System;
using System.Collections.ObjectModel;

namespace Pihrtsoft.Snippets.CodeGeneration
{
    public class ModifierDefinition
    {
        public ModifierDefinition(string name, string keyword, string shortcut, string[] tags)
        {
            Name = name;
            Keyword = keyword;
            Shortcut = shortcut;
            Tags = new ReadOnlyCollection<string>(tags);
            Kind = (ModifierKind)Enum.Parse(typeof(ModifierKind), Name);
        }

        public string Name { get; }
        public string Keyword { get; }
        public string Shortcut { get; }
        public ReadOnlyCollection<string> Tags { get; }
        public ModifierKind Kind { get; }
    }
}
