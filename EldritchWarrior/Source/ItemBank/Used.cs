using NWN.Framework.Lite;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.ItemBank
{
    public class Used
    {
        [ScriptHandler("bank_item_used")]
        public static void Chest()
        {
            // Vars
            uint pc = GetLastUsedBy();
            uint chest = OBJECT_SELF;
            string id = GetPCPublicCDKey(pc);
            string userID = GetLocalString(chest, "USER_ID");

            // End script if any of these conditions are met
            if (!GetIsPC(pc) || GetIsDM(pc) || GetIsDMPossessed(pc) || GetIsPossessedFamiliar(pc)) return;

            // If the chest is already in use then this must be a thief
            if (userID != "" && userID != id)
            {
                AssignCommand(pc, () => ActionMoveAwayFromObject(chest));
                return;
            }
        }
    }
}