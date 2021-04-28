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

            //Uncomment below to trigger bug
            /*while (NWScript.GetIsObjectValid(objectInArea))
            {
                if (NWScript.GetIsPC(objectInArea))
                {
                    // Found a player exit script
                    Console.WriteLine($"PC FOUND {NWScript.GetName(pc)}");
                }
                objectInArea = NWScript.GetNextObjectInArea(area);
            }*/
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
                }
                objectInArea = NWScript.GetNextObjectInArea(area);
            }
        }
    }
}