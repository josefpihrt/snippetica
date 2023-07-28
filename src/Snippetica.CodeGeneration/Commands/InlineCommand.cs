﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Snippetica.CodeGeneration.Commands;

public class InlineCommand : ModifierCommand
{
    public override CommandKind Kind => CommandKind.InlineModifier;

    protected override ModifierDefinition GetModifier(LanguageDefinition language)
    {
        return language.InlineModifier;
    }
}
