using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace RstEditor
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
    }
}
