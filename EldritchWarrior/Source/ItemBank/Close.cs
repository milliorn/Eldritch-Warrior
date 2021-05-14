using NWN.Framework.Lite;
using static NWN.Framework.Lite.NWScript;
using NWN.Framework.Lite.Enum;

namespace EldritchWarrior.Source.ItemBank
{
    public class Close
    {
        [ScriptHandler("bank_item_close")]
        public static void Chest()
        {
            // Vars
            uint pc = GetLastClosedBy();
            uint chest = OBJECT_SELF;
            Location location = GetLocation(pc);
            Location chestLocation = GetLocation(OBJECT_SELF);

            string userID = GetLocalString(chest, "USER_ID");
            string id = GetPCPublicCDKey(pc);
            string name = GetName(pc);

            int count = 0;

            // Lock the chest
            SetLocked(chest, true);

            // First loop to check for containers
            uint item = GetFirstItemInInventory(chest);
            while (GetIsObjectValid(item))
            {
                // Item count
                count++;

                if (GetHasInventory(item))
                {
                    // Send a message to the player
                    FloatingTextStringOnCreature($"Containers/bags are NOT allowed to be stored!!!\nPlease remove the container/bag.", pc);
                    // Unlock chest and end script
                    SetLocked(chest, false);
                    return;
                }
                else if (count > Extensions.maxItems)
                {
                    // Send a message to the player
                    FloatingTextStringOnCreature($"Maximum of {Extensions.maxItems} items are allowed to be stored!!!\nPlease remove the excess items.", pc);
                    AssignCommand(pc, () => ActionSpeakString($"{name} has more than 30 items in a bank chest and will lose  all items if that player doesn't reduce the amount to under 30 items", TalkVolumeType.Party));
                    // Unlock chest and end script
                    SetLocked(chest, false);
                    return;
                }

                // Next item
                item = GetNextItemInInventory(chest);
            }

            // Spawn in the NPC VisualEffectType 
            uint bank = CreateObject(ObjectType.Creature, "sfpb_storage", location, false, userID);

            // Loop through all items in the chest and copy them into
            // the NPC storers inventory and destroy the originals
            item = GetFirstItemInInventory(chest);
            while (GetIsObjectValid(item))
            {
                // This is to stop the duping bug, the dupe bug happened when a player
                // would exit the server while still holding a chest open, the reason for
                // the duping was the NPC VisualEffectType  would never spawn in this case thus not
                // having anywhere to store the items, which ended up the items storing
                // back into the chest duplicating itself, now if this happens, the players
                // items will not be saved thus avoiding any unwanted item duplicates.
                if (!GetIsObjectValid(bank))
                {
                    // Delete the local CD Key
                    DeleteLocalString(chest, "USER_ID");

                    // Unlock Chest
                    SetLocked(chest, false);
                    return;
                }

                // Copy item to the VisualEffectType 
                CopyItem(item, bank, true);

                // Destroy Original
                DestroyObject(item);

                // Next item
                item = GetNextItemInInventory(chest);
            }

            // Save the NPC VisualEffectType  into the database
            StoreCampaignObject(Extensions.itemBankName, $"{Extensions.itemBankName}{userID}", bank);
            // Destroy NPC VisualEffectType 
            DestroyObject(bank);
            // Delete the local CD Key
            DeleteLocalString(chest, "USER_ID");
            // Unlock Chest
            DelayCommand(6.0f, () => SetLocked(chest, false));

            // Visual FX
            ApplyEffectAtLocation(DurationType.Instant, EffectVisualEffect(VisualEffectType.Vfx_Fnf_Deck), chestLocation);
        }
    }
}