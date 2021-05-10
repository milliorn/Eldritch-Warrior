using NWN.Framework.Lite.Enum;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.Examine
{
    public static class Extensions
    {
        public static string PrintCRValue(this uint npc) => $"CR Value: {GetChallengeRating(npc)}\n\nSTR: {GetAbilityScore(npc, AbilityType.Strength)}\nDEX: {GetAbilityScore(npc, AbilityType.Dexterity)}\nCON: {GetAbilityScore(npc, AbilityType.Constitution)}\nINT: {GetAbilityScore(npc, AbilityType.Intelligence)}\nWIS: {GetAbilityScore(npc, AbilityType.Wisdom)}\nCHA: {GetAbilityScore(npc, AbilityType.Charisma)}\nAC: {GetAC(npc)}\nHP: {GetCurrentHitPoints(npc)}\nBAB: {GetBaseAttackBonus(npc)}\nFortitude: {GetFortitudeSavingThrow(npc)}\nReflex: {GetReflexSavingThrow(npc)}\nWill: {GetWillSavingThrow(npc)}\nSR: {GetSpellResistance(npc)}\n\n{GetDescription(npc, true)}";
    }
}