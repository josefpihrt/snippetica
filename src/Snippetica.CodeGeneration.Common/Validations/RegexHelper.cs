// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text.RegularExpressions;

namespace Snippetica.Validations;

public static class RegexHelper
{
    public static readonly Regex InvalidLeadingSpaces = new(@"(?m:^)(?:\ {2})+\ (?:[^ ]|\z)[^\r\n]*");

    public static readonly Regex TrimEnd = new(@"\ +(?=(?:\r?\n)|\z)");
}
