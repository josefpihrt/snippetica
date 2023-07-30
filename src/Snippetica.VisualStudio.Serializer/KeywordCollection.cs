// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Snippetica.VisualStudio.Serializer;

/// <summary>
/// Represents the set of keywords.
/// </summary>
[DebuggerDisplay("Count = {Count} {Keywords,nq}")]
public class KeywordCollection : Collection<string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="KeywordCollection"/> class that is empty.
    /// </summary>
    public KeywordCollection()
        : base(new List<string>())
    {
    }

    /// <summary>
    /// Sorts the elements in the entire <see cref="KeywordCollection"/>.
    /// </summary>
    public void Sort()
    {
        Sort(Snippetica.VisualStudio.Serializer.Comparers.Comparer.StringComparer);
    }

    /// <summary>
    /// Sorts the elements in the entire <see cref="KeywordCollection"/> using the specified comparer.
    /// </summary>
    /// <param name="comparer">The <see cref="IComparer{String}"/> implementation to use when comparing elements.</param>
    public void Sort(IComparer<string> comparer)
    {
        List.Sort(comparer);
    }

    private string Keywords
    {
        get { return string.Join(", ", Items); }
    }

    private List<string> List => (List<string>)Items;
}
