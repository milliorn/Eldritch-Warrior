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
            var pc = GetLastClosedBy();
            var chest = OBJECT_SELF;
            var pcLocation = GetLocation(pc);
            var chestLocation = GetLocation(OBJECT_SELF);

            string userID = GetLocalString(chest, "USER_ID");
            string id = GetPCPublicCDKey(pc);
            string pcName = GetName(pc);
            string modName = GetName(GetModule());

            int count = 0;

            // Lock the chest
            SetLocked(chest, true);

            // First loop to check for containers
            var inventoryItem = GetFirstItemInInventory(chest);
            while (GetIsObjectValid(inventoryItem))
            {
                // Item count
                count++;

                if (GetHasInventory(inventoryItem))
                {
                    // Send a message to the player
                    FloatingTextStringOnCreature("<c�>Containers/bags are NOT allowed to" + IntToString(Extensions.maxItems) + " be stored!!!" + "\nPlease remove the container/bag.</c>", pc);

                    // Unlock chest and end script
                    SetLocked(chest, false);
                    return;
                }
                else if (count > Extensions.maxItems)
                {
                    // Send a message to the player
                    FloatingTextStringOnCreature("<c�>Only a maximum of " + IntToString(Extensions.maxItems) + " items are allowed to be stored!!!" + "\nPlease remove the excess items.</c>", pc);

                    AssignCommand(pc, () => ActionSpeakString(pcName + " has more than 30 items in a bank chest and will lose " + " all items if that player doesn't reduce the amount to under 30 items", TalkVolumeType.Party));

                    // Unlock chest and end script
                    SetLocked(chest, false);
                    return;
                }

                // Next item
                inventoryItem = GetNextItemInInventory(chest);
            }

            // Spawn in the NPC storer
            var bankObject = CreateObject(ObjectType.Creature, "sfpb_storage", pcLocation, false, userID);

            // Loop through all items in the chest and copy them into
            // the NPC storers inventory and destroy the originals
            inventoryItem = GetFirstItemInInventory(chest);
            while (GetIsObjectValid(inventoryItem))
            {
                // This is to stop the duping bug, the dupe bug happened when a player
                // would exit the server while still holding a chest open, the reason for
                // the duping was the NPC storer would never spawn in this case thus not
                // having anywhere to store the items, which ended up the items storing
                // back into the chest duplicating itself, now if this happens, the players
                // items will not be saved thus avoiding any unwanted item duplicates.
                if (!GetIsObjectValid(bankObject))
                {
                    // Delete the local CD Key
                    DeleteLocalString(chest, "USER_ID");

                    // Unlock chest
                    SetLocked(chest, false);
                    return;
                }

                // Copy item to the storer
                CopyItem(inventoryItem, bankObject, true);

                // Destroy Original
                DestroyObject(inventoryItem);

                // Next item
                inventoryItem = GetNextItemInInventory(chest);
            }

            // Save the NPC storer into the database
            StoreCampaignObject(modName, Extensions.itemBankName + userID, bankObject);

            // Destroy NPC storer
            DestroyObject(bankObject);

            // Delete the local CD Key
            DeleteLocalString(chest, "USER_ID");

            // Unlock chest
            DelayCommand(6.0f, () => SetLocked(chest, false));

            // Visual FX
            ApplyEffectAtLocation(DurationType.Instant, EffectVisualEffect(VisualEffectType.Vfx_Fnf_Deck), chestLocation);
        }
    }
}