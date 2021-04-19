using NWN.Framework.Lite;

namespace EldritchWarrior.Source.PC
{
    public class Dying
    {
        [ScriptHandler("test")]
        public static void OnDying()
        {
            uint pc = NWScript.GetLastPlayerDying();

            pc.Scream();

            pc.TryStabilizing();
        }
    }
}