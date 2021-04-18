using NWN.Framework.Lite;

namespace EldritchWarrior.Source.Client
{
    public class Enter
    {
        [ScriptHandler("x3_mod_def_enter")]
        public static void OnClientEnter()
        {
            uint enter = NWScript.GetEnteringObject();
            string name = NWScript.GetName(enter);

            if (enter.CheckName(name)) return;

            if (NWScript.GetIsDM(enter))
            {
                enter.VerifyDMLogin();
                return;
            }

            enter.WelcomeMessage();
        }
    }
}