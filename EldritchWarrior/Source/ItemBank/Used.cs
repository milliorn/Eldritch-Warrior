using NWN.Framework.Lite;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.ItemBank
{
    public class Used
    {
        [ScriptHandler("bank_item_used")]
        public static void Chest()
        {
            uint pc = GetLastUsedBy();

            // End script if any of these conditions are met
            if (!GetIsPC(pc) || GetIsDM(pc) || GetIsDMPossessed(pc) || GetIsPossessedFamiliar(pc)) return;

            uint chest = OBJECT_SELF;
            string userID = GetLocalString(chest, "USER_ID");
            
            // If the chest is already in use then this must be a thief
            if (userID != "" && userID != GetPCPublicCDKey(pc))
            {
                AssignCommand(pc, () => ActionMoveAwayFromObject(chest));
                return;
            }
        }
    }
}