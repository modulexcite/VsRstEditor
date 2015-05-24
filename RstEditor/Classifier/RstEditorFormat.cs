using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace RstEditor.Classifier
{
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "rst.italics")]
    [Name("rst.italics")]
    sealed class RstItalicsFormat : ClassificationFormatDefinition
    {
        public RstItalicsFormat() { this.IsItalic = true; }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "rst.bold")]
    [Name("rst.bold")]
    sealed class RstBoldFormat : ClassificationFormatDefinition
    {
        public RstBoldFormat() { this.IsBold = true; }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "rst.literal")]
    [Name("rst.literal")]
    sealed class RstLiteralFormat : ClassificationFormatDefinition
    {
        public RstLiteralFormat() 
        {
            this.FontTypeface = new Typeface("Courier New");
            this.ForegroundColor = Color.FromRgb(43, 145, 175);
        }
    }
}
