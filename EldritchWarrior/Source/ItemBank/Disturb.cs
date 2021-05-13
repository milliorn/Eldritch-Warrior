using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;

using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.ItemBank
{
    public class Disturb
    {
        [ScriptHandler("bank_item_dist")]
        public static void Chest()
        {
            uint pc = GetLastDisturbed();
            uint chest = OBJECT_SELF;

            if (chest.ItemDepositSuccessful(pc) && GetInventoryDisturbType() == DisturbType.Added || GetInventoryDisturbType() == DisturbType.Removed)
            {
                SetCampaignString("ITEM_BANK_", GetLocalString(chest, "CHEST_TAG"), NWN.Framework.Lite.NWNX.Object.Serialize(chest), pc);
            }
            else
            {
                pc.RefuseItem(GetInventoryDisturbItem());
            }
        }
    }
}