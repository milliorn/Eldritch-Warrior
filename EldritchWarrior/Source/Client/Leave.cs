using NWN.Framework.Lite;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.Client
{
    public class Leave
    {
        [ScriptHandler("x3_mod_def_leave")]
        public static void OnClientLeave()
        {
            uint pc = GetExitingObject();

            pc.DeathLog();
            pc.PrintLogout();
        }
    }
}