using System.Collections.Generic;
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
            uint area = NWScript.GetArea(NWScript.OBJECT_SELF);
            uint objectInArea = NWScript.GetFirstObjectInArea(area);

            if (!Module.Extensions.GetIsClient(pc)) return;

            System.Console.WriteLine("before PlayersRemainInArea");
            if (area.PlayersRemainInArea()) return;
            System.Console.WriteLine("after PlayersRemainInArea");

            while (NWScript.GetIsObjectValid(objectInArea))
            {
                System.Console.WriteLine($"{NWScript.GetObjectType(objectInArea).ToString()}: {NWScript.GetName(objectInArea)}");
                objectInArea = NWScript.GetNextObjectInArea(area);
            }
        }
    }
}