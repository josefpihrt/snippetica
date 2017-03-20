// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Pihrtsoft.Snippets.CodeGeneration.Commands
{
    public class TypeCommand : BaseCommand
    {
        public TypeCommand(TypeDefinition type)
        {
            Type = type;
        }

        public TypeDefinition Type { get; }

        public ReadOnlyCollection<string> Tags
        {
            get { return Type.Tags; }
        }

        public override CommandKind Kind
        {
            get { return CommandKind.Type; }
        }

        public override Command ChildCommand
        {
            get { return new PrefixTitleCommand(Type); }
        }

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            LanguageDefinition language = ((LanguageExecutionContext)context).Language;

            snippet.AddTag(KnownTags.NonUniqueShortcut);
            snippet.AddTag(KnownTags.TitleStartsWithShortcut);

            if (Type == null)
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
                .ReplacePlaceholder(Placeholders.Type, Type.Keyword)
                .ReplacePlaceholder(Placeholders.OfType, $"of {Type.Keyword}")
                .ReplacePlaceholder(Placeholders.GenericType, language.GetTypeParameterList(Type.Keyword));

            snippet.Description = snippet.Description
                .ReplacePlaceholder(Placeholders.Type, Type.Keyword)
                .ReplacePlaceholder(Placeholders.OfType, $"of {Type.Keyword}")
                .ReplacePlaceholder(Placeholders.GenericType, language.GetTypeParameterList(Type.Keyword));

            snippet.AddNamespace(Type.Namespace);

            snippet.AddTag(KnownTags.ExcludeFromReadme);

            snippet.RemoveLiteralAndReplacePlaceholders(LiteralIdentifiers.Type, Type.Keyword);

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
