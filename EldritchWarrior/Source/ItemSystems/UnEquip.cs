using NWN.Framework.Lite;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.ItemSystems
{
    public class UnEquip
    {
        [ScriptHandler("mod_unequip")]
        public static void OnEquip()
        {
            var item = GetPCItemLastUnequipped();

            item.PrintGPValue();
        }
    }
}