using NWN.Framework.Lite;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.Client
{
    public class Enter
    {
        [ScriptHandler("mod_enter")]
        public static void OnClientEnter()
        {
            uint enter = GetEnteringObject();
            string name = GetName(enter);

            if (enter.CheckName(name)) return;

            if (GetIsDM(enter))
            {
                enter.VerifyDMLogin();
                return;
            }

            enter.WelcomeMessage();
        }
    }
}