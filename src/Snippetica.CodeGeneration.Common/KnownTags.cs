﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Snippetica;

public static class KnownTags
{
    public const string AccessModifier = nameof(AccessModifier);
    public const string AlternativeShortcut = nameof(AlternativeShortcut);
    public const string Arguments = nameof(Arguments);
    public const string Array = nameof(Array);
    public const string AutoGenerated = nameof(AutoGenerated);
    public const string BasicType = nameof(BasicType);
    public const string Collection = nameof(Collection);
    public const string Default = nameof(Default);
    public const string Environment = nameof(Environment);
    public const string ExcludeFromDocs = nameof(ExcludeFromDocs);
    public const string ExcludeFromSnippetBrowser = nameof(ExcludeFromSnippetBrowser);
    public const string ExcludeFromVisualStudio = nameof(ExcludeFromVisualStudio);
    public const string ExcludeFromVisualStudioCode = nameof(ExcludeFromVisualStudioCode);
    public const string GenerateXmlSnippets = nameof(GenerateXmlSnippets);
    public const string Initializer = nameof(Initializer);
    public const string NonUniqueShortcut = nameof(NonUniqueShortcut);
    public const string NonUniqueTitle = nameof(NonUniqueTitle);
    public const string ObsoleteShortcut = nameof(ObsoleteShortcut);
    public const string ShortcutSuffix = nameof(ShortcutSuffix);
    public const string TitleEndsWithUnderscore = nameof(TitleEndsWithUnderscore);
    public const string TitleStartsWithShortcut = nameof(TitleStartsWithShortcut);
    public const string TryParse = nameof(TryParse);
    public const string Variable = nameof(Variable);

    public const string MetaPrefix = "Meta-";
    public const string GeneratePrefix = "Generate";

    public const string GenerateAccessModifier = GeneratePrefix + "AccessModifier";

    public const string GenerateStaticModifier = GeneratePrefix + "StaticModifier";
    public const string GenerateVirtualModifier = GeneratePrefix + "VirtualModifier";
    public const string GenerateAbstractModifier = GeneratePrefix + "AbstractModifier";
    public const string GenerateAbstractModifierRequired = "AbstractModifierRequired";
    public const string GenerateInlineModifier = GeneratePrefix + "InlineModifier";
    public const string GenerateConstModifier = GeneratePrefix + "ConstModifier";
    public const string GenerateConstExprModifier = GeneratePrefix + "ConstExprModifier";
    public const string GenerateInitializer = GeneratePrefix + "Initializer";
    public const string GenerateType = GeneratePrefix + "Type";
    public const string GenerateAlternativeShortcut = GeneratePrefix + "AlternativeShortcut";
    public const string GenerateDeclarationAndDefinition = GeneratePrefix + "DeclarationAndDefinition";

    public const string GenerateBasicType = GeneratePrefix + "BasicType";
    public const string GenerateVoidType = GeneratePrefix + "VoidType";
    public const string GenerateBooleanType = GeneratePrefix + "BooleanType";
    public const string GenerateDateTimeType = GeneratePrefix + "DateTimeType";
    public const string GenerateDoubleType = GeneratePrefix + "DoubleType";
    public const string GenerateDecimalType = GeneratePrefix + "DecimalType";
    public const string GenerateInt32Type = GeneratePrefix + "Int32Type";
    public const string GenerateInt64Type = GeneratePrefix + "Int64Type";
    public const string GenerateObjectType = GeneratePrefix + "ObjectType";
    public const string GenerateStringType = GeneratePrefix + "StringType";
    public const string GenerateSingleType = GeneratePrefix + "SingleType";

    public static string GenerateTypeTag(string typeName)
    {
        return GeneratePrefix + typeName + "Type";
    }

    public static string GenerateModifierTag(string modifierName)
    {
        return GeneratePrefix + modifierName + "Modifier";
    }
}
