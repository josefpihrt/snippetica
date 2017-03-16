// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Snippets.CodeGeneration.Commands
{
    public class ParametersCommand : BaseCommand
    {
        public override CommandKind Kind
        {
            get { return CommandKind.Parameters; }
        }

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            LanguageDefinition language = ((LanguageExecutionContext)context).Language;

            snippet.SuffixTitle(" (with parameters)");
            snippet.SuffixShortcut(ShortcutChars.WithParameters);
            snippet.SuffixDescription(" (with parameters)");

            snippet.RemoveLiteralAndReplacePlaceholders(LiteralIdentifiers.ParameterList, "($parameters$)");

            snippet.AddLiteral("parameters", "Parameters", language.GetDefaultParameter());

            snippet.AddTag(KnownTags.ExcludeFromReadme);

            snippet.SuffixFileName("WithParameters");
        }
    }
}
