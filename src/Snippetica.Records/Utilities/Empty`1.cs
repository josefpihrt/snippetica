// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.ObjectModel;

namespace Snippetica.Records.Utilities;

internal static class Empty<T>
{
    public static ReadOnlyCollection<T> ReadOnlyCollection { get; } = new(Array.Empty<T>());
}
