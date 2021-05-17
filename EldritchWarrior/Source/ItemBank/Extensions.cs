using NWN.Framework.Lite.Enum;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.ItemBank
{
    public static class Extensions
    {
        public static readonly int maxItems = 30;
        public static readonly string itemBankName = "BANK_ITEM";

        public static int GetChestItemCount(this uint chest)
        {
            int count = 0;
            uint inventoryItem = GetFirstItemInInventory(chest);
            while (GetIsObjectValid(inventoryItem))
            {
                // Item count
                count++;
                inventoryItem = GetNextItemInInventory(chest);
            }
            return count;
        }

        public static bool IsItemDepositLegal(this uint chest, uint pc)
        {
            uint inventoryItem = GetFirstItemInInventory(chest);

            while (GetIsObjectValid(inventoryItem))
            {
                if (GetHasInventory(inventoryItem))
                {
                    // Send a message to the player
                    FloatingTextStringOnCreature("Containers/bags are NOT allowed to be stored!!!\nPlease remove the container/bag.", pc);
                    // Unlock chest and end script
                    SetLocked(chest, false);
                    return false;
                }
                else if (chest.GetChestItemCount() > maxItems)
                {
                    // Send a message to the player
                    FloatingTextStringOnCreature($"Only a maximum of {IntToString(maxItems)} items are allowed to be stored!!!\nPlease remove the excess items.", pc);
                    AssignCommand(pc, () => ActionSpeakString($"{GetName(pc)} has more than {maxItems} items in a bank chest and will lose  all items if that player doesn't reduce the amount to under {maxItems} items", TalkVolumeType.Party));
                    // End script
                    SetLocked(chest, false);
                    return false;
                }
                // Next item
                inventoryItem = GetNextItemInInventory(chest);
            }
            return true;
        }
    }
}