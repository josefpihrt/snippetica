using System.Collections.ObjectModel;

namespace Pihrtsoft.Snippets.CodeGeneration
{
    public class ModifierDefinitionCollection : KeyedCollection<string, ModifierDefinition>
    {
        protected override string GetKeyForItem(ModifierDefinition item)
        {
            return item.Name;
        }
    }
}
