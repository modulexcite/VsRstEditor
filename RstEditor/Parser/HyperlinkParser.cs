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
    class HyperlinkParser : IParser
    {
        const string pattern = @"`(?![ ])(?<phrase>.+?)(?<![ \n\x00])`_";

        Regex hyperlinkRegex = new Regex(pattern);


        public void Parse(SnapshotSpan paragraph, IClassificationTypeRegistryService registry, List<ClassificationSpan> classifications)
        {
            string text = paragraph.GetText();

            int pos = 0;
            while (pos < text.Length)
            {
                Match m = hyperlinkRegex.Match(text, pos);
                if (m.Success)
                {
                    var classification = registry.GetClassificationType("rst.hyperlink");
                    var snapshotSpan = new SnapshotSpan(paragraph.Start + m.Index, m.Length);

                    classifications.Add(new ClassificationSpan(snapshotSpan, classification));
                    pos = m.Index + m.Length;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
