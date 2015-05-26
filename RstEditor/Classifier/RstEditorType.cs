using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace RstEditor.Classifier
{
    internal static class RstEditorClassificationDefinition
    {
        /// <summary>
        /// Defines the "RstEditor" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("rst")]
        internal static ClassificationTypeDefinition RstClassificationDefinition = null;

        [Export]
        [Name("rst.italics")]
        [BaseDefinition("rst")]
        internal static ClassificationTypeDefinition RstItalicsClassificationDefinition = null;

        [Export]
        [Name("rst.bold")]
        [BaseDefinition("rst")]
        internal static ClassificationTypeDefinition RstBoldClassificationDefinition = null;

        [Export]
        [Name("rst.literal")]
        [BaseDefinition("rst")]
        internal static ClassificationTypeDefinition RstLiteralClassificationDefinition = null;

        [Export]
        [Name("rst.header")]
        [BaseDefinition("rst")]
        internal static ClassificationTypeDefinition RstHeaderDefinition = null;

        [Export]
        [Name("rst.header.h1")]
        [BaseDefinition("rst.header")]
        internal static ClassificationTypeDefinition RstH1Definition = null;

        [Export]
        [Name("rst.header.h2")]
        [BaseDefinition("rst.header")]
        internal static ClassificationTypeDefinition RstH2Definition = null;

        [Export]
        [Name("rst.header.h3")]
        [BaseDefinition("rst.header")]
        internal static ClassificationTypeDefinition RstH3Definition = null;

        [Export]
        [Name("rst.hyperlink")]
        [BaseDefinition("rst")]
        internal static ClassificationTypeDefinition RstHyperlinkDefinition = null;
    }
}
