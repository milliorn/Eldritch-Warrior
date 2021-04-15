using NWN.Framework.Lite;
using NWN.Framework.Lite.NWNX;
using NWN.Framework.Lite.Enum;

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

            }

            enter.WelcomeMessage();
        }

        public static void WelcomeMessage(uint enter)
        {
            string colorString = $"\nNAME:{NWScript.GetName(enter)}\nID:{NWScript.GetPCPublicCDKey(enter)}\nBIC:{Player.GetBicFileName(enter)}";
            string cdKey = NWScript.GetPCPublicCDKey(enter);

            NWScript.SendMessageToPC(enter, "Welcome to the server!");
            NWScript.SpeakString($"\nLOGIN:{colorString}", TalkVolumeType.Shout);
        }
    }
}