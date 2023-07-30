// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Snippetica.VisualStudio.Serializer;

/// <summary>
/// Represents the set of <see cref="Snippet"/>s.
/// </summary>
public class SnippetCollection : Collection<Snippet>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SnippetCollection"/> class that is empty.
    /// </summary>
    public SnippetCollection()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SnippetCollection"/> class as a wrapper for the specified list.
    /// </summary>
    /// <param name="list">The list to wrap.</param>
    public SnippetCollection(IList<Snippet> list)
        : base(list)
    {
    }
}
