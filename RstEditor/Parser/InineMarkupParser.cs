using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RstEditor.Parser
{
    class InlineMarkupParser : IParser
    {
        const string inline_start_prefix = @"(^|(?<=\s|[:/'""<([{]))";      // Inline markup must start with these restrictions
        const string non_whitespace_after = @"(?![ \n])";                   // Pattern for enforcing non-whitespace after a match
        const string non_whitespace_before = @"(?<![ \n])";                 // Pattern for enforcing non-whitespace before a match
                                                                            // BUGBUG Should include NULL character (for escape sequences)

        const string inline_markup = @"(?<markup>\*\*|\*(?!\*)|``)";        // Basic patterns for inline markup

        const string inline_end_suffix = @"($|(?=\s|[.,:;!?/'""<)\]}]))";   // Inline markup must end with these restrictions

        const string initial = inline_start_prefix + inline_markup + non_whitespace_after;      // Pattern to find possible inline markup
        const string strong = non_whitespace_before + @"(\*\*)" + inline_end_suffix;            // Given possible inline markup, finds strong
        const string emphasis = non_whitespace_before + @"(\*(?!\*))" + inline_end_suffix;      // ... finds emphasis
        const string literal = non_whitespace_before + @"(``)" + inline_end_suffix;             // ... finds literal

        // These regexs are based loosely on docutils\parsers\rst\states.py in RST parser

        Regex startPattern = new Regex(initial);
        Regex endStrong = new Regex(strong);
        Regex endEmphasis = new Regex(emphasis);
        Regex endLiteral = new Regex(literal);

        Dictionary<string, Regex> _lookups;
        Dictionary<string, string> _styles;

        public InlineMarkupParser()
        {
            _lookups = new Dictionary<string, Regex> {
                { "*", endEmphasis },
                { "**", endStrong },
                { "``", endLiteral }
            };

            _styles = new Dictionary<string, string> {
                { "*", "rst.italics" },
                { "**", "rst.bold" },
                { "``", "rst.literal" }
            };

        }

        public void Parse(SnapshotSpan paragraph, IClassificationTypeRegistryService registry, List<ClassificationSpan> classifications)
        {
            string text = paragraph.GetText();

            int pos = 0;
            while (pos < text.Length)
            {
                Match m = startPattern.Match(text, pos);
                if (m.Success)
                {
                    int len = m.Groups["markup"].Length;
                    int matchStart = m.Index + len;

                    string markup = m.Groups["markup"].Value;

                    var regex = _lookups[markup];

                    Match m2 = regex.Match(text, matchStart);
                    if (m2.Success)
                    {
                        var classification = registry.GetClassificationType(_styles[markup]);
                        var snapshotSpan = new SnapshotSpan(paragraph.Start + matchStart, m2.Index - matchStart);

                        classifications.Add(new ClassificationSpan(snapshotSpan, classification));
                        pos = m2.Index + m2.Length;
                    }
                    else
                    {
                        //Trace.TraceWarning("Start string without end string");
                        pos = text.IndexOf('\n', pos + 1);
                        if (pos == -1)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }

    }
}
