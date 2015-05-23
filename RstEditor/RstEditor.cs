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


// http://docutils.sourceforge.net/docs/ref/rst/restructuredtext.html

namespace RstEditor
{

    #region Provider definition
    /// <summary>
    /// This class causes a classifier to be added to the set of classifiers. Since 
    /// the content type is set to "text", this classifier applies to all text files
    /// </summary>
    [Export(typeof(IClassifierProvider))]
    [ContentType("rst")]
    internal class RstEditorProvider : IClassifierProvider
    {
        /// <summary>
        /// Import the classification registry to be used for getting a reference
        /// to the custom classification type later.
        /// </summary>
        [Import]
        internal IClassificationTypeRegistryService ClassificationRegistry = null; // Set via MEF

        [Import]
        internal SVsServiceProvider ServiceProvider = null;

        public IClassifier GetClassifier(ITextBuffer buffer)
        {
            OleMenuCommandService mcs = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                ////    Create the command for the menu item.
                //CommandID menuCommandID = new CommandID(CommandGuids.CmdSetGuid, (int)CommandId.cmdidMyCommand);
                //MenuCommand menuItem = new MenuCommand((s, e) => { }, menuCommandID);
                //mcs.AddCommand(menuItem);
            }

            return buffer.Properties.GetOrCreateSingletonProperty(() => new RstEditor(ClassificationRegistry));
        }
    }
    #endregion //provider def

    #region Classifier
    /// <summary>
    /// Classifier that classifies all text as an instance of the OrinaryClassifierType
    /// </summary>
    class RstEditor : IClassifier
    {
        IClassificationTypeRegistryService _classificationRegistry;
        RstParser _parser;

        internal RstEditor(IClassificationTypeRegistryService registry)
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
    #endregion //Classifier
}
