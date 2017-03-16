// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Pihrtsoft.Records;

namespace Pihrtsoft.Snippets.CodeGeneration
{
    public class Release
    {
        public Release(DateTime releaseDate, Version version, string comment = null)
        {
            if (version == null)
                throw new ArgumentNullException(nameof(version));

            ReleaseDate = releaseDate;
            Version = version;
            Comment = comment;
        }

        public DateTime ReleaseDate { get; }
        public Version Version { get; }
        public string Comment { get; }

        public static IEnumerable<Release> LoadFromDocument(string path)
        {
            return Document.ReadRecords(path)
                .Select(record => MapFromRecord(record));
        }

        private static Release MapFromRecord(Record record)
        {
            return new Release(
                DateTime.ParseExact(record.GetString("ReleaseDate"), "yyyy-MM-dd", null),
                Version.Parse(record.GetString("Version")),
                record.GetStringOrDefault("Comment"));
        }
    }
}
