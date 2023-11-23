// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml.Linq;

namespace Snippetica.Xml;

public static class XElementExtensions
{
    public static string LocalName(this XElement element)
    {
        return element.Name.LocalName;
    }
}
