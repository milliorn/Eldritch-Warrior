using System;
using NWN.Framework.Lite;
using NWN.Framework.Lite.Bioware;
using NWN.Framework.Lite.Enum;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.ItemSystems
{
    public class Acquire
    {
        [ScriptHandler("mod_acquire")]
        public static void OnAcquireItem()
        {
            uint acquired = GetModuleItemAcquired();
            uint by = GetModuleItemAcquiredBy();
            uint from = GetModuleItemAcquiredFrom();

            string name = GetName(acquired);
            string byName = GetName(by);

            from.PrintGPValue();
            BiowareXP2.IPRemoveAllItemProperties(acquired, DurationType.Temporary);

            if (GetIsDM(by)) return;

            by.BarterFix(from);

            if (String.IsNullOrEmpty(name))
            {
                by.SendMessageToAllPartyWithinDistance($"{byName} acquired Gold Pieces.", 40.0f);
                return;
            }

            //Stop spam to combat log upon login
            by.SendMessageToAllPartyWithinDistance($"{byName} acquired {name}.", 40.0f);
        }
    }
}