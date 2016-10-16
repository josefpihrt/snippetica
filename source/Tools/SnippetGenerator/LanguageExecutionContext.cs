using System.Collections.ObjectModel;

namespace Pihrtsoft.Snippets.CodeGeneration
{
    public class LanguageExecutionContext : ExecutionContext
    {
        public LanguageExecutionContext(Snippet snippet, LanguageDefinition language)
            : base(snippet)
        {
            Language = language;
        }

        public LanguageDefinition Language { get; }
    }
}