using EldritchWarrior.Source.ItemSystems;
using NWN.Framework.Lite;
using NWN.Framework.Lite.Bioware;
using NWN.Framework.Lite.Enum;

namespace EldritchWarrior.Source.ItemSystems
{
    public class UnAcquire
    {
        [ScriptHandler("mod_unacquire")]
        public static void OnUnAcquireItem()
        {
            var unAcquired = NWScript.GetModuleItemLost();
            var unAcquiredBy = NWScript.GetModuleItemLostBy();

            unAcquired.PrintGPValue();
            PlayerCharacter.Extensions.Save(unAcquiredBy);
            BiowareXP2.IPRemoveAllItemProperties(unAcquired, DurationType.Temporary);

            if (NWScript.GetIsDM(unAcquired)) return;

            //Hopefully temp fix for disarm feat
            if (NWScript.GetIsInCombat(unAcquiredBy) && !NWScript.GetStolenFlag(unAcquired) && NWScript.GetIsWeaponEffective(NWScript.GetLastAttacker(unAcquired)))
            {
                NWScript.CopyItem(unAcquired, unAcquiredBy, true);
                NWScript.DestroyObject(unAcquired, 0.2f);
                return;
            }
        }
    }
}