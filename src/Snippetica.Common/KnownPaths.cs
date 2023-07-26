// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using static Snippetica.KnownNames;

namespace Snippetica;

public static class KnownPaths
{
    public const string GitHubUrl = "http://github.com/josefpihrt/snippetica";

    public const string MainGitHubUrl = GitHubUrl + "/blob/main";

    public const string SourceGitHubUrl = MainGitHubUrl + "/" + SourceDirectoryName;

    public const string SolutionDirectoryPath = @"..\..\..\..\..\..";

    public static string VisualStudioExtensionProjectPath
    {
        get { return Path.Combine(SolutionDirectoryPath, SourceDirectoryName, VisualStudioExtensionProjectName); }
    }

    public static string VisualStudioCodeExtensionProjectPath
    {
        get { return Path.Combine(SolutionDirectoryPath, SourceDirectoryName, VisualStudioCodeExtensionProjectName); }
    }

    public static string VisualStudioExtensionGitHubUrl
    {
        get { return $"{SourceGitHubUrl}/{VisualStudioExtensionProjectName}"; }
    }

    public static string VisualStudioCodeExtensionGitHubUrl
    {
        get { return $"{SourceGitHubUrl}/{VisualStudioCodeExtensionProjectName}"; }
    }
}
