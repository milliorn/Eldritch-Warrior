using NWN.Framework.Lite;

namespace EldritchWarrior.Source.Client
{
    public class Leave
    {
        [ScriptHandler("x3_mod_def_leave")]
        public static void OnClientLeave()
        {
            uint pc = NWScript.GetExitingObject();

            pc.DeathLog();
            pc.PrintLogout();
        }
    }
}