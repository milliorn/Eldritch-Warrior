using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.Stealth
{
    public class After
    {
        [ScriptHandler("stlex_add_aft")]
        public static void Exit()
        {
            string name = GetName(OBJECT_SELF);

            if (!Module.Extensions.GetIsClient(OBJECT_SELF)) return;

            if (GetStealthMode(OBJECT_SELF) == StealthModeType.Active) return;

            if (!GetHasFeat(FeatType.HideInPlainSight, OBJECT_SELF)) return;

            if (GetLocalInt(OBJECT_SELF, "FEAT_HIDE_IN_PLAIN_SIGHT") != 1)
            {
                SetLocalInt(OBJECT_SELF, "FEAT_HIDE_IN_PLAIN_SIGHT", 1);
                DelayCommand(6.0f, () => DeleteLocalInt(OBJECT_SELF, "FEAT_HIDE_IN_PLAIN_SIGHT"));

                SendMessageToPC(OBJECT_SELF, "FEAT_HIDE_IN_PLAIN_SIGHT disabled for " + FloatToString(6.0f, 0, 0) + " seconds.");
                DelayCommand(6.0f, () => SendMessageToPC(OBJECT_SELF, "FEAT_HIDE_IN_PLAIN_SIGHT enabled."));
            }
        }
    }
}