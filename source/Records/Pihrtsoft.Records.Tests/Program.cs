using System;
using System.Collections.Generic;
using System.Linq;

namespace Pihrtsoft.Records.Tests
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            Record[] records = Document.Create(@"..\..\Test.xml")
                .ReadRecords()
                .ToArray();

            foreach (Record record in records)
            {
                WriteLine(record.EntityName);

                Indent();

                foreach (KeyValuePair<string, object> pair in record.Properties)
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
