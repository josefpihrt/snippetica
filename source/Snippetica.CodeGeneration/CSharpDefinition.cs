// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration
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
