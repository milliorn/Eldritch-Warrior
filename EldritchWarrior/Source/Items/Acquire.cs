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
            uint acquired = NWScript.GetModuleItemAcquired();
            uint by = NWScript.GetModuleItemAcquiredBy();
            uint from = NWScript.GetModuleItemAcquiredFrom();

            from.PrintGPValue();
            BiowareXP2.IPRemoveAllItemProperties(acquired, DurationType.Temporary);

            if (NWScript.GetIsDM(by)) return;

            by.BarterFix(from);
        }
    }
}