using System;
using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;

namespace EldritchWarrior.Source.PC
{
    public class Dying
    {
        [ScriptHandler("test")]
        public static void OnDying()
        {
            
        }

        public static void Scream(uint pc)
        {
            Effect damage = NWScript.EffectDamage(1, DamageType.Positive, DamagePowerType.PlusTwenty);

            switch (Module.Random.Next(1, 5))
            {
                case 1: NWScript.PlayVoiceChat(VoiceChatType.Cuss); break;
                case 2: NWScript.PlayVoiceChat(VoiceChatType.NearDeath); break;
                case 3: NWScript.PlayVoiceChat(VoiceChatType.Pain1); break;
                case 4: NWScript.PlayVoiceChat(VoiceChatType.Pain2); break;
                case 5: NWScript.PlayVoiceChat(VoiceChatType.Pain3); break;
            }
        }
    }
}