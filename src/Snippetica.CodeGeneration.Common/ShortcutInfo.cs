// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Snippetica.VisualStudio;

namespace Snippetica;

public class ShortcutInfo
{
    public string Value { get; set; }

    public string Description { get; set; }

    public string Comment { get; set; }

    public ShortcutKind Kind { get; set; }

    public List<Language> Languages { get; set; }

    public List<EnvironmentKind> Environments { get; set; }
}
