using NWN.Framework.Lite;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.ItemSystems
{
    public class Equip
    {
        [ScriptHandler("mod_equip")]
        public static void OnEquip()
        {
            var item = GetPCItemLastEquipped();

            item.PrintGPValue();
        }
    }
}