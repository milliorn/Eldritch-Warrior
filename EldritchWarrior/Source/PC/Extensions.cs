using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;

namespace EldritchWarrior.Source.PC
{
    public static class Extensions
    {
        public static void Scream(this uint pc)
        {
            NWScript.ApplyEffectToObject(DurationType.Instant, NWScript.EffectDamage(1, DamageType.Positive, DamagePowerType.PlusTwenty), pc);

            switch (Module.Random.Next(1, 5))
            {
                case 1: NWScript.PlayVoiceChat(VoiceChatType.Cuss); break;
                case 2: NWScript.PlayVoiceChat(VoiceChatType.NearDeath); break;
                case 3: NWScript.PlayVoiceChat(VoiceChatType.Pain1); break;
                case 4: NWScript.PlayVoiceChat(VoiceChatType.Pain2); break;
                case 5: NWScript.PlayVoiceChat(VoiceChatType.Pain3); break;
            }
        }

        public static void PlayerHasDied(this uint pc)
        {
            NWScript.PlayVoiceChat(VoiceChatType.Death);
            NWScript.ApplyEffectToObject(DurationType.Instant, NWScript.EffectVisualEffect(VisualEffectType.Vfx_Imp_Death), pc);
            NWScript.ApplyEffectToObject(DurationType.Instant, NWScript.EffectDeath(), pc);
        }
        public static void PlayerHasStabilized(this uint pc)
        {
            NWScript.PlayVoiceChat(VoiceChatType.GuardMe);
            NWScript.ApplyEffectToObject(DurationType.Instant, NWScript.EffectVisualEffect(VisualEffectType.Vfx_Imp_Healing_S), pc);
            NWScript.ApplyEffectToObject(DurationType.Instant, NWScript.EffectResurrection(), pc);
            NWScript.SendMessageToPC(pc, $"You have stabilized.");
        }

        public static void TryStabilizing(this uint pc)
        {
            int stabilize = Module.Random.D10(1);
            NWScript.SendMessageToPC(pc, $"Stabilize roll:{stabilize.ToString()}");

            if (NWScript.GetCurrentHitPoints(pc) <= -127)
            {
                pc.PlayerHasDied();
            }
            else if (stabilize == 1)
            {
                pc.PlayerHasStabilized();
            }
            else
            {
                NWScript.DelayCommand(1.0f, () => Dying.OnDying());
            }
        }
    }
}