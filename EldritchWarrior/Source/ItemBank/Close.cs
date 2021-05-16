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
            uint pc = GetLastClosedBy();
            uint chest = OBJECT_SELF;
            int count = 0;

            SetLocked(chest, true);

            // First loop to check if deposit is valid
            uint inventoryItem = GetFirstItemInInventory(chest);
            while (GetIsObjectValid(inventoryItem))
            {
                // Item count
                count++;

                if (GetHasInventory(inventoryItem))
                {
                    // Send a message to the player
                    FloatingTextStringOnCreature("Containers/bags are NOT allowed to be stored!!!\nPlease remove the container/bag.", pc);

                    // Unlock chest and end script
                    SetLocked(chest, false);
                    return;
                }
                else if (count > Extensions.maxItems)
                {
                    // Send a message to the player
                    FloatingTextStringOnCreature("Only a maximum of " + IntToString(Extensions.maxItems) + " items are allowed to be stored!!!" + "\nPlease remove the excess items.", pc);

                    AssignCommand(pc, () => ActionSpeakString(GetName(pc) + $" has more than {Extensions.maxItems} items in a bank chest and will lose " + $" all items if that player doesn't reduce the amount to under {Extensions.maxItems} items", TalkVolumeType.Party));

                    // End script
                    SetLocked(chest, false);
                    return;
                }

                // Next item
                inventoryItem = GetNextItemInInventory(chest);
            }


            string userID = GetLocalString(chest, "USER_ID");
            // Spawn in the NPC storer
            var bankObject = CreateObject(ObjectType.Creature, "sfpb_storage", GetLocation(pc), false, userID);
            
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
                    SetLocked(chest, false);
                    return;
                }

                // Copy item to the storer
                CopyItem(inventoryItem, bankObject, true);

                // Destroy Original
                DestroyObject(inventoryItem);

                inventoryItem = GetNextItemInInventory(chest);
            }

            // Save the NPC storer into the database
            StoreCampaignObject(Extensions.modName, Extensions.itemBankName + userID, bankObject);

            // Destroy NPC storer
            DestroyObject(bankObject);

            // Delete the local CD Key
            DeleteLocalString(chest, "USER_ID");

            DelayCommand(6.0f, () => SetLocked(chest, false));
            ApplyEffectAtLocation(DurationType.Instant, EffectVisualEffect(VisualEffectType.Vfx_Fnf_Deck), GetLocation(OBJECT_SELF));
        }
    }
}