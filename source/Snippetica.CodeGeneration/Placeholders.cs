// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Snippetica.CodeGeneration
{
    public static class Placeholders
    {
        private const string Delimiter = "@";

        public const string Type = Delimiter + "type" + Delimiter;
        public const string OfType = Delimiter + "ofType" + Delimiter;
        public const string GenericType = Delimiter + "genericType" + Delimiter;
    }
}
