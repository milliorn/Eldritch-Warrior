using NWN.Framework.Lite;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.PlayerCharacter
{
    public class Death
    {
        [ScriptHandler("mod_death")]
        public static void OnDeath()
        {
            uint pc = GetLastPlayerDied();
            pc.Save();
        }
    }
}