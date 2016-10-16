using System.Diagnostics;

namespace Pihrtsoft.Snippets
{
    public static class LanguageHelper
    {
        public static string GetLanguageTitle(Language language)
        {
            switch (language)
            {
                case Language.None:
                    return "";
                case Language.VisualBasic:
                    return "VB";
                case Language.CSharp:
                    return "C#";
                case Language.CPlusPlus:
                    return "C++";
                case Language.Xml:
                    return "XML";
                case Language.Xaml:
                    return "XAML";
                case Language.JavaScript:
                    return "JavaScript";
                case Language.Sql:
                    return "SQL";
                case Language.Html:
                    return "HTML";
                case Language.Css:
                    return "CSS";
                default:
                    {
                        Debug.Assert(false, language.ToString());
                        return null;
                    }
            }
        }
    }
}
