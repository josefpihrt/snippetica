// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration
{
    public abstract class LanguageDefinition
    {
        public static LanguageDefinition CSharp { get; set; }
        public static LanguageDefinition VisualBasic { get; set; }

        protected LanguageDefinition()
        {
            Modifiers = new ModifierDefinitionCollection();
            Types = new TypeDefinitionCollection();
        }

        public abstract Language Language { get; }

        public ModifierDefinitionCollection Modifiers { get; }

        public TypeDefinitionCollection Types { get; }

        public TypeDefinition Object
        {
            get { return Types["Object"]; }
        }

        public ModifierDefinition StaticModifier
        {
            get { return Modifiers["Static"]; }
        }

        public ModifierDefinition VirtualModifier
        {
            get { return Modifiers["Virtual"]; }
        }

        public string DefaultValue
        {
            get { return Object.DefaultValue; }
        }

        public abstract string GetTypeParameterList(string typeName);

        public abstract string GetDefaultParameter();

        public abstract string GetCollectionInitializer(string value);

        public abstract string GetDictionaryInitializer(string value);

        public abstract string GetArrayInitializer(string value);
    }
}
