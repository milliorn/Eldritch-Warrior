using System;
using System.Text;
using NWN.Framework.Lite;
using NWN.Framework.Lite.Bioware;
using NWN.Framework.Lite.Enum;
using NWN.Framework.Lite.NWNX;
using ItemProperty = NWN.Framework.Lite.ItemProperty;
using System.Globalization;
using Effect = NWN.Framework.Lite.Effect;

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
                case "*":
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
                    RollDice(pc, chatArray);
                    break;
                case "status":
                    SetStatus(pc, chatArray);
                    break;
                case "eyes":
                    SetEyes(pc, chatArray);
                    break;
                case "visual":
                    SetVisual(pc, chatArray);
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

        private static void SetStatus(uint pc, string[] chatArray)
        {
            if (chatArray[1].Equals("like") || chatArray[1].Equals("dislike"))
            {
                uint player = NWScript.GetFirstPC();

                while (NWScript.GetIsObjectValid(player))
                {
                    if (chatArray[1].Equals("like"))
                    {
                        NWScript.SetPCLike(pc, player);
                    }
                    else if (chatArray[1].Equals("dislike"))
                    {
                        NWScript.SetPCDislike(pc, player);
                    }
                }
            }
            else
            {
                NWScript.SendMessageToPC(pc, $"Cannot reset status to {chatArray}.");
                throw new ArgumentException($"Name:{NWScript.GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to reset status to {chatArray}.");
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
                NWScript.DelayCommand(1.0f, () => NWScript.SetXP(pc, xp));
            }
            else
            {
                NWScript.SendMessageToPC(pc, $"Cannot reset levels to {chatArray}.");
                throw new ArgumentException($"Name:{NWScript.GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed reset levels to {chatArray}.");
            }
        }

        private static void Emote(uint pc, string[] chatArray)
        {
            if (float.TryParse(chatArray[2].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out float animSpeed))
            {
                switch (chatArray[1])
                {
                    case "back": NWScript.PlayAnimation(AnimationType.LoopingDeadBack, animSpeed); break;
                    case "beg": NWScript.PlayAnimation(AnimationType.LoopingTalkPleading, animSpeed); break;
                    case "bored": NWScript.PlayAnimation(AnimationType.FireForgetPauseBored, animSpeed); break;
                    case "bow": NWScript.PlayAnimation(AnimationType.FireForgetBow, animSpeed); break;
                    case "c1": NWScript.PlayAnimation(AnimationType.LoopingConjure1, animSpeed); break;
                    case "c2": NWScript.PlayAnimation(AnimationType.LoopingConjure2, animSpeed); break;
                    case "dodge": NWScript.PlayAnimation(AnimationType.FireForgetDodgeSide, animSpeed); break;
                    case "drink": NWScript.PlayAnimation(AnimationType.FireForgetDrink, animSpeed); break;
                    case "drunk": NWScript.PlayAnimation(AnimationType.LoopingPauseDrunk, animSpeed); break;
                    case "duck": NWScript.PlayAnimation(AnimationType.FireForgetDodgeDuck, animSpeed); break;
                    case "forceful": NWScript.PlayAnimation(AnimationType.LoopingTalkForceful, animSpeed); break;
                    case "front": NWScript.PlayAnimation(AnimationType.LoopingDeadFront, animSpeed); break;
                    case "greet": NWScript.PlayAnimation(AnimationType.FireForgetGreeting, animSpeed); break;
                    case "left": NWScript.PlayAnimation(AnimationType.FireForgetHeadTurnLeft, animSpeed); break;
                    case "listen": NWScript.PlayAnimation(AnimationType.LoopingListen, animSpeed); break;
                    case "lol": NWScript.PlayAnimation(AnimationType.LoopingTalkLaughing, animSpeed); break;
                    case "look": NWScript.PlayAnimation(AnimationType.LoopingLookFar, animSpeed); break;
                    case "low": NWScript.PlayAnimation(AnimationType.LoopingGetLow, animSpeed); break;
                    case "meditate": NWScript.PlayAnimation(AnimationType.LoopingMeditate, animSpeed); break;
                    case "mid": NWScript.PlayAnimation(AnimationType.LoopingGetMid, animSpeed); break;
                    case "normal": NWScript.PlayAnimation(AnimationType.LoopingTalkNormal, animSpeed); break;
                    case "p1": NWScript.PlayAnimation(AnimationType.LoopingPause, animSpeed); break;
                    case "p2": NWScript.PlayAnimation(AnimationType.LoopingPause2, animSpeed); break;
                    case "read": NWScript.PlayAnimation(AnimationType.FireForgetRead, animSpeed); break;
                    case "right": NWScript.PlayAnimation(AnimationType.FireForgetHeadTurnRight, animSpeed); break;
                    case "salute": NWScript.PlayAnimation(AnimationType.FireForgetSalute, animSpeed); break;
                    case "scratch": NWScript.PlayAnimation(AnimationType.FireForgetPauseScratchHead, animSpeed); break;
                    case "shake": NWScript.PlayAnimation(AnimationType.FireForgetSpasm, animSpeed); break;
                    case "sit": NWScript.PlayAnimation(AnimationType.LoopingSitCross, animSpeed); break;
                    case "spasm": NWScript.PlayAnimation(AnimationType.LoopingSpasm, animSpeed); break;
                    case "squat": NWScript.PlayAnimation(AnimationType.LoopingSitChair, animSpeed); break;
                    case "steal": NWScript.PlayAnimation(AnimationType.FireForgetSteal, animSpeed); break;
                    case "taunt": NWScript.PlayAnimation(AnimationType.FireForgetTaunt, animSpeed); break;
                    case "tired": NWScript.PlayAnimation(AnimationType.LoopingPauseTired, animSpeed); break;
                    case "v1": NWScript.PlayAnimation(AnimationType.FireForgetVictory1, animSpeed); break;
                    case "v2": NWScript.PlayAnimation(AnimationType.FireForgetVictory2, animSpeed); break;
                    case "v3": NWScript.PlayAnimation(AnimationType.FireForgetVictory3, animSpeed); break;
                    case "worship": NWScript.PlayAnimation(AnimationType.LoopingWorship, animSpeed); break;
                    default: break;
                }
            }
        }

        private static void SetVisual(uint pc, string[] chatArray)
        {
            var item = NWScript.GetItemInSlot(InventorySlotType.RightHand, pc);
            if (NWScript.GetIsObjectValid(item))
            {
                BiowareXP2.IPRemoveMatchingItemProperties(item, ItemPropertyType.Visualeffect, DurationType.Permanent, -1);
                ItemProperty type;

                switch (chatArray[1])
                {
                    case "acid": type = NWScript.ItemPropertyVisualEffect(ItemVisualType.Acid); break;
                    case "cold": type = NWScript.ItemPropertyVisualEffect(ItemVisualType.Cold); break;
                    case "electric": type = NWScript.ItemPropertyVisualEffect(ItemVisualType.Electrical); break;
                    case "evil": type = NWScript.ItemPropertyVisualEffect(ItemVisualType.Evil); break;
                    case "fire": type = NWScript.ItemPropertyVisualEffect(ItemVisualType.Fire); break;
                    case "holy": type = NWScript.ItemPropertyVisualEffect(ItemVisualType.Holy); break;
                    case "sonic": type = NWScript.ItemPropertyVisualEffect(ItemVisualType.Sonic); break;
                    default:
                        NWScript.SendMessageToPC(pc, $"Cannot set weapon visual to {chatArray}.");
                        throw new ArgumentException($"Name:{NWScript.GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to set weapon visual to {chatArray}.");
                }

                BiowareXP2.IPSafeAddItemProperty(item, type, 0.0f, AddItemPropertyPolicy.ReplaceExisting, true, true);
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

        private static void RollDice(uint pc, string[] chatArray)
        {
            _ = int.TryParse(chatArray[1], out int n);
            try
            {
                int dice = Module.Random.Next(1, n);
                NWScript.SpeakString($"{NWScript.GetName(pc)} rolled a d{n} and got {dice}.", TalkVolumeType.Shout);
            }
            catch (Exception e)
            {
                NWScript.SendMessageToPC(pc, $"Cannot roll dice with {chatArray}.");
                throw new ArgumentException($"Exception:{e.GetType()} | Name:{NWScript.GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to roll dice with {chatArray}.");
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

        private static void SetEyes(uint pc, string[] chatArray)
        {
            string color = chatArray[1];
            RacialType race = NWScript.GetRacialType(pc);

            switch (color)
            {
                case "cyan": NWScript.ApplyEffectToObject(DurationType.Permanent, SetEyesCyan(pc), pc); break;
                case "green": NWScript.ApplyEffectToObject(DurationType.Permanent, SetEyesGreen(pc), pc); break;
                case "orange": NWScript.ApplyEffectToObject(DurationType.Permanent, SetEyesOrange(pc), pc); break;
                case "purple": NWScript.ApplyEffectToObject(DurationType.Permanent, SetEyesPurple(pc), pc); break;
                case "white": NWScript.ApplyEffectToObject(DurationType.Permanent, SetEyesWhite(pc), pc); break;
                case "yellow": NWScript.ApplyEffectToObject(DurationType.Permanent, SetEyesYellow(pc), pc); break;
                default: break;
            }
        }

        private static Effect SetEyesCyan(uint pc)
        {
            Effect eyeColor;
            GenderType gender = NWScript.GetGender(pc);
            RacialType race = NWScript.GetRacialType(pc);

            if (race == RacialType.Dwarf)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Dwarf_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Dwarf_Male);
            }
            else if (race == RacialType.Elf)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Elf_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Elf_Male);
            }
            else if (race == RacialType.Gnome)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Gnome_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Gnome_Male);
            }
            else if (race == RacialType.Halfelf)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Troglodyte) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Troglodyte);
            }
            else if (race == RacialType.Halfling)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Halfling_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Halfling_Male);
            }
            else if (race == RacialType.Halforc)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Halforc_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Halforc_Male);
            }
            else if (race == RacialType.Human)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Human_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Human_Male);
            }
            else
            {
                NWScript.SendMessageToPC(pc, $"Invalid Race {race}. SetEyesCyan.");
                throw new ArgumentException($"Name:{NWScript.GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change SetEyesCyan. Invalid race {race}.");
            }

            return eyeColor;
        }

        private static Effect SetEyesOrange(uint pc)
        {
            Effect eyeColor;
            GenderType gender = NWScript.GetGender(pc);
            RacialType race = NWScript.GetRacialType(pc);

            if (race == RacialType.Dwarf)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Dwarf_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Dwarf_Male);
            }
            else if (race == RacialType.Elf)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Elf_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Elf_Male);
            }
            else if (race == RacialType.Gnome)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Gnome_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Gnome_Male);
            }
            else if (race == RacialType.Halfelf)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Troglodyte) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Troglodyte);
            }
            else if (race == RacialType.Halfling)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Halfling_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Halfling_Male);
            }
            else if (race == RacialType.Halforc)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Halforc_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Halforc_Male);
            }
            else if (race == RacialType.Human)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Human_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Human_Male);
            }
            else
            {
                NWScript.SendMessageToPC(pc, $"Invalid Race {race}. SetEyesOrange.");
                throw new ArgumentException($"Name:{NWScript.GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change SetEyesOrange. Invalid race {race}.");
            }

            return eyeColor;
        }

        private static Effect SetEyesPurple(uint pc)
        {
            Effect eyeColor;
            GenderType gender = NWScript.GetGender(pc);
            RacialType race = NWScript.GetRacialType(pc);

            if (race == RacialType.Dwarf)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Dwarf_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Dwarf_Male);
            }
            else if (race == RacialType.Elf)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Elf_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Elf_Male);
            }
            else if (race == RacialType.Gnome)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Gnome_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Gnome_Male);
            }
            else if (race == RacialType.Halfelf)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Troglodyte) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Troglodyte);
            }
            else if (race == RacialType.Halfling)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Halfling_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Halfling_Male);
            }
            else if (race == RacialType.Halforc)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Halforc_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Halforc_Male);
            }
            else if (race == RacialType.Human)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Human_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Human_Male);
            }
            else
            {
                NWScript.SendMessageToPC(pc, $"Invalid Race {race}. SetEyesOrange.");
                throw new ArgumentException($"Name:{NWScript.GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change SetEyesOrange. Invalid race {race}.");
            }

            return eyeColor;
        }

        private static Effect SetEyesWhite(uint pc)
        {
            Effect eyeColor;
            GenderType gender = NWScript.GetGender(pc);
            RacialType race = NWScript.GetRacialType(pc);

            if (race == RacialType.Dwarf)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Dwarf_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Dwarf_Male);
            }
            else if (race == RacialType.Elf)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Elf_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Elf_Male);
            }
            else if (race == RacialType.Gnome)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Gnome_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Gnome_Male);
            }
            else if (race == RacialType.Halfelf)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Troglodyte) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Troglodyte);
            }
            else if (race == RacialType.Halfling)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Halfling_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Halfling_Male);
            }
            else if (race == RacialType.Halforc)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Halforc_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Halforc_Male);
            }
            else if (race == RacialType.Human)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Human_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Human_Male);
            }
            else
            {
                NWScript.SendMessageToPC(pc, $"Invalid Race {race}. SetEyesOrange.");
                throw new ArgumentException($"Name:{NWScript.GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change SetEyesOrange. Invalid race {race}.");
            }

            return eyeColor;
        }

        private static Effect SetEyesYellow(uint pc)
        {
            Effect eyeColor;
            GenderType gender = NWScript.GetGender(pc);
            RacialType race = NWScript.GetRacialType(pc);

            if (race == RacialType.Dwarf)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Dwarf_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Dwarf_Male);
            }
            else if (race == RacialType.Elf)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Elf_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Elf_Male);
            }
            else if (race == RacialType.Gnome)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Gnome_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Gnome_Male);
            }
            else if (race == RacialType.Halfelf)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Troglodyte) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Troglodyte);
            }
            else if (race == RacialType.Halfling)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Halfling_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Halfling_Male);
            }
            else if (race == RacialType.Halforc)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Halforc_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Halforc_Male);
            }
            else if (race == RacialType.Human)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Human_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Human_Male);
            }
            else
            {
                NWScript.SendMessageToPC(pc, $"Invalid Race {race}. SetEyesOrange.");
                throw new ArgumentException($"Name:{NWScript.GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change SetEyesOrange. Invalid race {race}.");
            }

            return eyeColor;
        }

        private static Effect SetEyesGreen(uint pc)
        {
            Effect eyeColor;
            GenderType gender = NWScript.GetGender(pc);
            RacialType race = NWScript.GetRacialType(pc);

            if (race == RacialType.Dwarf)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Dwarf_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Dwarf_Male);
            }
            else if (race == RacialType.Elf)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Elf_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Elf_Male);
            }
            else if (race == RacialType.Gnome)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Gnome_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Gnome_Male);
            }
            else if (race == RacialType.Halfelf)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Troglodyte) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Troglodyte);
            }
            else if (race == RacialType.Halfling)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Halfling_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Halfling_Male);
            }
            else if (race == RacialType.Halforc)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Halforc_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Halforc_Male);
            }
            else if (race == RacialType.Human)
            {
                eyeColor = gender == GenderType.Female ? NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Human_Female) : NWScript.EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Human_Male);
            }
            else
            {
                NWScript.SendMessageToPC(pc, $"Invalid Race {race}. SetEyesOrange.");
                throw new ArgumentException($"Name:{NWScript.GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change SetEyesOrange. Invalid race {race}.");
            }

            return eyeColor;
        }
    }
}