using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RstEditor.Parser
{
    class HeadingParser : IParser
    {
        const string adornment = @"(^[!""#$%&'()*+,-./:;<=>?@[\\\]^_`{\|}~])\1* *$";

        // This is a paragraph, so here are the cases:
        //  Overline + title + Underline
        //  Title + underline
        //  Text

        // Valid transitions:
        // Start => overline
        // Start => Text1
        // Overline => Title
        // Title => Underline
        // Text1 => Underline   (text1 was a title)
        // Text1 => Text  

        enum HeaderStates { Start, Overline, Underline, Text1, Text, Title, Error };

        bool hasUnderline;
        char heading;
        string lineText;
        ITextSnapshotLine title;
        ITextSnapshotLine line;

        HeaderStates TransitionOnMatch(HeaderStates state)
        {
            switch (state)
            {
                case HeaderStates.Start:
                    heading = lineText[0];
                    return HeaderStates.Overline;

                case HeaderStates.Text1:
                    // TODO: Check lengths
                    hasUnderline = true;
                    return HeaderStates.Underline;

                case HeaderStates.Title:
                    if (lineText[0] == heading)
                    {
                        // TODO: Check lengths
                        hasUnderline = true;
                        return HeaderStates.Underline;
                    }
                    else
                    {
                        return HeaderStates.Error;
                    }
                default:
                    return HeaderStates.Error;
            }
        }

        HeaderStates TransitionOnNoMatch(HeaderStates state)
        {
            switch (state)
            {
                case HeaderStates.Start:
                    title = line;  // May be a title
                    return state = HeaderStates.Text1;

                case HeaderStates.Overline:
                    title = line;
                    return HeaderStates.Title;

                case HeaderStates.Underline:
                    return HeaderStates.Text;

                case HeaderStates.Text1:
                    title = null;   // First line was not a title
                    return HeaderStates.Text;

                case HeaderStates.Text:
                    // no-op
                    return HeaderStates.Text;

                case HeaderStates.Title:
                    return HeaderStates.Error; // Multi-line titles are not allowed

                default:
                    return HeaderStates.Error;
            }
        }

        public void Parse(SnapshotSpan paragraph, IClassificationTypeRegistryService registry, List<ClassificationSpan> classifications)
        {
            var state = HeaderStates.Start;

            var startLine = paragraph.Start.GetContainingLine().LineNumber;
            var endLine = paragraph.End.GetContainingLine().LineNumber;

            heading = '\0';
            title = null;
            hasUnderline = false;

            for (int i = startLine; i <= endLine; i++)
            {
                line = paragraph.Snapshot.GetLineFromLineNumber(i);
                lineText = line.GetText();
                var match = Regex.Match(lineText, adornment);
                
                state = match.Success ? TransitionOnMatch(state) : TransitionOnNoMatch(state);
                if (state == HeaderStates.Error)
                {
                    break;
                }
            }
            if (title != null && hasUnderline)
            {
                var type = registry.GetClassificationType("rst.header.h1");
                var span = new ClassificationSpan(new SnapshotSpan(title.Start, title.Length), type);
                classifications.Add(span);
            }
        
        }
    }
}
