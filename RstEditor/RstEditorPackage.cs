using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace RstEditor
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [Guid(PackageGuids.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class RstEditorPackage : Package
    {
        public RstEditorPackage()
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Entering constructor for: {0}", this.ToString()));
        }

        protected override void Initialize()
        {
            //var mcs = (OleMenuCommandService)GetService(typeof(IMenuCommandService));
            //CommandID menuCommandID = new CommandID(PackageGuids.CommandGroupGuid, (int)CommandId.cmdidRstBold);
            //MenuCommand menuItem = new MenuCommand((s, e) => { System.Windows.MessageBox.Show("wha?");  }, menuCommandID);
            //mcs.AddCommand(menuItem);
        }
    }
}
