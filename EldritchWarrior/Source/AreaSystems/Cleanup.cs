using NWN.Framework.Lite;

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
                objectInArea.DestroyCreaturesInArea();
            }
        }
    }
}