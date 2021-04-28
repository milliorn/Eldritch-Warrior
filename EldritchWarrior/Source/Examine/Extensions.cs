using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;

namespace EldritchWarrior.Source.Examine
{
    public static class Extensions
    {
        public static string PrintCRValue(this uint npc) => $"CR Value: {NWScript.GetChallengeRating(npc)}\n\nSTR: {NWScript.GetAbilityScore(npc, AbilityType.Strength)}\nDEX: {NWScript.GetAbilityScore(npc, AbilityType.Dexterity)}\nCON: {NWScript.GetAbilityScore(npc, AbilityType.Constitution)}\nINT: {NWScript.GetAbilityScore(npc, AbilityType.Intelligence)}\nWIS: {NWScript.GetAbilityScore(npc, AbilityType.Wisdom)}\nCHA: {NWScript.GetAbilityScore(npc, AbilityType.Charisma)}\nAC: {NWScript.GetAC(npc)}\nHP: {NWScript.GetCurrentHitPoints(npc)}\nBAB: {NWScript.GetBaseAttackBonus(npc)}\nFortitude: {NWScript.GetFortitudeSavingThrow(npc)}\nReflex: {NWScript.GetReflexSavingThrow(npc)}\nWill: {NWScript.GetWillSavingThrow(npc)}\nSR: {NWScript.GetSpellResistance(npc)}\n\n{NWScript.GetDescription(npc, true)}";
    }
}