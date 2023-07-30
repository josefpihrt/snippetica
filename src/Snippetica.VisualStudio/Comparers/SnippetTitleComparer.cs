// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Snippetica.VisualStudio.Comparers;

internal class SnippetTitleComparer : SnippetStringComparer
{
    private static readonly StringComparer _stringComparer = StringComparer.CurrentCulture;

    protected override string GetValue(Snippet snippet)
    {
        if (snippet is null)
            throw new ArgumentNullException(nameof(snippet));

        return snippet.Title;
    }

    internal override StringComparer StringComparer => _stringComparer;
}
