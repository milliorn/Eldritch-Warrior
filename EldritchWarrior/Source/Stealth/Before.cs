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
            if (!Module.Extensions.GetIsClient(OBJECT_SELF)) return;

            if (GetStealthMode(OBJECT_SELF) == StealthModeType.Passive)
            {
                if (GetLocalInt(OBJECT_SELF, $"{GetName(OBJECT_SELF)}_FeatType.HideInPlainSight") == 1 && GetHasFeat(FeatType.HideInPlainSight, OBJECT_SELF))
                {
                    Events.SkipEvent();
                    SendMessageToPC(OBJECT_SELF, "");
                    FloatingTextStringOnCreature($"No FeatType.HideInPlainSight spam allowed.{GetName(OBJECT_SELF)}./nFeatType.HideInPlainSight once every {FloatToString(6.0f, 0, 0)} seconds.", OBJECT_SELF, true);
                    SendMessageToPC(OBJECT_SELF, "");
                    SendMessageToAllDMs($"WARNING! {GetName(OBJECT_SELF)} - feat spam - FeatType.HideInPlainSight");
                }
            }
        }
    }
}
