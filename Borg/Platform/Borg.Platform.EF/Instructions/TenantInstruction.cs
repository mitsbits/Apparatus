using Borg.Framework.EF.System.Domain.Silos;
using Borg.Framework.EF.System.Domain.System;

namespace Borg.Platform.EF.Instructions
{
    public class TenantInstruction : TenantInstruction<PlatformDb>
    {
    }

    public class DictionaryStateInstruction : DictionaryStateInstruction<PlatformDb> { }
    //public class FolderInstruction : FolderInstruction<PlatformDb> { }

    //public class EntryInstruction : EntryInstruction<PlatformDb> { }
}