using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.PlayerCharacter
{
    public class Rest
    {
        [ScriptHandler("mod_rest")]
        public static void OnRest()
        {
            uint pc = GetLastPCRested();

            if (GetLastRestEventType() == RestEventType.Cancelled || GetLastRestEventType() == RestEventType.Finished)
            {
                FadeFromBlack(pc, (float)FadeSpeedType.Slowest);
                pc.Save();
                //ExecuteScript("ws_saveall_sub", pc);
            }
            else
            {
                FadeToBlack(pc, (float)FadeSpeedType.Slowest);
            }
        }
    }
}