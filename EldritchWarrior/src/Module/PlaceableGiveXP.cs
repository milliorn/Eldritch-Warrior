/*using System;
using NWN.Framework.Lite;
namespace Source.Module
{
    public class PlaceableGiveXP
    {
        [ScriptHandler("p_onused_givexp")]
        public static void OnUsed()
        {
            var pc = NWScript.GetLastUsedBy();
            int nextLevel = NWScript.GetHitDice(pc) + 1;
            NWScript.SetXP(pc, (nextLevel) * ((nextLevel) - 1) * 500);
            NWScript.SendMessageToPC(pc, "It works!");
        }
    }
}
*/