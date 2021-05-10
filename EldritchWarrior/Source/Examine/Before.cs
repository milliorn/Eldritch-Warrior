using NWN.Framework.Lite;
using NWN.Framework.Lite.NWNX;
using NWN.Framework.Lite.Enum;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.Examine
{
    public class Before
    {
        [ScriptHandler("examine_reset")]
        public static void OnBefore()
        {
            uint examinedObject = StringToObject(Events.GetEventData("EXAMINEE_OBJECT_ID"));
            int hitDice = GetHitDice(examinedObject);

            if (!GetIsObjectValid(examinedObject) || !GetIsReactionTypeHostile(examinedObject) || GetObjectType(examinedObject) != ObjectType.Creature) return;

            if (GetSkillRank(SkillType.Lore) >= hitDice)
            {
                SetDescription(examinedObject, examinedObject.PrintCRValue());
            }
            else
            {
                FloatingTextStringOnCreature($"You need Lore Skill score of {IntToString(hitDice)} to refresh this creatures current stats.", OBJECT_SELF, false);
            }
        }
    }
}