using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace RstEditor
{
    static class PackageGuids
    {
        public const string PackageGuidString = "d7948a57-4d5a-4efd-97f1-3df9f447a28c";
        public const string CommandGroupGuidString = "c2503696-5187-4355-966d-29081b30368b";
        public const string CmdSetGuidString = "AF151646-8C9E-4593-B24E-3678B095D5A3";

        public static readonly Guid PackageGuid = new Guid(PackageGuidString);
        public static readonly Guid CommandGroupGuid = new Guid(CommandGroupGuidString);
        public static readonly Guid CmdSetGuid = new Guid(CmdSetGuidString);
    }


    [Guid(PackageGuids.CmdSetGuidString)]
    enum CommandId
    {
        cmdidRstBold = 0x100,
        cmdidRstItalic = 0x101,
        cmdidRstLiteral = 0x102
    }

}
