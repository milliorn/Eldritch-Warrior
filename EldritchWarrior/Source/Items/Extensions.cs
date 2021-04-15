using System;
using NWN.Framework.Lite;

namespace EldritchWarrior.Source.Items
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
    }
}