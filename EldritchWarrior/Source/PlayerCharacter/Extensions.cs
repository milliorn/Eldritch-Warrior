using NWN.Framework.Lite.Enum;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.PlayerCharacter
{
    public static class Extensions
    {
        public static void Scream(this uint pc)
        {
            ApplyEffectToObject(DurationType.Instant, EffectDamage(1, DamageType.Positive, DamagePowerType.PlusTwenty), pc);

            switch (Module.Random.Next(1, 5))
            {
                case 1: PlayVoiceChat(VoiceChatType.Cuss); break;
                case 2: PlayVoiceChat(VoiceChatType.NearDeath); break;
                case 3: PlayVoiceChat(VoiceChatType.Pain1); break;
                case 4: PlayVoiceChat(VoiceChatType.Pain2); break;
                case 5: PlayVoiceChat(VoiceChatType.Pain3); break;
            }
        }

        public static void PlayerHasDied(this uint pc)
        {
            PlayVoiceChat(VoiceChatType.Death);
            ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffectType.Vfx_Imp_Death), pc);
            ApplyEffectToObject(DurationType.Instant, EffectDeath(), pc);
        }
        public static void PlayerHasStabilized(this uint pc)
        {
            PlayVoiceChat(VoiceChatType.GuardMe);
            ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffectType.Vfx_Imp_Healing_S), pc);
            ApplyEffectToObject(DurationType.Instant, EffectResurrection(), pc);
            SendMessageToPC(pc, $"You have stabilized.");
        }

        public static void TryStabilizing(this uint pc)
        {
            int stabilize = Module.Random.D10(1);
            SendMessageToPC(pc, $"Stabilize roll:{stabilize.ToString()}");

            if (GetCurrentHitPoints(pc) <= -127)
            {
                pc.PlayerHasDied();
            }
            else if (stabilize == 1)
            {
                pc.PlayerHasStabilized();
            }
            else
            {
                DelayCommand(1.0f, () => Dying.OnDying());
            }
        }

        public static void Save(this uint pc)
        {
            ExportSingleCharacter(pc);
            SendMessageToPC(pc, "Character Saved");
        }
    }
}