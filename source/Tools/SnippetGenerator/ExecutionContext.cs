using System.Collections.ObjectModel;

namespace Pihrtsoft.Snippets.CodeGeneration
{
    public class ExecutionContext
    {
        public ExecutionContext(Snippet snippet)
        {
            Snippets = new Collection<Snippet>();
            Snippets.Add(snippet);
        }

        public bool IsCanceled { get; set; }

        public Collection<Snippet> Snippets { get; }
    }
}