// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Snippetica;

public static class ShortcutKindExtensions
{
    public static string GetTitle(this ShortcutKind kind)
    {
        switch (kind)
        {
            case ShortcutKind.None:
                return "";
            case ShortcutKind.MemberDeclaration:
                return "Type/Member Declaration";
            case ShortcutKind.DeclarationDefinition:
                return "Declaration/Definition";
            case ShortcutKind.Modifier:
                return "Modifer";
            case ShortcutKind.Specifier:
                return "Specifier";
            case ShortcutKind.Statement:
                return "Statement";
            case ShortcutKind.Operator:
                return "Operator";
            case ShortcutKind.Keyword:
                return "Keyword";
            case ShortcutKind.Type:
                return "Type";
            case ShortcutKind.Other:
                return "Other";
            default:
                throw new ArgumentException("", nameof(kind));
        }
    }
}
