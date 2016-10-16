using System.Collections.ObjectModel;

namespace Pihrtsoft.Snippets.CodeGeneration
{
    public class SnippetGeneratorSettings
    {
        public SnippetGeneratorSettings(LanguageDefinition language)
        {
            Language = language;

            Types = new Collection<TypeDefinition>();
        }

        public LanguageDefinition Language { get; }

        public Collection<TypeDefinition> Types { get; }
    }
}
