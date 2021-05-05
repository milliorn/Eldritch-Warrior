using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.ItemSystems
{
    public static class Extensions
    {
        public static string PrintGPValue(this uint item) => GetPlotFlag(item)
            ? $"Gold Piece Value:{GetGoldPieceValue(item)}\n\n{GetDescription(item, true, true)}"
            : GetDescription(item);

        public static void BarterFix(this uint by, uint from)
        {
            if (GetIsPC(by) && GetIsPC(from))
            {
                ExportSingleCharacter(by);
                ExportSingleCharacter(from);
                FloatingTextStringOnCreature("Character Saved.", by, false);
                FloatingTextStringOnCreature("Character Saved.", by, false);
                //Execute Shifter code ws_saveall_sub
            }
        }

        public static void SendMessageToAllPartyWithinDistance(this uint by, string message, float distance)
        { 
            uint factionMember = GetFirstFactionMember(by);
            uint area = GetArea(by);

            while(GetIsObjectValid(factionMember))
            {
                if(GetArea(factionMember) == area && GetDistanceBetween(by, factionMember) <= distance)
                {
                    FloatingTextStringOnCreature(message, factionMember);
                }
                factionMember = GetNextFactionMember(by);
            }
        }
    }
}