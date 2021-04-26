using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;

namespace EldritchWarrior.Source.AreaSystems
{
    public class Cleanup
    {
        [ScriptHandler("are_exit_cleanup")]
        public static void Map()
        {
            uint objectInArea = NWScript.GetFirstObjectInArea();
            if (!objectInArea.PlayersRemainInArea())
            {
                while (NWScript.GetIsObjectValid(objectInArea))
                {
                    switch (NWScript.GetObjectType(objectInArea))
                    {
                        //case ObjectType.AreaOfEffect: objectInArea.DestroyAOE(); break;
                        //case ObjectType.Creature: objectInArea.DestroyCreatureInArea(); break;
                        //case ObjectType.Door: NWScript.AssignCommand(objectInArea, () => NWScript.ActionCloseDoor(objectInArea)); break;
                        case ObjectType.Item: objectInArea.DestroyItem(); break;
                        //case ObjectType.Placeable: objectInArea.DestroyInventory(); break;
                        default: break;
                    }
                    objectInArea = NWScript.GetNextObjectInArea();
                }
            }
        }
    }
}