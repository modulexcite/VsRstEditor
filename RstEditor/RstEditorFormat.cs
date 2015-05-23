using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace RstEditor
{
    #region Format definition
    /// <summary>
    /// Defines an editor format for the RstEditor type that has a purple background
    /// and is underlined.
    /// </summary>
    //[Export(typeof(EditorFormatDefinition))]
    //[ClassificationType(ClassificationTypeNames = "RstEditor")]
    //[Name("RstEditor")]
    //[UserVisible(true)] //this should be visible to the end user
    //[Order(Before = Priority.Default)] //set the priority to be after the default classifiers
    //internal sealed class RstEditorFormat : ClassificationFormatDefinition
    //{
    //    /// <summary>
    //    /// Defines the visual format for the "RstEditor" classification type
    //    /// </summary>
    //    public RstEditorFormat()
    //    {
    //        this.DisplayName = "RstEditor"; //human readable version of the name
    //        this.BackgroundColor = Colors.BlueViolet;
    //        this.TextDecorations = System.Windows.TextDecorations.Underline;
    //    }
    //}

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


    #endregion //Format definition
}
