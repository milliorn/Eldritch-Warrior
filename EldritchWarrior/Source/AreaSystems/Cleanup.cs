using System;
using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;

namespace EldritchWarrior.Source.AreaSystems
{
    public class Cleanup
    {
        [ScriptHandler("are_exit_cleanup")]
        public static void Map()
        {
            uint pc = NWScript.GetExitingObject();
            uint area = NWScript.GetArea(pc);
            uint objectInArea = NWScript.GetFirstObjectInArea(area);

            if (!area.PlayersRemainInArea())
            {
                while (NWScript.GetIsObjectValid(objectInArea))
                {
                    switch (NWScript.GetObjectType(objectInArea))
                    {
                        case ObjectType.AreaOfEffect:
                        case ObjectType.Creature:
                        case ObjectType.Item:
                            Console.WriteLine($"{NWScript.GetObjectType(objectInArea).ToString()}: {NWScript.GetName(objectInArea)}");
                            NWScript.DestroyObject(objectInArea);
                            break;
                        case ObjectType.Door: NWScript.PlayAnimation(AnimationType.DoorClose); break;
                    }
                    objectInArea = NWScript.GetNextObjectInArea(area);
                }
            }
        }
    }
}