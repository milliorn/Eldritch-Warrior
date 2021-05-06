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
            System.Console.WriteLine("on_ent_stlth_be");
            if (!Module.Extensions.GetIsClient(OBJECT_SELF)) return;

            if (GetStealthMode(OBJECT_SELF) == StealthModeType.Passive)
            {
                if (GetLocalInt(OBJECT_SELF, $"{GetName(OBJECT_SELF)}_FeatType.HideInPlainSight") != 1 && GetHasFeat(FeatType.HideInPlainSight, OBJECT_SELF))
                {
                    SetLocalInt(OBJECT_SELF, $"{GetName(OBJECT_SELF)}_FeatType.HideInPlainSight", 1);
                    DelayCommand(6.0f, () => DeleteLocalInt(OBJECT_SELF, GetName(OBJECT_SELF) + "FeatType.HideInPlainSight"));

                    SendMessageToPC(OBJECT_SELF, $"FeatType.HideInPlainSight disabled for {FloatToString(6.0f, 0, 0)} seconds.");
                    DelayCommand(6.0f, () => SendMessageToPC(OBJECT_SELF, "FeatType.HideInPlainSight enabled."));
                }
            }
        }
    }
}
