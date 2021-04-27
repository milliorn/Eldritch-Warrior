using NWN.Framework.Lite;
using NWN.Framework.Lite.NWNX;

namespace EldritchWarrior.Source.Examine
{
    public class Before
    {
        [ScriptHandler("on_exam_obj_befo")]
        public static void OnBefore()
        {
            if (Events.GetCurrentEvent() != "NWNX_ON_EXAMINE_OBJECT_BEFORE") return;

            uint pc = NWScript.OBJECT_SELF;
        }
    }
}