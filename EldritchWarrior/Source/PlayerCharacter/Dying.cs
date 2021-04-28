using NWN.Framework.Lite;

namespace EldritchWarrior.Source.PlayerCharacter
{
    public class Dying
    {
        [ScriptHandler("nw_o0_dying")]
        public static void OnDying()
        {
            uint pc = NWScript.GetLastPlayerDying();

            pc.Scream();

            pc.TryStabilizing();
        }
    }
}