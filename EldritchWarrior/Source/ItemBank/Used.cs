using NWN.Framework.Lite;
using NWN.Framework.Lite.NWNX;

using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.ItemBank
{
    public class Used
    {
        [ScriptHandler("bank_item_used")]
        public static void Chest()
        {
            uint pc = GetLastUsedBy();
            Player.ForcePlaceableExamineWindow(pc, pc.GetBankObject(GetTag(OBJECT_SELF)));
        }
    }
}