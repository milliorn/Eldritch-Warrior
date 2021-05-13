using NWN.Framework.Lite;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.ItemBank
{
    public class Close
    {
        [ScriptHandler("bank_item_close")]
        public static void Chest()
        {
            uint pc = GetLastClosedBy();
            uint chest = OBJECT_SELF;
            Location pcLocation = GetLocation(pc);
            Location chestLocation = GetLocation(chest);
            string name = GetName(pc);
            string chestTag = GetLocalString(chest, "CHEST_TAG");

            if (chest.ItemDepositSuccessful(pc))
            {
                SetCampaignString("ITEM_BANK_", chestTag, NWN.Framework.Lite.NWNX.Object.Serialize(chest), pc);
                DestroyObject(chest);
                AssignCommand(pc, () => ClearAllActions(true));
            }
        }
    }
}