// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Snippetica.VisualStudio.Comparers;

internal abstract class SnippetStringComparer : SnippetComparer<string>
{
    internal abstract StringComparer StringComparer { get; }

    internal override IComparer<string> GenericComparer => StringComparer;

    internal override IEqualityComparer<string> GenericEqualityComparer => StringComparer;

    internal override IComparer Comparer => StringComparer;

    internal override IEqualityComparer EqualityComparer => StringComparer;
}
