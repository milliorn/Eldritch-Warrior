using System;
using NWN.Framework.Lite;

namespace EldritchWarrior.Source.Items
{
    public static class Extensions
    {
        public static string PrintGPValue(this uint item) => NWScript.GetPlotFlag(item)
            ? $"Gold Piece Value:{NWScript.GetGoldPieceValue(item)}\n\n{NWScript.GetDescription(item, true, true)}"
            : NWScript.GetDescription(item);
    }
}