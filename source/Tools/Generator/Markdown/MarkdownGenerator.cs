using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pihrtsoft.Snippets.CodeGeneration.Markdown
{
    public static class MarkdownGenerator
    {
        private const string SnippetsByTitleFileName = "SnippetsByTitle.md";
        private const string SnippetsByShortcutFileName = "SnippetsByShortcut.md";

        public static void WriteSolutionReadMe(SnippetDirectory[] snippetDirectories, GeneralSettings settings)
        {
            IOUtility.WriteAllText(
                Path.Combine(settings.SolutionDirectoryPath, settings.ReadMeFileName),
                GenerateSolutionReadMe(snippetDirectories, settings));
        }

        public static string GenerateSolutionReadMe(SnippetDirectory[] snippetDirectories, GeneralSettings settings)
        {
            using (var sw = new StringWriter())
            {
                sw.WriteLine("## Snippetica");
                sw.WriteLine();

                sw.WriteLine($"* {settings.GetProjectSubtitle(snippetDirectories)}");
                sw.WriteLine($"* [Release Notes]({settings.GitHubMasterPath}/{"ChangeLog.md"})");
                sw.WriteLine("* **Snippetica** is distributed as Visual Studio Extension.");
                sw.WriteLine();

                foreach (SnippetDirectory snippetDirectory in snippetDirectories)
                {
                    Snippet[] snippets = snippetDirectory.EnumerateSnippets().ToArray();

                    sw.WriteLine($"[{snippetDirectory.DirectoryName}]({settings.GitHubExtensionProjectPath}/{snippetDirectory.DirectoryName}/{settings.ReadMeFileName}) ({snippets.Length} snippets)");
                }

                return sw.ToString();
            }
        }

        public static void WriteProjectMarkdownFiles(SnippetDirectory[] snippetDirectories, string directoryPath)
        {
            IOUtility.WriteAllText(
                Path.Combine(directoryPath, "README.md"),
                GenerateProjectReadMe(snippetDirectories));

            Snippet[] snippets = snippetDirectories.SelectMany(f => f.EnumerateSnippets()).ToArray();

            IOUtility.WriteAllText(
                Path.Combine(directoryPath, SnippetsByTitleFileName),
                GenerateSnippetList(snippets, directoryPath, SnippetTableWriter.CreateLanguageThenTitleThenShortcut(directoryPath)));

            IOUtility.WriteAllText(
                Path.Combine(directoryPath, SnippetsByShortcutFileName),
                GenerateSnippetList(snippets, directoryPath, SnippetTableWriter.CreateLanguageThenShortcutThenTitle(directoryPath)));
        }

        private static string GenerateProjectReadMe(SnippetDirectory[] snippetDirectories)
        {
            using (var sw = new StringWriter())
            {
                sw.WriteLine();

                foreach (SnippetDirectory snippetDirectory in snippetDirectories)
                {
                    sw.WriteLine($"* [{snippetDirectory.DirectoryName}]({snippetDirectory.DirectoryName}) ({snippetDirectory.SnippetCount} snippets)");
                }

                return sw.ToString();
            }
        }

        public static void WriteDirectoryMarkdownFiles(SnippetDirectory[] snippetDirectories)
        {
            foreach (SnippetDirectory snippetDirectory in snippetDirectories)
                WriteDirectoryMarkdownFiles(snippetDirectory, snippetDirectory.Path);
        }

        public static void WriteDirectoryMarkdownFiles(SnippetDirectory snippetDirectory, string directoryPath)
        {
            WriteDirectoryMarkdownFiles(snippetDirectory.EnumerateSnippets().ToArray(), directoryPath);
        }

        public static void WriteDirectoryMarkdownFiles(Snippet[] snippets, string directoryPath)
        {
            IOUtility.WriteAllText(
                Path.Combine(directoryPath, "README.md"),
                GenerateDirectoryReadme(snippets.Where(f => !f.HasTag(KnownTags.ExcludeFromReadme)), directoryPath));

            IOUtility.WriteAllText(
                Path.Combine(directoryPath, SnippetsByTitleFileName),
                GenerateSnippetList(snippets, directoryPath, SnippetTableWriter.CreateTitleThenShortcut(directoryPath)));

            IOUtility.WriteAllText(
                Path.Combine(directoryPath, SnippetsByShortcutFileName),
                GenerateSnippetList(snippets, directoryPath, SnippetTableWriter.CreateShortcutThenTitle(directoryPath)));
        }

        private static string GenerateDirectoryReadme(IEnumerable<Snippet> snippets, string directoryPath)
        {
            using (var sw = new StringWriter())
            {
                sw.WriteLine($"## {Path.GetFileName(directoryPath)}");
                sw.WriteLine();

                sw.WriteLine($"* [full list of snippets (sorted by title)]({SnippetsByTitleFileName})");
                sw.WriteLine($"* [full list of snippets (sorted by shortcut)]({SnippetsByShortcutFileName})");
                sw.WriteLine();

                sw.WriteLine("### List of Selected Snippets");
                sw.WriteLine();

                using (SnippetTableWriter tableWriter = SnippetTableWriter.CreateTitleThenShortcut(directoryPath))
                {
                    tableWriter.WriteTable(snippets);
                    sw.Write(tableWriter.ToString());
                }

                return sw.ToString();
            }
        }

        private static string GenerateSnippetList(Snippet[] snippets, string directoryPath, SnippetTableWriter tableWriter)
        {
            using (var sw = new StringWriter())
            {
                sw.WriteLine($"## {Path.GetFileName(directoryPath)}");
                sw.WriteLine();

                string s = $"* {snippets.Length} snippets";
                sw.WriteLine(s);

                sw.WriteLine();
                sw.WriteLine("### List of Snippets");
                sw.WriteLine();

                tableWriter.WriteTable(snippets);
                sw.Write(tableWriter.ToString());

                return sw.ToString();
            }
        }
    }
}
