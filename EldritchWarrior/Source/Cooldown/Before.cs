using NWN.Framework.Lite;
using NWN.Framework.Lite.NWNX;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.Cooldown
{
    public class Before
    {
        [ScriptHandler("feat_use_bef")]
        public static void CombatFeat()
        {
            string feat = Events.GetEventData("FEAT_ID");
            switch (feat)
            {
                case "5":
                    {
                        if (GetLocalInt(OBJECT_SELF, feat) == 1)
                        {
                            Events.SkipEvent();
                            FloatingTextStringOnCreature($"No FEAT_CALLED_SHOT spam allowed {GetName(OBJECT_SELF)}. FEAT_CALLED_SHOT once every 6 seconds.", OBJECT_SELF, true);
                            SendMessageToAllDMs($"WARNING! {GetName(OBJECT_SELF)} - feat spam - FEAT_CALLED_SHOT");
                        }
                    }
                    break;
                case "9":
                    {
                        if (GetLocalInt(OBJECT_SELF, feat) == 1)
                        {
                            Events.SkipEvent();
                            FloatingTextStringOnCreature($"No FEAT_DISARM spam allowed {GetName(OBJECT_SELF)}. FEAT_DISARM once every 6 seconds.", OBJECT_SELF, true);
                            SendMessageToAllDMs($"WARNING! {GetName(OBJECT_SELF)} - feat spam - FEAT_DISARM");
                        }
                    }
                    break;
                case "16":
                    {
                        if (GetLocalInt(OBJECT_SELF, feat) == 1)
                        {
                            Events.SkipEvent();
                            FloatingTextStringOnCreature($"No FEAT_IMPROVED_DISARM spam allowed {GetName(OBJECT_SELF)}. FEAT_IMPROVED_DISARM once every 6 seconds.", OBJECT_SELF, true);
                            SendMessageToAllDMs($"WARNING! {GetName(OBJECT_SELF)} - feat spam - FEAT_IMPROVED_DISARM");
                        }
                    }
                    break;
                case "23":
                    {
                        if (GetLocalInt(OBJECT_SELF, feat) == 1)
                        {
                            Events.SkipEvent();
                            FloatingTextStringOnCreature($"No FEAT_KNOCKDOWN spam allowed {GetName(OBJECT_SELF)}. FEAT_KNOCKDOWN once every 6 seconds.", OBJECT_SELF, true);
                            SendMessageToAllDMs($"WARNING! {GetName(OBJECT_SELF)} - feat spam - FEAT_KNOCKDOWN");
                        }
                    }
                    break;
                case "17":
                    {
                        if (GetLocalInt(OBJECT_SELF, feat) == 1)
                        {
                            Events.SkipEvent();
                            FloatingTextStringOnCreature($"No FEAT_IMPROVED_KNOCKDOWN spam allowed {GetName(OBJECT_SELF)}. FEAT_IMPROVED_KNOCKDOWN once every 6 seconds.", OBJECT_SELF, true);
                            SendMessageToAllDMs($"WARNING! {GetName(OBJECT_SELF)} - feat spam - FEAT_IMPROVED_KNOCKDOWN");
                        }
                    }
                    break;
            }
        }
    }
}