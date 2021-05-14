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
            uint disturbedItem = GetInventoryDisturbItem();
            string name = GetName(pc);
            Location pcLocation = GetLocation(pc);
            int count = 0;

            ApplyEffectAtLocation(DurationType.Instant, EffectVisualEffect(VisualEffectType.Vfx_Fnf_Smoke_Puff), pcLocation);

            if (GetInventoryDisturbType() == DisturbType.Added)
            {
                FloatingTextStringOnCreature($"{GetName(pc)} ADDED " + GetName(disturbedItem) + " to the Transfer Chest " + " CD KEY = " + GetPCPublicCDKey(pc, true), pc, true);

                uint itemAdded = GetFirstItemInInventory(chest);
                while (GetIsObjectValid(itemAdded))
                {
                    // Item count
                    count++;

                    if (GetHasInventory(itemAdded))
                    {
                        // Send a message to the player
                        FloatingTextStringOnCreature($"Containers/bags are NOT allowed to be stored!!!\nPlease remove the container/bag.", pc);
                        return;
                    }
                    else if (count > Extensions.maxItems)
                    {
                        // Send a message to the player
                        FloatingTextStringOnCreature($"Only a maximum of {Extensions.maxItems} items are allowed to be stored!!!\nPlease remove the excess items.", pc);
                        AssignCommand(pc, () => ActionSpeakString($"{name} has more than 30 items in a bank chest and will lose  all items if that player doesn't reduce the amount to under 30 items", TalkVolumeType.Party));
                        return;
                    }

                    // Next item
                    itemAdded = GetNextItemInInventory(chest);
                }
            }
            else if (GetInventoryDisturbType() == DisturbType.Removed)
            {
                FloatingTextStringOnCreature($"{GetName(pc)} REMOVED {GetName(disturbedItem)} from the Persistant Chest  CD KEY = {GetPCPublicCDKey(pc)}", pc, true);

                uint itemRemoved = GetFirstItemInInventory(chest);
                while (GetIsObjectValid(itemRemoved))
                {
                    // Item count
                    count++;

                    if (GetHasInventory(itemRemoved))
                    {
                        // Send a message to the player
                        FloatingTextStringOnCreature($"Containers/bags are NOT allowed to be stored!!!\nPlease remove the container/bag.", pc);
                        return;
                    }
                    else if (count > Extensions.maxItems)
                    {
                        // Send a message to the player
                        FloatingTextStringOnCreature($"Only a maximum of {Extensions.maxItems} items are allowed to be stored!!!\nPlease remove the excess items.", pc);
                        AssignCommand(pc, () => ActionSpeakString($"{name} has more than 30 items in a bank chest and will lose  all items if that player doesn't reduce the amount to under 30 items", TalkVolumeType.Party));
                        return;
                    }

                    // Next item
                    itemRemoved = GetNextItemInInventory(chest);
                }
            }
            else
            {
                SpeakString($"ERROR! Transfer Chest. {name} {GetPCPublicCDKey(pc, true)}.", TalkVolumeType.Party);
            }
        }
    }
}
