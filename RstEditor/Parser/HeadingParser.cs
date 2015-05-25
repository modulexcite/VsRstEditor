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
        // TODO:
        // Check line lengths
        // Format by heading level
        // Remember to treat overlines as separate heading levels!
        // Merge Error and Text states into a single Halt state

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

        enum HeaderState { Start, Overline, Underline, Text1, Text, Title, Error };

        IClassificationTypeRegistryService _registry;
        List<ClassificationSpan> _classifications;

        char heading;
        string lineText;
        
        ITextSnapshotLine title;
        ITextSnapshotLine overline;
        ITextSnapshotLine underline;

        ITextSnapshotLine line;

        // BUGBUG This should be a lookup table and configurable. OK for now.
        IClassificationType GetClassificationType()
        {
            switch (heading)
            {
                case '=':
                    return _registry.GetClassificationType("rst.header.h1");

                case '-':
                    return _registry.GetClassificationType("rst.header.h2");

                default:
                    return _registry.GetClassificationType("rst.header.h3");
            }
        }

        HeaderState TransitionAdornmentLine(HeaderState state)
        {
            switch (state)
            {
                case HeaderState.Start:
                    heading = lineText[0];
                    overline = line;
                    return HeaderState.Overline;

                case HeaderState.Text1:
                    // TODO: Check lengths
                    underline = line;
                    heading = lineText[0];
                    return HeaderState.Underline;

                case HeaderState.Title:
                    if (lineText[0] == heading)
                    {
                        // TODO: Check lengths
                        underline = line;
                        return HeaderState.Underline;
                    }
                    else
                    {
                        return HeaderState.Error;  // Overline does not match underline
                    }
                default:   // If state is overline or underline, then we have two consecutive adornment lines 
                    return HeaderState.Error;
            }
        }

        HeaderState TransitionTextLine(HeaderState state)
        {
            switch (state)
            {
                case HeaderState.Start:
                    title = line;  // May be a title
                    return state = HeaderState.Text1;

                case HeaderState.Overline:
                    title = line;
                    return HeaderState.Title;

                case HeaderState.Underline:
                    CreateClassificationSpan();
                    title = line;  // This may be the start of another title
                    return HeaderState.Text1;

                case HeaderState.Text1:
                    title = null;   // First line was not a title
                    return HeaderState.Text;

                case HeaderState.Text:
                    // no-op
                    return HeaderState.Text;

                case HeaderState.Title:
                    return HeaderState.Error; // Multi-line titles are not allowed

                default:
                    return HeaderState.Error;
            }
        }


        void CreateClassificationSpan()
        {
            if (title != null && underline != null)
            {
                var start = title.Start;
                var len = title.LengthIncludingLineBreak + underline.LengthIncludingLineBreak;

                if (overline != null)
                {
                    start = overline.Start;
                    len += overline.LengthIncludingLineBreak;
                }

                var type = GetClassificationType();
                var span = new ClassificationSpan(new SnapshotSpan(start, len), type);
                _classifications.Add(span);
            }

            // Reset
            title = null;
            underline = null;
            overline = null;
        }

        public void Parse(SnapshotSpan paragraph, IClassificationTypeRegistryService registry, List<ClassificationSpan> classifications)
        {
            _registry = registry;
            _classifications = classifications;

            var state = HeaderState.Start;

            var startLine = paragraph.Start.GetContainingLine().LineNumber;
            var endLine = paragraph.End.GetContainingLine().LineNumber;

            heading = '\0';
            title = null;
            overline = null;
            underline = null;

            for (int i = startLine; i <= endLine; i++)
            {
                line = paragraph.Snapshot.GetLineFromLineNumber(i);
                lineText = line.GetText();
                var match = Regex.Match(lineText, adornment);
                
                state = match.Success ? TransitionAdornmentLine(state) : TransitionTextLine(state);
                if (state == HeaderState.Error || state == HeaderState.Text)
                {
                    break;
                }
            }
            CreateClassificationSpan();
        }
    }
}
