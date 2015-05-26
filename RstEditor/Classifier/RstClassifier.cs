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
using RstEditor.Parser;


// http://docutils.sourceforge.net/docs/ref/rst/restructuredtext.html

namespace RstEditor.Classifier
{
    /// <summary>
    /// Classifier that classifies all text as an instance of the OrinaryClassifierType
    /// </summary>
    class RstClassifier : IClassifier
    {
        IClassificationTypeRegistryService _classificationRegistry;
        ITextBuffer _buffer;

        List<IParser> _parsers = new List<IParser>() {
            new HeadingParser(),
            new InlineMarkupParser(),
            new HyperlinkParser()
        };

        internal RstClassifier(ITextBuffer buffer, IClassificationTypeRegistryService registry)
        {
            _classificationRegistry = registry;
            _buffer = buffer;
            _buffer.Changed += OnTextBufferChanged;
        }

        void OnTextBufferChanged(object sender, TextContentChangedEventArgs e)
        {
            var handler = this.ClassificationChanged;
            if (handler != null)
            {
                // For now raise the event for the entire changed buffer.
                var span = new SnapshotSpan(_buffer.CurrentSnapshot, 0, _buffer.CurrentSnapshot.Length);

                handler(this, new ClassificationChangedEventArgs(span));
            }
        }

        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;


        /// <summary>
        /// This method scans the given SnapshotSpan for potential matches for this classification.
        /// In this instance, it classifies everything and returns each span as a new ClassificationSpan.
        /// </summary>
        /// <param name="trackingSpan">The span currently being classified</param>
        /// <returns>A list of ClassificationSpans that represent spans identified to be of this classification</returns>
        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            span = span.GetEnclosingParagraph();

            List<ClassificationSpan> classifications = new List<ClassificationSpan>();

            foreach (var parser in _parsers)
            {
                parser.Parse(span, _classificationRegistry, classifications);
            }
            return classifications;
        }

    }
}
