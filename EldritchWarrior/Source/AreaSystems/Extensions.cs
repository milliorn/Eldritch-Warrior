using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.AreaSystems
{
    public static class Extensions
    {
        public static bool PlayersRemainInArea(this uint objectInArea)
        {
            for (uint obj = GetFirstObjectInArea(); GetIsObjectValid(objectInArea); objectInArea = GetNextObjectInArea())
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
            for (uint obj = GetFirstItemInInventory(objectInArea); GetIsObjectValid(obj); obj = GetNextItemInInventory(objectInArea))
            {
                DestroyObject(obj);
            }
        }
    }
}