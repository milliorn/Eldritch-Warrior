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

            System.Console.WriteLine("before PlayersRemainInArea");
            if (area.PlayersRemainInArea()) return;
            System.Console.WriteLine("after PlayersRemainInArea");

            while (NWScript.GetIsObjectValid(objectInArea))
            {
                System.Console.WriteLine($"{NWScript.GetObjectType(objectInArea).ToString()}");
                
                if (NWScript.GetObjectType(objectInArea) == ObjectType.Creature)
                {
                    System.Console.WriteLine("ObjectType.Creature");
                }

                if (NWScript.GetObjectType(objectInArea) == ObjectType.Item)
                {
                    System.Console.WriteLine("ObjectType.Item");
                }
                objectInArea = NWScript.GetNextObjectInArea(area);
            }
        }
    }
}