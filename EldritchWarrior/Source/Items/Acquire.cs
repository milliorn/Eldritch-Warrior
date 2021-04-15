using NWN.Framework.Lite;
using NWN.Framework.Lite.Bioware;
using NWN.Framework.Lite.Enum;

namespace EldritchWarrior.Source.Items
{
    public class Acquire
    {
        [ScriptHandler("x2_mod_def_aqu")]
        public static void OnAcquireItem()
        {
            uint acquiredBy = NWScript.GetModuleItemAcquiredBy();
            uint from = NWScript.GetModuleItemAcquiredFrom();
            uint acquired = NWScript.GetModuleItemAcquired();

            from.PrintGPValue();
            BiowareXP2.IPRemoveAllItemProperties(acquired, DurationType.Temporary);
        }
    }
}