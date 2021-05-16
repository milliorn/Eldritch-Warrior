using NWN.Framework.Lite;
using static NWN.Framework.Lite.NWScript;
using NWN.Framework.Lite.Enum;

namespace EldritchWarrior.Source.ItemBank
{
    public class Disturb
    {
        [ScriptHandler("bank_item_dist")]
        public static void Chest()
        {
            uint pc = GetLastDisturbed();
            uint chest = OBJECT_SELF;
            int itemCount = chest.GetChestItemCount();

            ApplyEffectAtLocation(DurationType.Instant, EffectVisualEffect(VisualEffectType.Vfx_Fnf_Smoke_Puff), GetLocation(pc));

            uint disturbedItem = GetInventoryDisturbItem();
            if (GetInventoryDisturbType() == DisturbType.Added)
            {
                if (chest.IsItemDepositLegal(pc))
                {
                    FloatingTextStringOnCreature($"{GetName(pc)} added " + GetName(disturbedItem) + " to the Transfer Chest\n" + " CD KEY = " + GetPCPublicCDKey(pc, true) + "\n" + itemCount + " items left in chest.", pc, true);
                }
            }
            else if (GetInventoryDisturbType() == DisturbType.Removed)
            {
                if (chest.IsItemDepositLegal(pc))
                {
                    FloatingTextStringOnCreature($"{GetName(pc)} removed " + GetName(disturbedItem) + " from the Transfer Chest\n" + " CD KEY = " + GetPCPublicCDKey(pc, true) + "\n" + itemCount + " items left in chest.", pc, true);
                }
            }
            else
            {
                SpeakString($"ERROR! Transfer Chest. {GetName(pc)} {GetPCPublicCDKey(pc, true)}.", TalkVolumeType.Party);
            }
        }
    }
}