// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Snippetica.VisualStudio.Serializer;

/// <summary>
/// Represents the set of shortcuts.
/// </summary>
[DebuggerDisplay("Count = {Count} {Shortcuts,nq}")]
public class ShortcutCollection : Collection<string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ShortcutCollection"/> class that is empty.
    /// </summary>
    public ShortcutCollection()
        : base(new List<string>())
    {
    }

    /// <summary>
    /// Sorts the elements in the entire <see cref="ShortcutCollection"/>.
    /// </summary>
    public void Sort()
    {
        Sort(Snippetica.VisualStudio.Serializer.Comparers.Comparer.StringComparer);
    }

    /// <summary>
    /// Sorts the elements in the entire <see cref="ShortcutCollection"/> using the specified comparer.
    /// </summary>
    /// <param name="comparer">The <see cref="IComparer{String}"/> implementation to use when comparing elements.</param>
    public void Sort(IComparer<string> comparer)
    {
        ((List<string>)Items).Sort(comparer);
    }

    private string Shortcuts
    {
        get { return string.Join(", ", Items); }
    }
}
