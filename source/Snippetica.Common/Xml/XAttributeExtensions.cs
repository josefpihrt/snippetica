// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml.Linq;

namespace Snippetica.Xml
{
    public static class XAttributeExtensions
    {
        public static string LocalName(this XAttribute attribute)
        {
            return attribute.Name.LocalName;
        }
    }
}
