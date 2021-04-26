using System;
using NWN.Framework.Lite;

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

        public static void DestroyCreatures(this uint objectInArea)
        {
            while (NWScript.GetIsObjectValid(objectInArea))
            {
                if (Convert.ToBoolean(NWScript.GetIsEncounterCreature(objectInArea)))
                {
                    NWScript.DestroyObject(objectInArea);
                }
            }
        }
    }
}