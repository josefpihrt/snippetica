// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Snippetica.VisualStudio;

/// <summary>
/// Represents an information about snippets with same shortcut.
/// </summary>
[DebuggerDisplay("{Shortcut,nq} Count = {Count}")]
public class DuplicateShortcutInfo
{
    internal DuplicateShortcutInfo(string shortcut, IEnumerable<Snippet> snippets)
    {
        Shortcut = shortcut;
        Snippets = snippets;
    }

    /// <summary>
    /// Gets snippet shortcut.
    /// </summary>
    public string Shortcut { get; }

    /// <summary>
    /// Gets an enumerable collection of snippets with same shortcut.
    /// </summary>
    public IEnumerable<Snippet> Snippets { get; }

    internal int Count
    {
        get { return Snippets.Count(); }
    }
}
