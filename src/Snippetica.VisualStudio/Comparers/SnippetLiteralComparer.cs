// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Snippetica.VisualStudio.Comparers;

public abstract class SnippetLiteralComparer : IComparer<SnippetLiteral>, IEqualityComparer<SnippetLiteral>, IComparer, IEqualityComparer
{
    public static SnippetLiteralComparer Identifier { get; } = new IdentifierComparer();

    public abstract int Compare(SnippetLiteral? x, SnippetLiteral? y);

    public abstract bool Equals(SnippetLiteral? x, SnippetLiteral? y);

    public abstract int GetHashCode(SnippetLiteral obj);

    public int Compare(object? x, object? y)
    {
        if (x == y)
            return 0;

        if (x is null)
            return -1;

        if (y is null)
            return 1;

        if (x is SnippetLiteral a
            && y is SnippetLiteral b)
        {
            return Compare(a, b);
        }

        throw new ArgumentException("", nameof(x));
    }

    new public bool Equals(object? x, object? y)
    {
        if (x == y)
            return true;

        if (x is null)
            return false;

        if (y is null)
            return false;

        if (x is SnippetLiteral a
            && y is SnippetLiteral b)
        {
            return Equals(a, b);
        }

        throw new ArgumentException("", nameof(x));
    }

    public int GetHashCode(object obj)
    {
        if (obj is null)
            return 0;

        if (obj is SnippetLiteral type)
            return GetHashCode(type);

        throw new ArgumentException("", nameof(obj));
    }

    private class IdentifierComparer : SnippetLiteralComparer
    {
        public override int Compare(SnippetLiteral? x, SnippetLiteral? y)
        {
            if (object.ReferenceEquals(x, y))
                return 0;

            if (x is null)
                return -1;

            if (y is null)
                return 1;

            return StringComparer.CurrentCulture.Compare(x.Identifier, y.Identifier);
        }

        public override bool Equals(SnippetLiteral? x, SnippetLiteral? y)
        {
            if (object.ReferenceEquals(x, y))
                return true;

            if (x is null)
                return false;

            if (y is null)
                return false;

            return StringComparer.CurrentCulture.Equals(x.Identifier, y.Identifier);
        }

        public override int GetHashCode(SnippetLiteral? obj)
        {
            return (obj is not null) ? StringComparer.CurrentCulture.GetHashCode(obj.Identifier) : 0;
        }
    }
}
