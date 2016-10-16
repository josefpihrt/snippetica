using System.Collections.ObjectModel;

namespace Pihrtsoft.Snippets.CodeGeneration
{
    public class TypeDefinitionCollection : KeyedCollection<string, TypeDefinition>
    {
        protected override string GetKeyForItem(TypeDefinition item)
        {
            return item.Name;
        }
    }
}
