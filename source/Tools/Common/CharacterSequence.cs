using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Pihrtsoft.Records;

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

        public static IEnumerable<CharacterSequence> LoadFromFile(string uri, string languagePrefix)
        {
            return Document.ReadRecords(uri)
                .Where(f => !f.HasTag(KnownTags.Disabled))
                .Select(record =>
                {
                    return new CharacterSequence(
                        record.GetString("Value"),
                        record.GetString("Description"),
                        record.GetStringOrDefault("Comment", "-"),
                        record.GetItems("Languages").Select(f => languagePrefix + f),
                        record.Tags);
                });
        }
    }
}
