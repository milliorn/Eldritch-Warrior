using NWN.Framework.Lite;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.PlayerCharacter
{
    public class Dying
    {
        [ScriptHandler("nw_o0_dying")]
        public static void OnDying()
        {
            uint pc = GetLastPlayerDying();

            pc.Scream();

            pc.TryStabilizing();
        }
    }
}