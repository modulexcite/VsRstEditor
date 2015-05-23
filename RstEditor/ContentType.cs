using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;


namespace RstEditor
{
    internal static class ContentType 
    {
        [Export]
        [Name("rst")]
        [BaseDefinition("htmlx")]
        internal static ContentTypeDefinition rstContentTypeDefinition = null;

        [Export]
        [FileExtension(".rst")]
        [ContentType("rst")]
        internal static FileExtensionToContentTypeDefinition hiddenFileExtensionDefinition = null;
    }




}