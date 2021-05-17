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
            SetLocked(chest, true);

            // First check if deposit is valid
            if (!chest.IsItemDepositLegal(pc)) return;

            string userID = GetLocalString(chest, "USER_ID");
            // Spawn in the NPC storer
            var bankObject = CreateObject(ObjectType.Creature, "sfpb_storage", GetLocation(pc), false, userID);

            // Loop through all items in the chest and copy them into
            // the NPC storers inventory and destroy the originals
            uint inventoryItem = GetFirstItemInInventory(chest);
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
            StoreCampaignObject(Extensions.itemBankName, userID, bankObject);

            // Destroy NPC storer
            DestroyObject(bankObject);

            // Delete the local CD Key
            DeleteLocalString(chest, "USER_ID");

            DelayCommand(6.0f, () => SetLocked(chest, false));
            ApplyEffectAtLocation(DurationType.Instant, EffectVisualEffect(VisualEffectType.Vfx_Fnf_Deck), GetLocation(OBJECT_SELF));
        }
    }
}