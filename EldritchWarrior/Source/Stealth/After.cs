using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.Stealth
{
    public class After
    {
        [ScriptHandler("on_exit_stlth_af")]
        public static void Exit()
        {
            System.Console.WriteLine("on_exit_stlth_af");
        }
    }
}
