using NWN.Framework.Lite;
using NWN.Framework.Lite.Bioware;
using NWN.Framework.Lite.Enum;

namespace EldritchWarrior.Source.ItemSystems
{
    public class UnAcquire
    {
        [ScriptHandler("x2_mod_def_unaqu")]
        public static void OnUnAcquireItem()
        {
            uint unAcquired = NWScript.GetModuleItemLost();
            uint unAcquiredBy = NWScript.GetModuleItemLostBy();

            unAcquired.PrintGPValue();
            BiowareXP2.IPRemoveAllItemProperties(unAcquired, DurationType.Temporary);

            if (NWScript.GetIsDM(unAcquired)) return;

            //Temp fix for disarm feat
            if (NWScript.GetIsInCombat(unAcquiredBy) && !NWScript.GetStolenFlag(unAcquired) && NWScript.GetIsWeaponEffective(NWScript.GetLastAttacker(unAcquired)))
            {
                NWScript.CopyItem(unAcquired, unAcquiredBy, true);
                NWScript.DestroyObject(unAcquired, 0.2f);
                return;
            }
        }
    }
}