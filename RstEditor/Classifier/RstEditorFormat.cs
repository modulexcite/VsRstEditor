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


    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "rst.header")]
    [Name("rst.header")]
    [UserVisible(true)]
    sealed class RstHeaderFormat : ClassificationFormatDefinition
    {
        public RstHeaderFormat() { this.ForegroundColor = Colors.MediumPurple; }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "rst.header.h1")]
    [Name("rst.header.h1")]
    sealed class RstH1Format : ClassificationFormatDefinition
    {
        public RstH1Format() { this.FontRenderingSize = 22; }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "rst.header.h2")]
    [Name("rst.header.h2")]
    sealed class RstH2Format : ClassificationFormatDefinition
    {
        public RstH2Format() { this.FontRenderingSize = 20; }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "rst.header.h3")]
    [Name("rst.header.h3")]
    sealed class RstH3Format : ClassificationFormatDefinition
    {
        public RstH3Format() { this.FontRenderingSize = 18; }
    }
}
