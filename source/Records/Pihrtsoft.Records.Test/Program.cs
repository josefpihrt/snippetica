// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Pihrtsoft.Records.Tests
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            var settings = new DocumentSettings() { UseVariables = true };

            foreach (Record record in Document.ReadRecords(@"..\..\Test.xml", settings))
            {
                WriteLine(record.EntityName);

#if DEBUG
                Indent();

                foreach (KeyValuePair<string, object> pair in record.GetProperties())
                {
                    var list = pair.Value as List<object>;
                    if (list != null)
                    {
                        WriteLine($"[{pair.Key}]");

                        Indent();

                        foreach (object item in list)
                            WriteLine(item);

                        Unindent();
                    }
                    else
                    {
                        WriteLine($"[{pair.Key}] {pair.Value}");
                    }
                }

                Unindent();
#endif

                Console.WriteLine("");
            }

            Console.WriteLine("** DONE ***");
            Console.ReadKey();
        }

        private static void Indent()
        {
            IndentString += "  ";
        }

        private static void Unindent()
        {
            if (IndentString.Length >= 2)
            {
                IndentString = IndentString.Remove(IndentString.Length - 2);
            }
        }

        private static string IndentString { get; set; } = "";

        private static void WriteLine(object value)
        {
            Console.WriteLine(IndentString + value);
        }
    }
}
