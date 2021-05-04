using NWN.Framework.Lite;

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
/*
            if (!Module.Extensions.GetIsClient(NWScript.GetExitingObject())) return;

            if (area.PlayersRemainInArea()) return;
*/
            while (NWScript.GetIsObjectValid(objectInArea))
            {
                System.Console.WriteLine($"{NWScript.GetObjectType(objectInArea).ToString()}: {NWScript.GetName(objectInArea)}");
                NWScript.DestroyObject(objectInArea);
                objectInArea = NWScript.GetNextObjectInArea(area);
            }
        }
    }
}