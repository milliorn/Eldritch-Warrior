using System;
using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;
using NWN.Framework.Lite.NWNX;
using NWN.Framework.Lite.NWNX.Enum;

namespace Source.ChatSystem
{
    public class Player
    {
        private static readonly char wildcard = '!';

        [ScriptHandler("on_player_chat")]
        public static void OnPlayerChat()
        {
            string message = NWScript.GetPCChatMessage();
            uint pc = NWScript.GetPCChatSpeaker();

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
                    //chat.Sender.Xp += x;
                    NWScript.SetXP(pc, x);
                    break;
                case "live":
                    chat.Sender.HP = chat.Sender.HP = chat.Sender.MaxHP;
                    break;
                case "dead":
                    chat.Sender.ApplyEffect(EffectDuration.Instant, NWN.API.Effect.Damage(1));
                    break;
                case "roster":
                    Roster(chat);
                    break;
                case "armbone":
                    SetArmBone(chat);
                    break;
                case "armskin":
                    SetArmNormal(chat);
                    break;
                case "!":
                    Emote(chat, chatArray);
                    break;
                case "head":
                    SetHead(chat, chatArray);
                    break;
                case "portrait":
                    SetPortrait(chat, chatArray);
                    break;
                case "voice":
                    SetVoice(chat, chatArray);
                    break;
                case "skin":
                    SetSkin(chat, chatArray);
                    break;
                case "hair":
                    SetHair(chat, chatArray);
                    break;
                case "tattoocolor1":
                    SetTattooColor1(chat, chatArray);
                    break;
                case "tattoocolor2":
                    SetTattooColor2(chat, chatArray);
                    break;
                case "tail":
                    SetTail(chat, chatArray);
                    break;
                case "wings":
                    SetWings(chat, chatArray);
                    break;
                case "alignment":
                    SetAlignment(chat, chatArray);
                    break;
                case "resetlevel":
                    ResetLevel(chat, chatArray);
                    break;
                case "roll":
                    RollDice(chat, chatArray);
                    break;
                case "status":
                    SetStatus(chat, chatArray);
                    break;
                case "eyes":
                    SetEyes(chat, chatArray);
                    break;
                case "visual":
                    SetVisual(chat, chatArray);
                    break;
                case "lfg":
                    NwModule.Instance.SpeakString($"{chat.Sender.Name.ColorString(Color.WHITE)} is looking for a party!", TalkVolume.Shout);
                    break;
                case "save":
                    chat.Sender.ExportCharacter();
                    break;
                case "delete":
                    chat.Sender.Delete($"{chat.Sender.BicFileName}.bic has been deleted.");
                    break;
                default:
                    break;
            }
        }

        private static bool TriggerChatTools(string message) => message.StartsWith(wildcard);
    }
}