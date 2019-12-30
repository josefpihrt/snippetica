// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text.RegularExpressions;
using Pihrtsoft.Text.RegularExpressions.Linq;
using static Pihrtsoft.Text.RegularExpressions.Linq.Patterns;

namespace Snippetica.Validations
{
    public static class RegexHelper
    {
        public static readonly Pattern InvalidLeadingSpacesPattern =
            BeginLine()
                .OneMany(Space(2))
                .Space()
                .Any(NotSpace(), EndInput())
                .WhileNotNewLineChar();

        public static readonly Pattern TrimEndPattern = Spaces().Assert(NewLine(), EndInput());

        public static readonly Regex InvalidLeadingSpaces = InvalidLeadingSpacesPattern.ToRegex();

        public static readonly Regex TrimEnd = TrimEndPattern.ToRegex();
    }
}
