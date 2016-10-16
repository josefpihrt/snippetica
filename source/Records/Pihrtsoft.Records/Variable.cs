using System.Diagnostics;

namespace Pihrtsoft.Records
{
    [DebuggerDisplay("{Name,nq} = {Value,nq}")]
    public class Variable : IKey<string>
    {
        public Variable(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public string Value { get; }

        public string GetKey()
        {
            return Name;
        }
    }
}
