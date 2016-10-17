using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Pihrtsoft.Snippets.CodeGeneration
{
    public class CharacterSequence
    {
        public CharacterSequence(
            string value,
            string description,
            string comment,
            IEnumerable<string> directoryNames,
            IEnumerable<string> tags)
        {
            Value = value;
            Description = description;
            Comment = comment;
            DirectoryNames = new ReadOnlyCollection<string>(directoryNames.ToArray());
            Tags = new ReadOnlyCollection<string>(tags.ToArray());
        }

        public string Value { get; }
        public string Description { get; }
        public string Comment { get; }
        public ReadOnlyCollection<string> DirectoryNames { get; }
        public ReadOnlyCollection<string> Tags { get; }
    }
}
