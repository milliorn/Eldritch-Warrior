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
            // Vars
            uint pc = GetLastOpenedBy();
            uint chest = OBJECT_SELF;

            Location pcLocation = GetLocation(pc);
            Location chestLocation = GetLocation(OBJECT_SELF);

            string userID = GetLocalString(chest, "USER_ID");
            string id = GetPCPublicCDKey(pc, true);
            string pcName = GetName(pc);

            // End script if any of these conditions are met
            if (!GetIsPC(pc) || GetIsDM(pc) || GetIsDMPossessed(pc) || GetIsPossessedFamiliar(pc)) return;

            // If the chest is already in use then this must be a thief
            if (userID != "" && userID != id)
            {
                AssignCommand(pc, () => ActionMoveAwayFromObject(chest));
                return;
            }

            FloatingTextStringOnCreature($"Reminder that only a maximum of {Extensions.maxItems} items are allowed to be stored.", pc);

            // Set the players ID as a local string onto the chest
            // for anti theft purposes
            SetLocalString(chest, "USER_ID", id);

            // Get the player's storer NPC from the database
            uint bank = RetrieveCampaignObject($"{Extensions.itemBankName}", $"{Extensions.itemBankName}{userID}", pcLocation);
            DeleteCampaignVariable($"{Extensions.itemBankName}", $"{Extensions.itemBankName}{userID}");

            // loop through the NPC storers inventory and copy the items
            // into the chest.
            uint oItem = GetFirstItemInInventory(bank);
            while (GetIsObjectValid(oItem))
            {
                // Copy the item into the chest
                CopyItem(oItem, chest, true);

                // Destroy the original
                DestroyObject(oItem);

                // Next item
                oItem = GetNextItemInInventory(bank);
            }

            // Destroy the NPC storer
            DestroyObject(bank);

            //Visual FX
            ApplyEffectAtLocation(DurationType.Instant, EffectVisualEffect(VisualEffectType.Vfx_Fnf_Deck), chestLocation);
        }
    }
}
