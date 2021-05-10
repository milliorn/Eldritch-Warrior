using NWN.Framework.Lite.NWNX;
using NWN.Framework.Lite;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.Pickpocket
{
    public class After
    {
        [ScriptHandler("use_skill_aft")]
        public static void OnSkillUsed()
        {
            string eventData = Events.GetEventData("SKILL_ID");

            if (Module.Extensions.GetIsClient(OBJECT_SELF)) return;

            if (eventData == "13")
            {
                if (GetLocalInt(OBJECT_SELF, $"{GetName(OBJECT_SELF)}_SKILL_PICK_POCKET") != 1)
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