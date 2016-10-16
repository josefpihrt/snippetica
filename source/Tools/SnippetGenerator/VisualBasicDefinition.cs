using System;

namespace Pihrtsoft.Snippets.CodeGeneration
{
    public class VisualBasicDefinition : LanguageDefinition
    {
        public override Language Language
        {
            get { return Language.VisualBasic; }
        }

        public override string GetTypeParameterList(string typeName)
        {
            return $"(Of {typeName})";
        }

        public override string GetDefaultParameter()
        {
            return $"parameter As {Object.Keyword}";
        }

        public override string GetDictionaryInitializer(string value)
        {
            return $" From {{{{0, {value}}}}}";
        }

        public override string GetCollectionInitializer(string value)
        {
            return " From {" + value + "}";
        }

        public override string GetArrayInitializer(string value)
        {
            return " {" + value + "}";
        }
    }
}
