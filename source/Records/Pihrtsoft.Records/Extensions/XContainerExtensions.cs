// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Xml.Linq;

namespace Pihrtsoft.Records
{
    internal static class XContainerExtensions
    {
        public static XElement FirstElement(this XContainer container)
        {
            return container.Elements().FirstOrDefault();
        }
    }
}
