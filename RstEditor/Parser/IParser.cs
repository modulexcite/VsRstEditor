using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using System;
using System.Collections.Generic;

namespace RstEditor.Parser
{
    interface IParser
    {
        void Parse(SnapshotSpan paragraph, IClassificationTypeRegistryService registry, List<ClassificationSpan> classifications);
    }
}
