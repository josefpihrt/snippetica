// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Snippetica.VisualStudio.Validations;

internal static class ErrorCode
{
    public const string MissingVersion = "S1000";
    public const string InvalidVersion = "S1001";
    public const string MissingTitle = "S1002";
    public const string TitleTitleContainsWhiteSpaceOnly = "S1003";
    public const string MissingShortcut = "S1004";
    public const string InvalidShortcut = "S1005";
    public const string MissingDescription = "S1006";
    public const string NamespaceDuplicate = "S1007";
    public const string MissingLiteralIdentifier = "S1008";
    public const string InvalidLiteralIdentifier = "S1009";
    public const string MissingLiteralDefault = "S1010";
    public const string LiteralWithoutPlaceholder = "S1011";
    public const string MissingAssemblyReferenceName = "S1012";
    public const string MissingLanguage = "S1013";
    public const string MissingCode = "S1014";
    public const string InvalidCode = "S1015";
    public const string LiteralIdentifierDuplicate = "S1016";
    public const string PlaceholderWithoutLiteral = "S1017";
    public const string MissingEndPlaceholder = "S1018";
    public const string MultipleEndPlaceholders = "S1019";
    public const string MissingSelectedPlaceholder = "S1020";
    public const string MultipleSelectedPlaceholders = "S1021";
    public const string UnclosedDelimiter = "S1022";
    public const string MissingSnippetType = "S1023";
}
