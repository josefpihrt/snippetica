// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Pihrtsoft.Snippets;

namespace Snippetica.CodeGeneration.VisualStudioCode;

public class VisualStudioCodeEnvironment : SnippetEnvironment
{
    public override EnvironmentKind Kind
    {
        get { return EnvironmentKind.VisualStudioCode; }
    }

    protected override bool ShouldGenerateSnippets(SnippetDirectory directory)
    {
        return base.ShouldGenerateSnippets(directory)
            && !directory.HasTag(KnownTags.ExcludeFromVisualStudioCode);
    }

    protected override SnippetGenerator CreateSnippetGenerator(SnippetDirectory directory)
    {
        switch (directory.Language)
        {
            case Language.VisualBasic:
                return new VisualStudioCodeSnippetGenerator(this, LanguageDefinitions.VisualBasic);
            case Language.CSharp:
                return new VisualStudioCodeSnippetGenerator(this, LanguageDefinitions.CSharp);
            case Language.Cpp:
                return new VisualStudioCodeSnippetGenerator(this, LanguageDefinitions.Cpp);
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
            case Language.CSharp:
            case Language.VisualBasic:
            case Language.Cpp:
            case Language.Xml:
            case Language.JavaScript:
            case Language.Sql:
            case Language.Html:
            case Language.Css:
            case Language.Json:
            case Language.Markdown:
                return true;
            default:
                return false;
        }
    }

    public override PackageGenerator CreatePackageGenerator()
    {
        return new VisualStudioCodePackageGenerator(this);
    }

    public override string GetVersion(Language language)
    {
        switch (language)
        {
            case Language.CSharp:
            case Language.VisualBasic:
            case Language.Cpp:
                return "1.0.0";
            case Language.Xml:
            case Language.JavaScript:
            case Language.Sql:
            case Language.Html:
            case Language.Css:
            case Language.Json:
            case Language.Markdown:
                return "0.6.0";
            default:
                throw new ArgumentException("", nameof(language));
        }
    }
}
