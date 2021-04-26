using System;
using NWN.Framework.Lite;

namespace EldritchWarrior.Source.ItemSystems
{
    public static class Extensions
    {
        public static string PrintGPValue(this uint item) => NWScript.GetPlotFlag(item)
            ? $"Gold Piece Value:{NWScript.GetGoldPieceValue(item)}\n\n{NWScript.GetDescription(item, true, true)}"
            : NWScript.GetDescription(item);

        public static void BarterFix(this uint by, uint from)
        {
            if (NWScript.GetIsPC(by) && NWScript.GetIsPC(from))
            {
                NWScript.ExportSingleCharacter(by);
                NWScript.ExportSingleCharacter(from);
                NWScript.FloatingTextStringOnCreature("Character Saved.", by, false);
                NWScript.FloatingTextStringOnCreature("Character Saved.", by, false);
                //Execute Shifter code ws_saveall_sub
            }
        }

        public static void SendMessageToAllPartyWithinDistance(this uint by, string message, float distance)
        { 
            uint factionMember = NWScript.GetFirstFactionMember(by);
            uint area = NWScript.GetArea(by);

            while(NWScript.GetIsObjectValid(factionMember))
            {
                if(NWScript.GetArea(factionMember) == area && NWScript.GetDistanceBetween(by, factionMember) <= distance)
                {
                    NWScript.FloatingTextStringOnCreature(message, factionMember);
                }
                factionMember = NWScript.GetNextFactionMember(by);
            }
        }
    }
}