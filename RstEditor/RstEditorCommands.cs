using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Shell;

namespace RstEditor
{
    class RstEditorCommands : IOleCommandTarget
    {
        internal IOleCommandTarget _nextTarget;

        private ITextView TextView { get; set; }
        private IVsLinkedUndoTransactionManager UndoManager { get; set; }

        private ReadOnlyCollection<uint> _commandIds;


        private T GetService<T>(Type type) where T : class
        {
            return ServiceProvider.GlobalProvider.GetService(type) as T;
        }

        public RstEditorCommands(IVsTextView textViewAdapter, ITextView textView)
        {
            TextView = textView;

            textViewAdapter.AddCommandFilter(this, out _nextTarget);

            var ids = (uint[])Enum.GetValues(typeof(CommandId));
            _commandIds = new ReadOnlyCollection<uint>(ids);

            UndoManager = GetService<IVsLinkedUndoTransactionManager>(typeof(SVsLinkedUndoTransactionManager));


        }

        private void Bold()
        {
            if (TextView.Selection.SelectedSpans.Count > 0)
            {
                var start = TextView.Selection.Start.Position.Position;
                var end = TextView.Selection.End.Position.Position;

                UndoManager.OpenLinkedUndo((uint)LinkedTransactionFlags.mdtDefault, "Bold");

                TextView.TextBuffer.Insert(end, "**");
                TextView.TextBuffer.Insert(start, "**");

                UndoManager.CloseLinkedUndo();
            }
        }

        private void Italic()
        {
            if (TextView.Selection.SelectedSpans.Count > 0)
            {
                var start = TextView.Selection.Start.Position.Position;
                var end = TextView.Selection.End.Position.Position;

                UndoManager.OpenLinkedUndo((uint)LinkedTransactionFlags.mdtDefault, "Italic");

                TextView.TextBuffer.Insert(end, "*");
                TextView.TextBuffer.Insert(start, "*");

                UndoManager.CloseLinkedUndo();
            }
        }


        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (pguidCmdGroup == PackageGuids.CommandGroupGuid)
            {
                switch (nCmdID)
                {
                    case (uint)CommandId.cmdidRstBold:
                        Bold();
                        break;

                    case (uint)CommandId.cmdidRstItalic:
                        Italic();
                        break;
                }

            }


            return _nextTarget.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            if (pguidCmdGroup != PackageGuids.CommandGroupGuid)
            {
                return _nextTarget.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
            }
            
            for (int i = 0; i < cCmds; i++)
            {
                if (_commandIds.Contains(prgCmds[i].cmdID))
                {
                    prgCmds[i].cmdf = (uint)(OLECMDF.OLECMDF_ENABLED | OLECMDF.OLECMDF_SUPPORTED);
                    return VSConstants.S_OK;
                }

            }
            return _nextTarget.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }
    }
}
