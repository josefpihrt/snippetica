// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration.VisualStudio
{
    public class VisualStudioEnvironment : SnippetEnvironment
    {
        public override EnvironmentKind Kind
        {
            get { return EnvironmentKind.VisualStudio; }
        }

        protected override bool ShouldGenerateSnippets(SnippetDirectory directory)
        {
            return base.ShouldGenerateSnippets(directory)
                && !directory.HasTag(KnownTags.ExcludeFromVisualStudio);
        }

        protected override SnippetGenerator CreateSnippetGenerator(SnippetDirectory directory)
        {
            switch (directory.Language)
            {
                case Language.VisualBasic:
                    return new VisualStudioSnippetGenerator(LanguageDefinition.VisualBasic);
                case Language.CSharp:
                    return new VisualStudioSnippetGenerator(LanguageDefinition.CSharp);
                case Language.Xaml:
                    return new XamlSnippetGenerator();
                case Language.Html:
                    return new HtmlSnippetGenerator();
                default:
                    throw new ArgumentException("", nameof(directory));
            }
        }

        public override bool IsSupportedLanguage(Language language)
        {
            switch (language)
            {
                case Language.VisualBasic:
                case Language.CSharp:
                case Language.Cpp:
                case Language.Xml:
                case Language.Xaml:
                case Language.JavaScript:
                case Language.Sql:
                case Language.Html:
                case Language.Css:
                    return true;
                default:
                    return false;
            }
        }

        public override PackageGenerator CreatePackageGenerator()
        {
            return new VisualStudioPackageGenerator(this);
        }
    }
}
