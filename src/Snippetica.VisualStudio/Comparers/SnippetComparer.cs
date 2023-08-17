// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Snippetica.VisualStudio.Comparers;

internal abstract class SnippetComparer : IComparer<Snippet>, IEqualityComparer<Snippet>, IComparer, IEqualityComparer
{
    public static SnippetComparer Shortcut { get; } = new ShortcutComparer();

    public abstract int Compare(Snippet? x, Snippet? y);

    public abstract bool Equals(Snippet? x, Snippet? y);

    public abstract int GetHashCode(Snippet obj);

    public int Compare(object? x, object? y)
    {
        if (x == y)
            return 0;

        if (x is null)
            return -1;

        if (y is null)
            return 1;

        if (x is Snippet a
            && y is Snippet b)
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

        if (x is Snippet a
            && y is Snippet b)
        {
            return Equals(a, b);
        }

        throw new ArgumentException("", nameof(x));
    }

    public int GetHashCode(object obj)
    {
        if (obj is null)
            return 0;

        if (obj is Snippet type)
            return GetHashCode(type);

        throw new ArgumentException("", nameof(obj));
    }

    private class ShortcutComparer : SnippetComparer
    {
        public override int Compare(Snippet? x, Snippet? y)
        {
            if (object.ReferenceEquals(x, y))
                return 0;

            if (x is null)
                return -1;

            if (y is null)
                return 1;

            return StringComparer.CurrentCulture.Compare(x.Shortcut, y.Shortcut);
        }

        public override bool Equals(Snippet? x, Snippet? y)
        {
            if (object.ReferenceEquals(x, y))
                return true;

            if (x is null)
                return false;

            if (y is null)
                return false;

            return StringComparer.CurrentCulture.Equals(x.Shortcut, y.Shortcut);
        }

        public override int GetHashCode(Snippet? obj)
        {
            return (obj is not null) ? StringComparer.CurrentCulture.GetHashCode(obj.Shortcut) : 0;
        }
    }
}
