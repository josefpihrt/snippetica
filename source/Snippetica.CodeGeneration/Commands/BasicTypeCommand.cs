// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration.Commands
{
    public class BasicTypeCommand : SnippetCommand
    {
        public BasicTypeCommand(TypeDefinition type)
        {
            Type = type;
        }

        public TypeDefinition Type { get; }

        public ReadOnlyCollection<string> Tags => Type.Tags;

        public override CommandKind Kind => CommandKind.Type;

        public override Command ChildCommand => (Type != null) ? new PrefixTitleCommand(Type) : null;

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            LanguageDefinition language = ((LanguageExecutionContext)context).Language;

            if (Type == null)
                return;

            snippet.AddTag(KnownTags.NonUniqueShortcut);

            if (ReferenceEquals(Type, TypeDefinition.Default))
                return;

            if (snippet.HasTag(KnownTags.TryParse) && !Tags.Contains(KnownTags.TryParse))
            {
                context.IsCanceled = true;
                return;
            }

            if (Type.Name == "Void" && snippet.Language == Language.VisualBasic)
            {
                snippet.ReplaceSubOrFunctionLiteral("Sub");

                snippet.RemoveLiteral(LiteralIdentifiers.As);
                snippet.ReplacePlaceholders(LiteralIdentifiers.Type, "");

                snippet.CodeText = Regex.Replace(snippet.CodeText, $@"[\s-[\r\n]]*\${LiteralIdentifiers.As}\$[\s-[\r\n]]*", "");
            }
            else
            {
                snippet.ReplaceSubOrFunctionLiteral("Function");
            }

            snippet.Title = snippet.Title
                .Replace(Placeholders.Type, Type.Title)
                .Replace(Placeholders.OfType, $"of {Type.Title}")
                .Replace(Placeholders.GenericType, language.GetTypeParameterList(Type.Title));

            snippet.Description = snippet.Description
                .Replace(Placeholders.Type, Type.Title)
                .Replace(Placeholders.OfType, $"of {Type.Title}")
                .Replace(Placeholders.GenericType, language.GetTypeParameterList(Type.Title));

            snippet.AddNamespace(Type.Namespace);

            snippet.AddTag(KnownTags.ExcludeFromReadme);
            snippet.AddTag(KnownTags.ExcludeFromSnippetBrowser);

            if (Type.Keyword == "this")
            {
                snippet.AddLiteral(Literal.CreateClassNameLiteral("this", "Containing type name", "ThisName"));
                snippet.RemoveLiteralAndReplacePlaceholders(LiteralIdentifiers.Type, "$this$");
            }
            else
            {
                snippet.RemoveLiteralAndReplacePlaceholders(LiteralIdentifiers.Type, Type.Keyword);
            }

            Literal valueLiteral = snippet.Literals.Find(LiteralIdentifiers.Value);

            if (valueLiteral != null)
                valueLiteral.DefaultValue = Type.DefaultValue;

            if (Type.DefaultIdentifier != null)
            {
                Literal identifierLiteral = snippet.Literals.Find(LiteralIdentifiers.Identifier);

                if (identifierLiteral != null)
                    identifierLiteral.DefaultValue = Type.DefaultIdentifier;
            }

            string fileName = Path.GetFileName(snippet.FilePath);

            if (fileName.IndexOf("OfT", StringComparison.Ordinal) != -1)
            {
                fileName = fileName.Replace("OfT", $"Of{Type.Name}");
            }
            else if (snippet.HasTag(KnownTags.TryParse))
            {
                fileName = Path.GetFileNameWithoutExtension(fileName) + Type.Name + Path.GetExtension(fileName);
            }
            else
            {
                fileName = Type.Name + fileName;
            }

            snippet.SetFileName(fileName);
        }
    }
}
