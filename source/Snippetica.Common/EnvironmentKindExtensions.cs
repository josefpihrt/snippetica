// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Snippetica
{
    public static class EnvironmentKindExtensions
    {
        public static string GetIdentifier(this EnvironmentKind kind)
        {
            switch (kind)
            {
                case EnvironmentKind.VisualStudio:
                    return "vs";
                default:
                    throw new ArgumentException("", nameof(kind));
            }
        }

        public static string GetTitle(this EnvironmentKind kind)
        {
            switch (kind)
            {
                case EnvironmentKind.VisualStudio:
                    return "Visual Studio";
                default:
                    throw new ArgumentException("", nameof(kind));
            }
        }
    }
}
