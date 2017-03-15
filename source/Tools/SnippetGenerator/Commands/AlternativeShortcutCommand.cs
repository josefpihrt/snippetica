using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Pihrtsoft.Text.RegularExpressions.Linq;
using static Pihrtsoft.Text.RegularExpressions.Linq.Patterns;

namespace Pihrtsoft.Snippets.CodeGeneration.Commands
{
    public class AlternativeShortcutCommand : BaseCommand
    {
        private static readonly Pattern _pattern = AssertBack(LetterLower()).Assert(LetterUpper());

        private static readonly Regex _regex = _pattern.ToRegex();

        public override CommandKind Kind
        {
            get { return CommandKind.AlternativeShortcut; }
        }

        public override Command ChildCommand
        {
            get { return new SimpleCommand(f => f.SuffixFileName("_"), CommandKind.SuffixFileName); }
        }

        protected override void Execute(ExecutionContext context, Snippet snippet)
        {
            IEnumerable<string> values = _regex.Split(snippet.Shortcut)
                .Select(f => f.Substring(0, 1) + f.Substring(f.Length - 1, 1))
                .Select(f => f.ToLower());

            snippet.Shortcut = string.Concat(values);
        }
    }
}
