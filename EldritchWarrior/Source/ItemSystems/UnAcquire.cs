using NWN.Framework.Lite;
using NWN.Framework.Lite.Bioware;
using NWN.Framework.Lite.Enum;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.ItemSystems
{
    public class UnAcquire
    {
        [ScriptHandler("x2_mod_def_unaqu")]
        public static void OnUnAcquireItem()
        {
            uint unAcquired = GetModuleItemLost();
            uint unAcquiredBy = GetModuleItemLostBy();

            unAcquired.PrintGPValue();
            BiowareXP2.IPRemoveAllItemProperties(unAcquired, DurationType.Temporary);

            if (GetIsDM(unAcquired)) return;

            //Temp fix for disarm feat
            if (GetIsInCombat(unAcquiredBy) && !GetStolenFlag(unAcquired) && GetIsWeaponEffective(GetLastAttacker(unAcquired)))
            {
                CopyItem(unAcquired, unAcquiredBy, true);
                DestroyObject(unAcquired, 0.2f);
                return;
            }
        }
    }
}