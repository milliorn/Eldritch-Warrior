using System;
using System.Text;
using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;
using NWN.Framework.Lite.NWNX;

namespace Source.ChatSystem
{
    public class PlayerChat
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

        private static bool TriggerChatTools(string message) => message.StartsWith(wildcard);

        private static void Roster(uint pc)
        {
            int playerCount = 0;
            int dmCount = 0;
            StringBuilder stringBuilder = new("Players Online.\n");
            uint player = NWScript.GetFirstPC();

            while (NWScript.GetIsObjectValid(player))
            {
                if (NWScript.GetIsDM(player))
                {
                    dmCount++;
                }
                else
                {
                    playerCount++;
                    stringBuilder.Append($"{NWScript.GetName(player)} | {NWScript.GetArea(player)}\n");
                }
            }

            stringBuilder.Append($"Player Online | {playerCount.ToString()}");
            stringBuilder.Append($"DM Online | {dmCount.ToString()}");
            NWScript.SendMessageToPC(pc, stringBuilder.ToString());
        }

        private static void Router(uint pc, string[] chatArray)
        {
            switch (chatArray[0])
            {
                case "xp":
                    _ = int.TryParse(chatArray[1], out int x);
                    NWScript.SetXP(pc, x);
                    break;
                case "live":
                    NWScript.ApplyEffectToObject(DurationType.Instant, NWScript.EffectHeal(NWScript.GetMaxHitPoints()), pc);
                    break;
                case "dead":
                    NWScript.ApplyEffectToObject(DurationType.Instant, NWScript.EffectDamage(NWScript.GetMaxHitPoints() + 1, DamageType.Positive, DamagePowerType.PlusTwenty), pc);
                    break;
                case "roster":
                    Roster(pc);
                    break;
                case "armbone":
                    SetArmBone(pc);
                    break;
                case "armskin":
                    SetArmNormal(pc);
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
                    NWScript.SpeakString($"{NWScript.GetName(pc)} is looking for a party!", TalkVolumeType.Shout);
                    break;
                case "save":
                    NWScript.ExportSingleCharacter(pc);
                    break;
                case "delete":
                    Administration.DeletePlayerCharacter(pc, true, $"Name:{NWScript.GetName(pc)} | BIC:{Player.GetBicFileName(pc)} deleted.");
                    break;
                default:
                    break;
            }
        }

        private static void SetArmNormal(uint pc)
        {
            NWScript.SetCreatureBodyPart(CreaturePartType.LeftBicep, (int)CreatureModelType.Skin, pc);
            NWScript.SetCreatureBodyPart(CreaturePartType.LeftForearm, (int)CreatureModelType.Skin, pc);
            NWScript.SetCreatureBodyPart(CreaturePartType.LeftHand, (int)CreatureModelType.Skin, pc);
        }

        private static void SetArmBone(uint pc)
        {
            NWScript.SetCreatureBodyPart(CreaturePartType.LeftBicep, (int)CreatureModelType.Undead, pc);
            NWScript.SetCreatureBodyPart(CreaturePartType.LeftForearm, (int)CreatureModelType.Undead, pc);
            NWScript.SetCreatureBodyPart(CreaturePartType.LeftHand, (int)CreatureModelType.Undead, pc);
        }
    }
}