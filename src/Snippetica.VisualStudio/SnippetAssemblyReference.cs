// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Snippetica.VisualStudio;

/// <summary>
/// Represents a reference to the assembly.
/// </summary>
[DebuggerDisplay("{AssemblyName,nq} {Url,nq}")]
public class SnippetAssemblyReference
#if NETFRAMEWORK
    : ICloneable
#endif
{
    public SnippetAssemblyReference(string assemblyName)
    {
        AssemblyName = assemblyName ?? throw new ArgumentNullException(nameof(assemblyName));
    }

    /// <summary>
    /// Creates a new <see cref="SnippetAssemblyReference"/> that is a deep copy of the current instance.
    /// </summary>
    /// <returns>A new <see cref="SnippetAssemblyReference"/> that is a deep copy of the current instance.</returns>
    public object Clone()
    {
        var clone = new SnippetAssemblyReference(AssemblyName);

        if (Url is not null)
            clone.Url = new Uri(Url.OriginalString);

        return clone;
    }

    /// <summary>
    /// Gets or sets assembly name.
    /// </summary>
    public string AssemblyName { get; set; }

    /// <summary>
    /// Gets or sets url.
    /// </summary>
    public Uri? Url { get; set; }
}
