using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;
using NWN.Framework.Lite.NWNX;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.ChatSystem
{
    public class PlayerChat
    {
        private static readonly char wildcard = '!';
        private static bool TriggerChatTools(string message) => message.StartsWith(wildcard);


        [ScriptHandler("mod_chat")]
        public static void OnPlayerChat()
        {
            string message = GetPCChatMessage();
            uint pc = GetPCChatSpeaker();

            if (TriggerChatTools(message))
            {
                message = message[1..].ToLower();
                Router(pc, message.Split(' '));
            }
        }

        private static void Router(uint pc, string[] chatArray)
        {
            switch (chatArray[0])
            {
                case "xp":
                    _ = int.TryParse(chatArray[1], out int x);
                    SetXP(pc, x);
                    break;
                case "live":
                    ApplyEffectToObject(DurationType.Instant, EffectHeal(GetMaxHitPoints()), pc);
                    break;
                case "dead":
                    ApplyEffectToObject(DurationType.Instant, EffectDamage(GetMaxHitPoints() + 1, DamageType.Positive, DamagePowerType.PlusTwenty), pc);
                    break;
                case "roster":
                    pc.Roster();
                    break;
                case "armbone":
                    pc.SetArmBone();
                    break;
                case "armskin":
                    pc.SetArmNormal();
                    break;
                case "*":
                    pc.Emote(chatArray);
                    break;
                case "head":
                    pc.SetHead(chatArray);
                    break;
                case "portrait":
                    pc.SetPortrait(chatArray);
                    break;
                case "voice":
                    pc.SetVoice(chatArray);
                    break;
                case "skin":
                    pc.SetSkin(chatArray);
                    break;
                case "hair":
                    pc.SetHair(chatArray);
                    break;
                case "tattoocolor1":
                    pc.SetTattooColor1(chatArray);
                    break;
                case "tattoocolor2":
                    pc.SetTattooColor2(chatArray);
                    break;
                case "tail":
                    pc.SetTail(chatArray);
                    break;
                case "wings":
                    pc.SetWings(chatArray);
                    break;
                case "alignment":
                    pc.SetAlignment(chatArray);
                    break;
                case "resetlevel":
                    pc.ResetLevel(chatArray);
                    break;
                case "roll":
                    pc.RollDice(chatArray);
                    break;
                case "status":
                    pc.SetStatus(chatArray);
                    break;
                case "eyes":
                    pc.SetEyes(chatArray);
                    break;
                case "visual":
                    pc.SetVisual(chatArray);
                    break;
                case "lfg":
                    SpeakString($"{GetName(pc)} is looking for a party!", TalkVolumeType.Shout);
                    break;
                case "save":
                    ExportSingleCharacter(pc);
                    break;
                case "delete":
                    Administration.DeletePlayerCharacter(pc, true, $"Name:{GetName(pc)} | BIC:{Player.GetBicFileName(pc)} deleted.");
                    break;
                default:
                    break;
            }
        }
    }
}