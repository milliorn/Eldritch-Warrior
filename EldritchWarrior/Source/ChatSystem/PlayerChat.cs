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
                    Emote(pc, chatArray);
                    break;
                case "head":
                    SetHead(pc, chatArray);
                    break;
                case "portrait":
                    SetPortrait(pc, chatArray);
                    break;
                case "voice":
                    SetVoice(pc, chatArray);
                    break;
                case "skin":
                    SetSkin(pc, chatArray);
                    break;
                case "hair":
                    SetHair(pc, chatArray);
                    break;
                case "tattoocolor1":
                    SetTattooColor1(pc, chatArray);
                    break;
                case "tattoocolor2":
                    SetTattooColor2(pc, chatArray);
                    break;
                case "tail":
                    SetTail(pc, chatArray);
                    break;
                case "wings":
                    SetWings(pc, chatArray);
                    break;
                case "alignment":
                    SetAlignment(pc, chatArray);
                    break;
                case "resetlevel":
                    ResetLevel(pc, chatArray);
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

        private static void ResetLevel(uint pc, string[] chatArray)
        {
            if (chatArray[1].Equals("one"))
            {
                int hd = NWScript.GetHitDice(pc);
                NWScript.SetXP(pc, (hd * (hd - 1) / 2 * 1000) - 1);
            }
            else if (chatArray[1].Equals("all"))
            {
                int xp = NWScript.GetXP(pc);
                NWScript.SetXP(pc, 0);
                NWScript.SetXP(pc, xp);
            }
            else
            {
                NWScript.SendMessageToPC(pc, $"Cannot reset levels to {chatArray}.");
                throw new ArgumentException($"Name:{NWScript.GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed reset levels to {chatArray}.");
            }
        }

        private static void SetAlignment(uint pc, string[] chatArray)
        {
            switch (chatArray[1])
            {
                case "chaotic": NWScript.AdjustAlignment(pc, AlignmentType.Chaotic, 100, false); break;
                case "evil": NWScript.AdjustAlignment(pc, AlignmentType.Evil, 100, false); break;
                case "good": NWScript.AdjustAlignment(pc, AlignmentType.Good, 100, false); break;
                case "lawful": NWScript.AdjustAlignment(pc, AlignmentType.Lawful, 100, false); break;
                case "neutral": NWScript.AdjustAlignment(pc, AlignmentType.Neutral, 100, false); break;
                default:
                    NWScript.SendMessageToPC(pc, $"Cannot change alignment to {chatArray}."); break;
            }
        }

        private static void SetWings(uint pc, string[] chatArray)
        {
            switch (chatArray[1])
            {
                case "angel": NWScript.SetCreatureWingType(CreatureWingType.Angel, pc); break;
                case "bat": NWScript.SetCreatureWingType(CreatureWingType.Bat, pc); break;
                case "bird": NWScript.SetCreatureWingType(CreatureWingType.Bird, pc); break;
                case "butterfly": NWScript.SetCreatureWingType(CreatureWingType.Butterfly, pc); break;
                case "demon": NWScript.SetCreatureWingType(CreatureWingType.Demon, pc); break;
                case "dragon": NWScript.SetCreatureWingType(CreatureWingType.Dragon, pc); break;
                default:
                    NWScript.SendMessageToPC(pc, $"Cannot change wings to {chatArray}."); break;
            }
        }

        private static void SetTail(uint pc, string[] chatArray)
        {
            switch (chatArray[1])
            {
                case "bone": NWScript.SetCreatureTailType(CreatureTailType.Bone, pc); break;
                case "devil": NWScript.SetCreatureTailType(CreatureTailType.Bone, pc); break;
                case "lizard": NWScript.SetCreatureTailType(CreatureTailType.Bone, pc); break;
                default:
                    NWScript.SendMessageToPC(pc, $"Cannot change tail to {chatArray}."); break;
            }
        }

        private static void SetTattooColor2(uint pc, string[] chatArray)
        {
            _ = int.TryParse(chatArray[1], out int n);
            try
            {
                NWScript.SetColor(pc, ColorChannelType.Tattoo2, n);
            }
            catch (Exception e)
            {
                NWScript.SendMessageToPC(pc, $"Cannot change tattoo 2 color to {chatArray}.");
                throw new ArgumentException($"Exception:{e.GetType()} | Name:{NWScript.GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change tattoo 2 color to {chatArray}.");
            }
        }

        private static void SetTattooColor1(uint pc, string[] chatArray)
        {
            _ = int.TryParse(chatArray[1], out int n);
            try
            {
                NWScript.SetColor(pc, ColorChannelType.Tattoo1, n);
            }
            catch (Exception e)
            {
                NWScript.SendMessageToPC(pc, $"Cannot change tattoo 1 color to {chatArray}.");
                throw new ArgumentException($"Exception:{e.GetType()} | Name:{NWScript.GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change tattoo 1 color to {chatArray}.");
            }
        }

        private static void SetHair(uint pc, string[] chatArray)
        {
            _ = int.TryParse(chatArray[1], out int n);
            try
            {
                NWScript.SetColor(pc, ColorChannelType.Hair, n);
            }
            catch (Exception e)
            {
                NWScript.SendMessageToPC(pc, $"Cannot change hair color to {chatArray}.");
                throw new ArgumentException($"Exception:{e.GetType()} | Name:{NWScript.GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change hair color to {chatArray}.");
            }
        }

        private static void SetSkin(uint pc, string[] chatArray)
        {
            _ = int.TryParse(chatArray[1], out int n);
            try
            {
                NWScript.SetColor(pc, ColorChannelType.Skin, n);
            }
            catch (Exception e)
            {
                NWScript.SendMessageToPC(pc, $"Cannot change skin color to {chatArray}.");
                throw new ArgumentException($"Exception:{e.GetType()} | Name:{NWScript.GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change skin color to {chatArray}.");
            }
        }

        private static void SetVoice(uint pc, string[] chatArray)
        {
            _ = int.TryParse(chatArray[1], out int n);
            try
            {
                Creature.SetSoundset(pc, n);
            }
            catch (Exception e)
            {
                NWScript.SendMessageToPC(pc, $"Cannot change soundset to {chatArray}.");
                throw new ArgumentException($"Exception:{e.GetType()} | Name:{NWScript.GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change soundset to {chatArray}.");
            }
        }

        private static void SetPortrait(uint pc, string[] chatArray)
        {
            _ = int.TryParse(chatArray[1], out int n);
            try
            {
                NWScript.SetPortraitId(pc, n);
            }
            catch (Exception e)
            {
                NWScript.SendMessageToPC(pc, $"Cannot change portrait to {chatArray}.");
                throw new ArgumentException($"Exception:{e.GetType()} | Name:{NWScript.GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change portrait to {chatArray}.");
            }
        }

        private static void SetHead(uint pc, string[] chatArray)
        {
            _ = int.TryParse(chatArray[1], out int n);
            try
            {
                NWScript.SetCreatureBodyPart(CreaturePartType.Head, n, pc);
            }
            catch (Exception e)
            {
                NWScript.SendMessageToPC(pc, $"Cannot change head to {chatArray}.");
                throw new ArgumentException($"Exception:{e.GetType()} | Name:{NWScript.GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change head to {chatArray}.");
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