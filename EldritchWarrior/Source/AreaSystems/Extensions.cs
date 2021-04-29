using System;
using NWN.Framework.Lite;

namespace EldritchWarrior.Source.AreaSystems
{
    public static class Extensions
    {
        public static bool PlayersRemainInArea(this uint area)
        {
            for (uint obj = NWScript.GetFirstObjectInArea(); NWScript.GetIsObjectValid(area); area = NWScript.GetNextObjectInArea())
            {
                if (NWScript.GetIsPC(obj))
                {
                    return true;
                }
            }
            return false;
        }

        public static void DestroyInventory(this uint objectInArea)
        {
            var oItem = NWScript.GetFirstItemInInventory(objectInArea);

            while (oItem != NWScript.OBJECT_INVALID)
            {
                NWScript.DestroyObject((uint)oItem);
                oItem = NWScript.GetNextItemInInventory(objectInArea);
            }
        }
    }
}