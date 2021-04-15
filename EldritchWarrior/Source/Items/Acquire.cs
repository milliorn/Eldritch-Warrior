using System;
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

            string name = NWScript.GetName(acquired);

            from.PrintGPValue();
            BiowareXP2.IPRemoveAllItemProperties(acquired, DurationType.Temporary);

            if (NWScript.GetIsDM(by)) return;

            by.BarterFix(from);

            if (String.IsNullOrEmpty(name))
            {
                by.SendMessageToAllPartyWithinDistance($"{name} acquired {NWScript.GetItemStackSize(acquired)} Gold Pieces.", 40.0f);
            }

            //Stop spam to combat log upon login
            if (from == NWScript.OBJECT_INVALID) return;

            by.SendMessageToAllPartyWithinDistance($"{name} acquired {NWScript.GetBaseItemType(acquired)}.", 40.0f);
        }
    }
}