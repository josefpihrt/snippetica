// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Snippetica.CodeGeneration
{
    [DebuggerDisplay("{Name,nq}")]
    public class TypeDefinition
    {
        public static TypeDefinition Default { get; } = new TypeDefinition(null, null, null, "a", null, null, null, 0, Array.Empty<string>());

        public TypeDefinition(
            string name,
            string title,
            string keyword,
            string shortcut,
            string defaultValue,
            string defaultIdentifier,
            string @namespace,
            int arity,
            string[] tags)
        {
            Name = name;
            Title = title;
            Keyword = keyword;
            Shortcut = shortcut;
            DefaultValue = defaultValue;
            DefaultIdentifier = defaultIdentifier;
            Namespace = @namespace;
            Arity = arity;
            Tags = new ReadOnlyCollection<string>(tags);
        }

        public string Name { get; }

        public string Title { get; }

        public string Keyword { get; }

        public string Shortcut { get; }

        public string DefaultValue { get; }

        public string DefaultIdentifier { get; }

        public string Namespace { get; }

        public int Arity { get; }

        public ReadOnlyCollection<string> Tags { get; }

        public bool IsDictionary => Name.EndsWith("Dictionary", StringComparison.Ordinal);

        public bool IsReadOnly => Name.StartsWith("ReadOnly", StringComparison.Ordinal);

        public bool IsImmutable => Name.StartsWith("Immutable", StringComparison.Ordinal);

        public bool IsInterface
        {
            get
            {
                return Name.Length > 2
                    && Name[0] == 'I'
                    && char.IsUpper(Name[1])
                    && char.IsLower(Name[2]);
            }
        }

        public bool HasTag(string tag)
        {
            return Tags.Contains(tag);
        }
    }
}
