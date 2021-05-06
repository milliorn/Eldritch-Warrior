using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;
using NWN.Framework.Lite.NWNX;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.Stealth
{
    public class Before
    {
        [ScriptHandler("on_ent_stlth_be")]
        public static void Enter()
        {
            System.Console.WriteLine("on_ent_stlth_be");
        }
    }
}
