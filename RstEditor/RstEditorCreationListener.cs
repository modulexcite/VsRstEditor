using Microsoft.VisualStudio.Editor;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text.Editor;


namespace RstEditor
{
    [Export(typeof(IVsTextViewCreationListener))]
    [ContentType("rst")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    class RstEditorCreationListener : IVsTextViewCreationListener
    {

        [Import(typeof(IVsEditorAdaptersFactoryService))]
        internal IVsEditorAdaptersFactoryService editorFactory = null;


        public void VsTextViewCreated(Microsoft.VisualStudio.TextManager.Interop.IVsTextView textViewAdapter)
        {
            IWpfTextView textView = editorFactory.GetWpfTextView(textViewAdapter);
            if (textView == null)
                return;

            textView.Properties.GetOrCreateSingletonProperty(() => new RstEditorCommands(textViewAdapter, textView));
        }
    }
}
