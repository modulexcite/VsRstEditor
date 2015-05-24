using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.Text.RegularExpressions;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System.ComponentModel.Design;
using RstEditor.Extensions;


// http://docutils.sourceforge.net/docs/ref/rst/restructuredtext.html

namespace RstEditor.Classifier
{
    /// <summary>
    /// Classifier that classifies all text as an instance of the OrinaryClassifierType
    /// </summary>
    class RstClassifier : IClassifier
    {
        IClassificationTypeRegistryService _classificationRegistry;
        RstParser _parser;

        internal RstClassifier(IClassificationTypeRegistryService registry)
        {
            _classificationRegistry = registry;

            _parser = new RstParser(_classificationRegistry);
        }

        /// <summary>
        /// This method scans the given SnapshotSpan for potential matches for this classification.
        /// In this instance, it classifies everything and returns each span as a new ClassificationSpan.
        /// </summary>
        /// <param name="trackingSpan">The span currently being classified</param>
        /// <returns>A list of ClassificationSpans that represent spans identified to be of this classification</returns>
        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            //System.Diagnostics.Debug.WriteLine(">>>> GetClassificationSpans");
            //System.Diagnostics.Debug.WriteLine(span.GetText());
            //System.Diagnostics.Debug.WriteLine("<<<<");

            span = span.GetEnclosingParagraph();

            var t = span.GetText();

            var classifications = _parser.ParseParagraph(span);

            return classifications;
        }


#pragma warning disable 67
        // This event gets raised if a non-text change would affect the classification in some way,
        // for example typing /* would cause the classification to change in C# without directly
        // affecting the span.
        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;
#pragma warning restore 67

    }
}
