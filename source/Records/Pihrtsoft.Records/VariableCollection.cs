using System.Collections.Generic;
using Pihrtsoft.Records.Utilities;

namespace Pihrtsoft.Records
{
    public class VariableCollection : ReadOnlyKeyedCollection<string, Variable>
    {
        public VariableCollection(IList<Variable> list)
            : base(list)
        {
        }

        internal VariableCollection(ExtendedKeyedCollection<string, Variable> collection)
            : base(collection)
        {
        }
    }
}
