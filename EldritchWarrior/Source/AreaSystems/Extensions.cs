using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.AreaSystems
{
    public static class Extensions
    {
        public static bool PlayersRemainInArea(this uint area)
        {
            for (uint obj = GetFirstObjectInArea(); GetIsObjectValid(area); area = GetNextObjectInArea())
            {
                if (GetIsPC(obj))
                {
                    return true;
                }
            }
            return false;
        }

        public static void DestroyInventory(this uint objectInArea)
        {
            var oItem = GetFirstItemInInventory(objectInArea);

            while (oItem != OBJECT_INVALID)
            {
                DestroyObject((uint)oItem);
                oItem = GetNextItemInInventory(objectInArea);
            }
        }
    }
}