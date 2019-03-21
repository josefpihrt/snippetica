// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Snippetica.CodeGeneration.Commands
{
    public enum CommandKind
    {
        None,
        Multi,
        Declaration,
        Definition,
        Initializer,
        Type,
        Collection,
        ConstExprModifier,
        ConstModifier,
        InlineModifier,
        StaticModifier,
        VirtualModifier,
        AccessModifier,
        AlternativeShortcut,
        XamlProperty,
        PrefixTitle,
        SuffixFileName,
        ShortcutToLowercase,
    }
}
