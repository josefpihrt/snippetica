using System;

namespace Pihrtsoft.Snippets.CodeGeneration
{
    public class CSharpDefinition : LanguageDefinition
    {
        public override Language Language
        {
            get { return Language.CSharp; }
        }

        public override string GetTypeParameterList(string typeName)
        {
            return $"<{typeName}>";
        }

        public override string GetDefaultParameter()
        {
            return $"{Object.Keyword} parameter";
        }

        public override string GetDictionaryInitializer(string value)
        {
            return $" {{ [0] = {value} }}";
        }

        public override string GetCollectionInitializer(string value)
        {
            return " { " + value + " }";
        }

        public override string GetArrayInitializer(string value)
        {
            return GetCollectionInitializer(value);
        }
    }
}
