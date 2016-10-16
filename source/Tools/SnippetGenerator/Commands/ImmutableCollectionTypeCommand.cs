using System.IO;

namespace Pihrtsoft.Snippets.CodeGeneration.Commands
{
    public class ImmutableCollectionTypeCommand : CollectionTypeCommand
    {
        public ImmutableCollectionTypeCommand(TypeDefinition type)
            : base(type)
        {
        }

        public override CommandKind Kind
        {
            get { return CommandKind.Collection; }
        }

        protected override void ProcessFilePath(ExecutionContext context, Snippet snippet)
        {
            snippet.SetFileName(Path.GetFileName(snippet.FilePath).Replace("ImmutableCollection", Type.Name));
        }
    }
}
