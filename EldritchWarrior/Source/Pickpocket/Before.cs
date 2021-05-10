using NWN.Framework.Lite.NWNX;
using NWN.Framework.Lite;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.Pickpocket
{
    public class Before
    {
        [ScriptHandler("use_skill_bef")]
        public static void OnSkillUsed()
        {
            string eventData = Events.GetEventData("SKILL_ID");

            if (!Module.Extensions.GetIsClient(OBJECT_SELF)) return;

            System.Console.WriteLine(eventData);

            if (eventData == "13")
            {
                if ((int)Area.GetPVPSetting(GetArea(OBJECT_SELF)) == 0)
                {
                    Events.SkipEvent();
                    AssignCommand(OBJECT_SELF, () => ClearAllActions());
                    SendMessageToPC(OBJECT_SELF, "SKILL_PICK_POCKET not allowed in No PVP areas.");
                    SendMessageToAllDMs($"WARNING! {GetName(OBJECT_SELF)} - SKILL_PICK_POCKET not allowed in No PVP areas.");
                }
                else if (GetLocalInt(OBJECT_SELF, $"{GetName(OBJECT_SELF)}_SKILL_PICK_POCKET") == 1)
                {
                    Events.SkipEvent();
                    FloatingTextStringOnCreature($"No SKILL_PICK_POCKET spam allowed {GetName(OBJECT_SELF)}. SKILL_PICK_POCKET once every {Extensions.timerRounds} seconds.", OBJECT_SELF, true);
                    SendMessageToAllDMs($"WARNING! {GetName(OBJECT_SELF)} - skill spam - SKILL_PICK_POCKET");
                }
                else
                {
                    SetLocalInt(OBJECT_SELF, $"{GetName(OBJECT_SELF)}_SKILL_PICK_POCKET", 1);
                    DelayCommand(Extensions.timerRounds, () => DeleteLocalInt(OBJECT_SELF, $"{GetName(OBJECT_SELF)}_SKILL_PICK_POCKET"));

                    SendMessageToPC(OBJECT_SELF, $"SKILL_PICK_POCKET disabled for {Extensions.timerRounds} seconds.");
                    DelayCommand(Extensions.timerRounds, () => SendMessageToPC(OBJECT_SELF, "SKILL_PICK_POCKET enabled."));
                }
            }
        }
    }
}