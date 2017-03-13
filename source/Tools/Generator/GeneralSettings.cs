using System.IO;
using System.Linq;

namespace Pihrtsoft.Snippets.CodeGeneration
{
    public class GeneralSettings
    {
        public GeneralSettings()
        {
            ProjectTitle = "Snippetica";
            DirectoryNamePrefix = ProjectTitle + ".";
            SourceDirectoryName = "source";
            GitHubPath = "http://github.com/JosefPihrt/Snippetica";
            GitHubMasterPath = $"{GitHubPath}/blob/master";
            GitHubSourcePath = $"{GitHubMasterPath}/{SourceDirectoryName}";
            ProjectName = ProjectTitle;
            ExtensionProjectName = $"{ProjectTitle}.VisualStudio";
            ChangeLogFileName = "ChangeLog.md";
            GalleryDescriptionFileName = "GalleryDescription.txt";
            PkgDefFileName = "regedit.pkgdef";
            ReadMeFileName = "README.md";
        }

        public string SolutionDirectoryPath { get; set; }

        public string ProjectTitle { get; }
        public string DirectoryNamePrefix { get; }
        public string GitHubPath { get; }
        public string GitHubMasterPath { get; }
        public string GitHubSourcePath { get; }
        public string ProjectName { get; }
        public string ExtensionProjectName { get; set; }
        public string ChangeLogFileName { get; }
        public string GalleryDescriptionFileName { get; }
        public string SourceDirectoryName { get; }
        public string PkgDefFileName { get; }
        public string ReadMeFileName { get; }

        public string ProjectPath
        {
            get { return Path.Combine(SolutionDirectoryPath, SourceDirectoryName, ProjectName); }
        }

        public string GitHubProjectPath
        {
            get { return $"{GitHubSourcePath}/{ProjectName}"; }
        }

        public string ExtensionProjectPath
        {
            get { return Path.Combine(SolutionDirectoryPath, SourceDirectoryName, ExtensionProjectName); }
        }

        public string GitHubExtensionProjectPath
        {
            get { return $"{GitHubSourcePath}/{ExtensionProjectName}"; }
        }

        public string GetProjectSubtitle(SnippetDirectory[] snippetDirectories)
        {
            return $"A collection of snippets for {GetLanguagesSeparatedWithComma(snippetDirectories)}.";
        }

        private static string GetLanguagesSeparatedWithComma(SnippetDirectory[] snippetDirectories)
        {
            string[] languages = snippetDirectories
                .GroupBy(f => f.LanguageTitle)
                .Select(f => f.Key)
                .ToArray();

            for (int i = 1; i < languages.Length - 1; i++)
            {
                languages[i] = ", " + languages[i];
            }

            languages[languages.Length - 1] = " and " + languages[languages.Length - 1];

            return string.Join("", languages);
        }
    }
}
