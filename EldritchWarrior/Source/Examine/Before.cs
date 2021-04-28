using NWN.Framework.Lite;
using NWN.Framework.Lite.NWNX;

namespace EldritchWarrior.Source.Examine
{
    public class Before
    {
        [ScriptHandler("on_exam_obj_befo")]
        public static void OnBefore()
        {
            uint pc = NWScript.OBJECT_SELF;
            uint npc = NWScript.StringToObject(Events.GetEventData("EXAMINEE_OBJECT_ID"));

            if (Events.GetCurrentEvent() != "NWNX_ON_EXAMINE_OBJECT_BEFORE") return; /*|| !NWScript.GetIsObjectValid(npc) || !NWScript.GetIsReactionTypeHostile(npc) || NWScript.GetObjectType(npc) != ObjectType.Creature) return;*/
            npc.PrintCRValue();

        }
    }
}