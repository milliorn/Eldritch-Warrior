using System;
using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;

namespace EldritchWarrior.Source.AreaSystems
{
    public static class Extensions
    {
        public static bool PlayersRemain(this uint objectInArea)
        {
            uint area = NWScript.GetArea(objectInArea);
            while (NWScript.GetIsObjectValid(objectInArea))
            {
                if (NWScript.GetIsPC(objectInArea))
                {
                    return true;
                }
                objectInArea = NWScript.GetNextObjectInArea(area);
            }
            return false;
        }

        public static void DestroyCreaturesInArea(this uint objectInArea)
        {
            uint area = NWScript.GetArea(objectInArea);
            while (NWScript.GetIsObjectValid(objectInArea))
            {
                if (Convert.ToBoolean(NWScript.GetObjectType(objectInArea) == ObjectType.Creature))
                {
                    NWScript.DestroyObject(objectInArea);
                }
                objectInArea = NWScript.GetNextObjectInArea(area);
            }
        }

        public static void DestroyItemsInArea(this uint objectInArea)
        {
            uint area = NWScript.GetArea(objectInArea);
            while (NWScript.GetIsObjectValid(objectInArea))
            {
                if (Convert.ToBoolean(NWScript.GetObjectType(objectInArea) == ObjectType.Creature))
                {
                    NWScript.DestroyObject(objectInArea);
                }
                objectInArea = NWScript.GetNextObjectInArea(area);
            }
        }
    }
}