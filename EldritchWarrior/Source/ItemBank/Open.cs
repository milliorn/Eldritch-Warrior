using NWN.Framework.Lite;
using static NWN.Framework.Lite.NWScript;
using NWN.Framework.Lite.Enum;

namespace EldritchWarrior.Source.ItemBank
{
    public class Open
    {
        [ScriptHandler("bank_item_open")]
        public static void Chest()
        {
            uint pc = GetLastOpenedBy();

            // End script if any of these conditions are met
            if (!GetIsPC(pc) || GetIsDM(pc) || GetIsDMPossessed(pc) || GetIsPossessedFamiliar(pc)) return;

            string id = GetPCPublicCDKey(pc);
            uint chest = OBJECT_SELF;
            string userID = GetLocalString(chest, "USER_ID");

            // If the chest is already in use then this must be a thief
            if (userID != "" && userID != id) return;

            FloatingTextStringOnCreature($"<cþf >Reminder that only a maximum of {IntToString(Extensions.maxItems)} items are allowed to be stored.</c>", pc);

            // Set the players ID as a local string onto the chest
            // for anti theft purposes
            SetLocalString(chest, "USER_ID", id);

            // Get the player's storer NPC from the database
            var storer = RetrieveCampaignObject(Extensions.itemBankName, id, GetLocation(pc));
            DeleteCampaignVariable(Extensions.itemBankName, id);

            // loop through the NPC storers inventory and copy the items
            // into the chest.
            uint inventoryItem = GetFirstItemInInventory(storer);
            while (GetIsObjectValid(inventoryItem))
            {
                // Copy the item into the chest
                CopyItem(inventoryItem, chest, true);

                // Destroy the original
                DestroyObject(inventoryItem);

                inventoryItem = GetNextItemInInventory(storer);
            }

            DestroyObject(storer);
            ApplyEffectAtLocation(DurationType.Instant, EffectVisualEffect(VisualEffectType.Vfx_Fnf_Deck), GetLocation(OBJECT_SELF));
        }
    }
}
