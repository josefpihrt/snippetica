using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
                sw.WriteLine($"* [Release Notes]({settings.GitHubMasterPath}/{"ChangeLog.md"}).");
                sw.WriteLine();
                sw.WriteLine("### Distribution");
                sw.WriteLine();
                sw.WriteLine("* **Snippetica** is distributed as [Visual Studio Extension](http://visualstudiogallery.msdn.microsoft.com/a5576f35-9f87-4c9c-8f1f-059421a23aed).");
                sw.WriteLine();
                sw.WriteLine("### Folders");
                sw.WriteLine();

                foreach (SnippetDirectory snippetDirectory in snippetDirectories)
                {
                    Snippet[] snippets = snippetDirectory.EnumerateSnippets().ToArray();

                    sw.WriteLine($"* [{snippetDirectory.DirectoryName}]({settings.GitHubExtensionProjectPath}/{snippetDirectory.DirectoryName}/{settings.ReadMeFileName}) ({snippets.Length} snippets)");
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
                    sw.WriteLine($"* [{snippetDirectory.DirectoryName}]({snippetDirectory.DirectoryName}/README.md) ({snippetDirectory.SnippetCount} snippets)");
                }

                return sw.ToString();
            }
        }

        public static void WriteDirectoryMarkdownFiles(SnippetDirectory[] snippetDirectories, CharacterSequence[] characterSequences)
        {
            foreach (SnippetDirectory snippetDirectory in snippetDirectories)
            {
                WriteDirectoryMarkdownFiles(
                    snippetDirectory,
                    snippetDirectory.Path,
                    characterSequences.Where(f => f.DirectoryNames.Contains(snippetDirectory.DirectoryName)).ToArray());
            }
        }

        public static void WriteDirectoryMarkdownFiles(SnippetDirectory snippetDirectory, string directoryPath, CharacterSequence[] characterSequences)
        {
            WriteDirectoryMarkdownFiles(snippetDirectory.EnumerateSnippets().ToArray(), directoryPath, characterSequences);
        }

        public static void WriteDirectoryMarkdownFiles(Snippet[] snippets, string directoryPath, CharacterSequence[] characterSequences)
        {
            IOUtility.WriteAllText(
                Path.Combine(directoryPath, "README.md"),
                GenerateDirectoryReadme(snippets.Where(f => !f.HasTag(KnownTags.ExcludeFromReadme)), directoryPath, characterSequences));

            IOUtility.WriteAllText(
                Path.Combine(directoryPath, SnippetsByTitleFileName),
                GenerateSnippetList(snippets, directoryPath, SnippetTableWriter.CreateTitleThenShortcut(directoryPath)));

            IOUtility.WriteAllText(
                Path.Combine(directoryPath, SnippetsByShortcutFileName),
                GenerateSnippetList(snippets, directoryPath, SnippetTableWriter.CreateShortcutThenTitle(directoryPath)));
        }

        private static string GenerateDirectoryReadme(IEnumerable<Snippet> snippets, string directoryPath, CharacterSequence[] characterSequences)
        {
            using (var sw = new StringWriter())
            {
                string directoryName = Path.GetFileName(directoryPath);

                sw.WriteLine($"## {directoryName}");
                sw.WriteLine();

                if (characterSequences.Length > 0)
                {
                    sw.WriteLine("### Quick Reference");
                    sw.WriteLine();

                    string filePath = $@"..\..\..\..\..\text\{directoryName}.md";

                    if (File.Exists(filePath))
                    {
                        sw.WriteLine(File.ReadAllText(filePath, Encoding.UTF8));
                        sw.WriteLine();
                    }

                    using (CharacterSequenceTableWriter tableWriter = CharacterSequenceTableWriter.Create())
                    {
                        tableWriter.WriteTable(characterSequences);
                        sw.Write(tableWriter.ToString());
                    }

                    sw.WriteLine();
                }

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
