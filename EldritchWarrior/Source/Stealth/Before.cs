using NWN.Framework.Lite;
using NWN.Framework.Lite.NWNX;
using NWN.Framework.Lite.Enum;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.Stealth
{
    public class Before
    {
        [ScriptHandler("stlent_add_bef")]
        public static void Enter()
        {
            string name = GetName(OBJECT_SELF);

            if (!Module.Extensions.GetIsClient(OBJECT_SELF)) return;
            
            if (!GetHasFeat(FeatType.HideInPlainSight, OBJECT_SELF)) return;
            
            if (GetLocalInt(OBJECT_SELF, "FEAT_HIDE_IN_PLAIN_SIGHT") == 1)
            {
                Events.SkipEvent();
                SendMessageToPC(OBJECT_SELF, $"\nNo FEAT_HIDE_IN_PLAIN_SIGHT spam allowed {name}. FEAT_HIDE_IN_PLAIN_SIGHT once every {FloatToString(6.0f, 0, 0)} seconds.\n");
                SendMessageToAllDMs($"WARNING! {name} - feat spam - FEAT_HIDE_IN_PLAIN_SIGHT");
            }
        }
    }
}
