// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Pihrtsoft.Records.Utilities;

namespace Pihrtsoft.Records
{
    [DebuggerDisplay("Count = {Count} {TagsText,nq}")]
    public class TagCollection : HashSet<string>
    {
        public TagCollection()
            : base(DefaultComparer.StringComparer)
        {
        }

        public TagCollection(IList<string> list)
            : base(list, DefaultComparer.StringComparer)
        {
        }

        private string TagsText
        {
            get { return (Count > 0) ? string.Join(", ", this.OrderBy(f => f)) : ""; }
        }

        public bool ContainsAll(params string[] tags)
        {
            foreach (string tag in tags)
            {
                if (!Contains(tag))
                    return false;
            }

            return true;
        }

        public bool ContainsAny(params string[] tags)
        {
            foreach (string tag in tags)
            {
                if (Contains(tag))
                    return true;
            }

            return false;
        }
    }
}
