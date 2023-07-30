// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Snippetica.VisualStudio.Serializer;

/// <summary>
/// Represents the set of namespaces.
/// </summary>
[DebuggerDisplay("Count = {Count} {Namespaces,nq}")]
public class NamespaceCollection : Collection<string>
{
    private static readonly NamespaceComparer _comparer = new(placeSystemFirst: true);

    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceCollection"/> class that is empty.
    /// </summary>
    public NamespaceCollection()
        : base(new List<string>())
    {
    }

    /// <summary>
    /// Sorts the elements in the entire <see cref="NamespaceCollection"/>.
    /// </summary>
    public void Sort()
    {
        Sort(_comparer);
    }

    /// <summary>
    /// Sorts the elements in the entire <see cref="NamespaceCollection"/> using the specified comparer.
    /// </summary>
    /// <param name="comparer">The <see cref="IComparer{String}"/> implementation to use when comparing elements.</param>
    public void Sort(IComparer<string> comparer)
    {
        List.Sort(comparer);
    }

    private List<string> List => (List<string>)Items;

    private string Namespaces
    {
        get { return string.Join(", ", Items); }
    }
}
