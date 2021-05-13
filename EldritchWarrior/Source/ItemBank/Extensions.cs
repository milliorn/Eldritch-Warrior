using static NWN.Framework.Lite.NWScript;
using NWN.Framework.Lite.Enum;

namespace EldritchWarrior.Source.ItemBank
{
    public static class Extensions
    {
        public static readonly int maxItems = 30;

        public static string DBLocationCDKEY(this string cdkey) => $"{GetModuleName()}:{cdkey}";

        public static uint CreateBankObject(uint pc, string chestTag)
        {
            uint bank = CreateObject(ObjectType.Placeable, "_bank_", GetLocation(pc));
            SetLocalString(bank, "CHEST_TAG", chestTag);
            return bank;
        }

        public static bool ItemDepositSuccessful(this uint chest, uint pc)
        {
            //iterate over all items in the inventory
            int count = 0;
            uint item = GetFirstItemInInventory(chest);

            while (GetIsObjectValid(item))
            {
                //Item count
                count++;

                if (GetHasInventory(item))
                {
                    RefuseItem(pc, item);
                    FloatingTextStringOnCreature("No Bags allowed!", pc, false);
                    return false;
                }

                //repeated above
                else if (count > maxItems)
                {
                    pc.RefuseItem(item);
                    FloatingTextStringOnCreature("Maximum Item Count Exceeded!", pc, false);
                    return false;
                }

                //Next item
                item = GetNextItemInInventory(chest);
            }

            return true;
        }

        public static void RefuseItem(this uint pc, uint item)
        {
            DestroyObject(item, 0.2f);
            CopyItem(NWN.Framework.Lite.NWNX.Object.Deserialize(NWN.Framework.Lite.NWNX.Object.Serialize(item)));
        }
    }
}