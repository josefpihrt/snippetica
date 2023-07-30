// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Security;

namespace Snippetica.VisualStudio.Serializer;

/// <summary>
/// Provides a set of static methods that searches for snippet files.
/// </summary>
public static class SnippetFileSearcher
{
    /// <summary>
    /// Represents search pattern for a snippet file. This field is a constant.
    /// </summary>
    public const string Pattern = "*." + SnippetFile.Extension;

    private const SearchOption DefaultSearchOption = SearchOption.TopDirectoryOnly;

    /// <summary>
    /// Returns enumerable collection of snippet file names from a specified directories.
    /// </summary>
    /// <param name="directoryPaths">Enumerable collection of absolute or relative paths to the directories to search.</param>
    /// <returns>An enumerable collection of snippet file names.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="directoryPaths"/> is <c>null</c>.</exception>
    public static IEnumerable<string> EnumerateSnippetFiles(IEnumerable<string> directoryPaths)
    {
        return EnumerateSnippetFiles(directoryPaths, DefaultSearchOption);
    }

    /// <summary>
    /// Returns enumerable collection of snippet file names from a specified directories, optionally searching subdirectories.
    /// </summary>
    /// <param name="directoryPaths">Enumerable collection of absolute or relative paths to the directories to search.</param>
    /// <param name="searchOption">A <see cref="SearchOption"/> value that specifies whether the search should include all subdirectories or only current directory. The default value is <see cref="SearchOption.TopDirectoryOnly"/>.</param>
    /// <returns>An enumerable collection of snippet file names.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="directoryPaths"/> is <c>null</c>.</exception>
    public static IEnumerable<string> EnumerateSnippetFiles(IEnumerable<string> directoryPaths, SearchOption searchOption)
    {
        if (directoryPaths is null)
            throw new ArgumentNullException(nameof(directoryPaths));

        return EnumerateSnippetFiles();

        IEnumerable<string> EnumerateSnippetFiles()
        {
            foreach (string directoryPath in directoryPaths)
            {
                foreach (string filePath in SnippetFileSearcher.EnumerateSnippetFiles(directoryPath, searchOption))
                    yield return filePath;
            }
        }
    }

    /// <summary>
    /// Returns enumerable collection of snippet file names from a specified directory.
    /// </summary>
    /// <param name="directoryPath">The absolute or relative path to the directory to search.</param>
    /// <returns>An enumerable collection of snippet file names.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="directoryPath"/> is <c>null</c>.</exception>
    public static IEnumerable<string> EnumerateSnippetFiles(string directoryPath)
    {
        return EnumerateSnippetFiles(directoryPath, DefaultSearchOption);
    }

    /// <summary>
    /// Returns enumerable collection of snippet file names from a specified directory, optionally searching subdirectories.
    /// </summary>
    /// <param name="directoryPath">The absolute or relative path to the directory to search.</param>
    /// <param name="searchOption">A <see cref="SearchOption"/> value that specifies whether the search should include all subdirectories or only current directory. The default value is <see cref="SearchOption.TopDirectoryOnly"/>.</param>
    /// <returns>An enumerable collection of snippet file names.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="directoryPath"/> is <c>null</c>.</exception>
    public static IEnumerable<string> EnumerateSnippetFiles(string directoryPath, SearchOption searchOption)
    {
        if (directoryPath is null)
            throw new ArgumentNullException(nameof(directoryPath));

        return EnumerateSnippetFiles();

        IEnumerable<string> EnumerateSnippetFiles()
        {
            var stack = new Stack<string>();
            stack.Push(directoryPath);

            while (stack.Count > 0)
            {
                string dirPath = stack.Pop();

                if (!Directory.Exists(dirPath))
                    continue;

                IEnumerator<string> fe = GetFilesEnumerator(dirPath);
                if (fe is not null)
                {
                    using (fe)
                    {
                        while (fe.MoveNext())
                            yield return fe.Current;
                    }
                }

                if (searchOption == SearchOption.AllDirectories)
                {
                    IEnumerator<string> de = GetDirectoriesEnumerator(dirPath);
                    if (de is not null)
                    {
                        using (de)
                        {
                            while (de.MoveNext())
                                stack.Push(de.Current);
                        }
                    }
                }
            }
        }
    }

    private static IEnumerator<string> GetFilesEnumerator(string directoryPath)
    {
        try
        {
            return Directory.EnumerateFiles(directoryPath, Pattern, SearchOption.TopDirectoryOnly).GetEnumerator();
        }
        catch (Exception ex) when (IsAllowedException(ex))
        {
        }

        return null;
    }

    private static IEnumerator<string> GetDirectoriesEnumerator(string directoryPath)
    {
        try
        {
            return Directory.EnumerateDirectories(directoryPath, "*", SearchOption.TopDirectoryOnly).GetEnumerator();
        }
        catch (Exception ex) when (IsAllowedException(ex))
        {
        }

        return null;
    }

    private static bool IsAllowedException(Exception ex)
    {
        return ex is IOException
            || ex is ArgumentException
            || ex is UnauthorizedAccessException
            || ex is SecurityException
            || ex is NotSupportedException;
    }
}
