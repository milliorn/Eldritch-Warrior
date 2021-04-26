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
                    switch(NWScript.GetObjectType(objectInArea))
                    {
                        case ObjectType.AreaOfEffect: objectInArea.DestroyAOE(); break;
                    }
                }
            }
        }
    }
}