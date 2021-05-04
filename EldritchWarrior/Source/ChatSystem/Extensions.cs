using System;
using System.Text;

using NWN.Framework.Lite.Bioware;
using NWN.Framework.Lite.Enum;

using NWN.Framework.Lite.NWNX;
using ItemProperty = NWN.Framework.Lite.ItemProperty;
using System.Globalization;
using Effect = NWN.Framework.Lite.Effect;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.ChatSystem
{
    public static class Extensions
    {
        public static void Roster(this uint pc)
        {
            int playerCount = 0;
            int dmCount = 0;
            StringBuilder stringBuilder = new("Players Online.\n");
            uint player = GetFirstPC();

            while (GetIsObjectValid(player))
            {
                if (GetIsDM(player))
                {
                    dmCount++;
                }
                else
                {
                    playerCount++;
                    stringBuilder.Append($"{GetName(player)} | {GetArea(player)}\n");
                }
            }

            stringBuilder.Append($"Player Online | {playerCount.ToString()}");
            stringBuilder.Append($"DM Online | {dmCount.ToString()}");
            SendMessageToPC(pc, stringBuilder.ToString());
        }

        public static void SetStatus(this uint pc, string[] chatArray)
        {
            if (chatArray[1].Equals("like") || chatArray[1].Equals("dislike"))
            {
                uint player = GetFirstPC();

                while (GetIsObjectValid(player))
                {
                    if (chatArray[1].Equals("like"))
                    {
                        SetPCLike(pc, player);
                    }
                    else if (chatArray[1].Equals("dislike"))
                    {
                        SetPCDislike(pc, player);
                    }
                }
            }
            else
            {
                SendMessageToPC(pc, $"Cannot reset status to {chatArray}.");
                throw new ArgumentException($"Name:{GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to reset status to {chatArray}.");
            }
        }

        public static void ResetLevel(this uint pc, string[] chatArray)
        {
            if (chatArray[1].Equals("one"))
            {
                int hd = GetHitDice(pc);
                SetXP(pc, (hd * (hd - 1) / 2 * 1000) - 1);
            }
            else if (chatArray[1].Equals("all"))
            {
                int xp = GetXP(pc);
                SetXP(pc, 0);
                DelayCommand(1.0f, () => SetXP(pc, xp));
            }
            else
            {
                SendMessageToPC(pc, $"Cannot reset levels to {chatArray}.");
                throw new ArgumentException($"Name:{GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed reset levels to {chatArray}.");
            }
        }

        public static void Emote(this uint pc, string[] chatArray)
        {
            if (float.TryParse(chatArray[2].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out float animSpeed))
            {
                switch (chatArray[1])
                {
                    case "back": PlayAnimation(AnimationType.LoopingDeadBack, animSpeed); break;
                    case "beg": PlayAnimation(AnimationType.LoopingTalkPleading, animSpeed); break;
                    case "bored": PlayAnimation(AnimationType.FireForgetPauseBored, animSpeed); break;
                    case "bow": PlayAnimation(AnimationType.FireForgetBow, animSpeed); break;
                    case "c1": PlayAnimation(AnimationType.LoopingConjure1, animSpeed); break;
                    case "c2": PlayAnimation(AnimationType.LoopingConjure2, animSpeed); break;
                    case "dodge": PlayAnimation(AnimationType.FireForgetDodgeSide, animSpeed); break;
                    case "drink": PlayAnimation(AnimationType.FireForgetDrink, animSpeed); break;
                    case "drunk": PlayAnimation(AnimationType.LoopingPauseDrunk, animSpeed); break;
                    case "duck": PlayAnimation(AnimationType.FireForgetDodgeDuck, animSpeed); break;
                    case "forceful": PlayAnimation(AnimationType.LoopingTalkForceful, animSpeed); break;
                    case "front": PlayAnimation(AnimationType.LoopingDeadFront, animSpeed); break;
                    case "greet": PlayAnimation(AnimationType.FireForgetGreeting, animSpeed); break;
                    case "left": PlayAnimation(AnimationType.FireForgetHeadTurnLeft, animSpeed); break;
                    case "listen": PlayAnimation(AnimationType.LoopingListen, animSpeed); break;
                    case "lol": PlayAnimation(AnimationType.LoopingTalkLaughing, animSpeed); break;
                    case "look": PlayAnimation(AnimationType.LoopingLookFar, animSpeed); break;
                    case "low": PlayAnimation(AnimationType.LoopingGetLow, animSpeed); break;
                    case "meditate": PlayAnimation(AnimationType.LoopingMeditate, animSpeed); break;
                    case "mid": PlayAnimation(AnimationType.LoopingGetMid, animSpeed); break;
                    case "normal": PlayAnimation(AnimationType.LoopingTalkNormal, animSpeed); break;
                    case "p1": PlayAnimation(AnimationType.LoopingPause, animSpeed); break;
                    case "p2": PlayAnimation(AnimationType.LoopingPause2, animSpeed); break;
                    case "read": PlayAnimation(AnimationType.FireForgetRead, animSpeed); break;
                    case "right": PlayAnimation(AnimationType.FireForgetHeadTurnRight, animSpeed); break;
                    case "salute": PlayAnimation(AnimationType.FireForgetSalute, animSpeed); break;
                    case "scratch": PlayAnimation(AnimationType.FireForgetPauseScratchHead, animSpeed); break;
                    case "shake": PlayAnimation(AnimationType.FireForgetSpasm, animSpeed); break;
                    case "sit": PlayAnimation(AnimationType.LoopingSitCross, animSpeed); break;
                    case "spasm": PlayAnimation(AnimationType.LoopingSpasm, animSpeed); break;
                    case "squat": PlayAnimation(AnimationType.LoopingSitChair, animSpeed); break;
                    case "steal": PlayAnimation(AnimationType.FireForgetSteal, animSpeed); break;
                    case "taunt": PlayAnimation(AnimationType.FireForgetTaunt, animSpeed); break;
                    case "tired": PlayAnimation(AnimationType.LoopingPauseTired, animSpeed); break;
                    case "v1": PlayAnimation(AnimationType.FireForgetVictory1, animSpeed); break;
                    case "v2": PlayAnimation(AnimationType.FireForgetVictory2, animSpeed); break;
                    case "v3": PlayAnimation(AnimationType.FireForgetVictory3, animSpeed); break;
                    case "worship": PlayAnimation(AnimationType.LoopingWorship, animSpeed); break;
                    default: break;
                }
            }
        }

        public static void SetVisual(this uint pc, string[] chatArray)
        {
            var item = GetItemInSlot(InventorySlotType.RightHand, pc);
            if (GetIsObjectValid(item))
            {
                BiowareXP2.IPRemoveMatchingItemProperties(item, ItemPropertyType.Visualeffect, DurationType.Permanent, -1);
                ItemProperty type;

                switch (chatArray[1])
                {
                    case "acid": type = ItemPropertyVisualEffect(ItemVisualType.Acid); break;
                    case "cold": type = ItemPropertyVisualEffect(ItemVisualType.Cold); break;
                    case "electric": type = ItemPropertyVisualEffect(ItemVisualType.Electrical); break;
                    case "evil": type = ItemPropertyVisualEffect(ItemVisualType.Evil); break;
                    case "fire": type = ItemPropertyVisualEffect(ItemVisualType.Fire); break;
                    case "holy": type = ItemPropertyVisualEffect(ItemVisualType.Holy); break;
                    case "sonic": type = ItemPropertyVisualEffect(ItemVisualType.Sonic); break;
                    default:
                        SendMessageToPC(pc, $"Cannot set weapon visual to {chatArray}.");
                        throw new ArgumentException($"Name:{GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to set weapon visual to {chatArray}.");
                }

                BiowareXP2.IPSafeAddItemProperty(item, type, 0.0f, AddItemPropertyPolicy.ReplaceExisting, true, true);
            }
        }

        public static void SetAlignment(this uint pc, string[] chatArray)
        {
            switch (chatArray[1])
            {
                case "chaotic": AdjustAlignment(pc, AlignmentType.Chaotic, 100, false); break;
                case "evil": AdjustAlignment(pc, AlignmentType.Evil, 100, false); break;
                case "good": AdjustAlignment(pc, AlignmentType.Good, 100, false); break;
                case "lawful": AdjustAlignment(pc, AlignmentType.Lawful, 100, false); break;
                case "neutral": AdjustAlignment(pc, AlignmentType.Neutral, 100, false); break;
                default:
                    SendMessageToPC(pc, $"Cannot change alignment to {chatArray}."); break;
            }
        }

        public static void SetWings(this uint pc, string[] chatArray)
        {
            switch (chatArray[1])
            {
                case "angel": SetCreatureWingType(CreatureWingType.Angel, pc); break;
                case "bat": SetCreatureWingType(CreatureWingType.Bat, pc); break;
                case "bird": SetCreatureWingType(CreatureWingType.Bird, pc); break;
                case "butterfly": SetCreatureWingType(CreatureWingType.Butterfly, pc); break;
                case "demon": SetCreatureWingType(CreatureWingType.Demon, pc); break;
                case "dragon": SetCreatureWingType(CreatureWingType.Dragon, pc); break;
                default:
                    SendMessageToPC(pc, $"Cannot change wings to {chatArray}."); break;
            }
        }

        public static void SetTail(this uint pc, string[] chatArray)
        {
            switch (chatArray[1])
            {
                case "bone": SetCreatureTailType(CreatureTailType.Bone, pc); break;
                case "devil": SetCreatureTailType(CreatureTailType.Bone, pc); break;
                case "lizard": SetCreatureTailType(CreatureTailType.Bone, pc); break;
                default:
                    SendMessageToPC(pc, $"Cannot change tail to {chatArray}."); break;
            }
        }

        public static void RollDice(this uint pc, string[] chatArray)
        {
            _ = int.TryParse(chatArray[1], out int n);
            try
            {
                SpeakString($"{GetName(pc)} rolled a d{n} and got {Module.Random.Next(1, n)}.", TalkVolumeType.Shout);
            }
            catch (Exception e)
            {
                SendMessageToPC(pc, $"Cannot roll dice with {chatArray}.");
                throw new ArgumentException($"Exception:{e.GetType()} | Name:{GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to roll dice with {chatArray}.");
            }
        }

        public static void SetTattooColor2(this uint pc, string[] chatArray)
        {
            _ = int.TryParse(chatArray[1], out int n);
            try
            {
                SetColor(pc, ColorChannelType.Tattoo2, n);
            }
            catch (Exception e)
            {
                SendMessageToPC(pc, $"Cannot change tattoo 2 color to {chatArray}.");
                throw new ArgumentException($"Exception:{e.GetType()} | Name:{GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change tattoo 2 color to {chatArray}.");
            }
        }

        public static void SetTattooColor1(this uint pc, string[] chatArray)
        {
            _ = int.TryParse(chatArray[1], out int n);
            try
            {
                SetColor(pc, ColorChannelType.Tattoo1, n);
            }
            catch (Exception e)
            {
                SendMessageToPC(pc, $"Cannot change tattoo 1 color to {chatArray}.");
                throw new ArgumentException($"Exception:{e.GetType()} | Name:{GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change tattoo 1 color to {chatArray}.");
            }
        }

        public static void SetHair(this uint pc, string[] chatArray)
        {
            _ = int.TryParse(chatArray[1], out int n);
            try
            {
                SetColor(pc, ColorChannelType.Hair, n);
            }
            catch (Exception e)
            {
                SendMessageToPC(pc, $"Cannot change hair color to {chatArray}.");
                throw new ArgumentException($"Exception:{e.GetType()} | Name:{GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change hair color to {chatArray}.");
            }
        }

        public static void SetSkin(this uint pc, string[] chatArray)
        {
            _ = int.TryParse(chatArray[1], out int n);
            try
            {
                SetColor(pc, ColorChannelType.Skin, n);
            }
            catch (Exception e)
            {
                SendMessageToPC(pc, $"Cannot change skin color to {chatArray}.");
                throw new ArgumentException($"Exception:{e.GetType()} | Name:{GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change skin color to {chatArray}.");
            }
        }

        public static void SetVoice(this uint pc, string[] chatArray)
        {
            _ = int.TryParse(chatArray[1], out int n);
            try
            {
                Creature.SetSoundset(pc, n);
            }
            catch (Exception e)
            {
                SendMessageToPC(pc, $"Cannot change soundset to {chatArray}.");
                throw new ArgumentException($"Exception:{e.GetType()} | Name:{GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change soundset to {chatArray}.");
            }
        }

        public static void SetPortrait(this uint pc, string[] chatArray)
        {
            _ = int.TryParse(chatArray[1], out int n);
            try
            {
                SetPortraitId(pc, n);
            }
            catch (Exception e)
            {
                SendMessageToPC(pc, $"Cannot change portrait to {chatArray}.");
                throw new ArgumentException($"Exception:{e.GetType()} | Name:{GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change portrait to {chatArray}.");
            }
        }

        public static void SetHead(this uint pc, string[] chatArray)
        {
            _ = int.TryParse(chatArray[1], out int n);
            try
            {
                SetCreatureBodyPart(CreaturePartType.Head, n, pc);
            }
            catch (Exception e)
            {
                SendMessageToPC(pc, $"Cannot change head to {chatArray}.");
                throw new ArgumentException($"Exception:{e.GetType()} | Name:{GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change head to {chatArray}.");
            }
        }

        public static void SetArmNormal(this uint pc)
        {
            SetCreatureBodyPart(CreaturePartType.LeftBicep, (int)CreatureModelType.Skin, pc);
            SetCreatureBodyPart(CreaturePartType.LeftForearm, (int)CreatureModelType.Skin, pc);
            SetCreatureBodyPart(CreaturePartType.LeftHand, (int)CreatureModelType.Skin, pc);
        }

        public static void SetArmBone(this uint pc)
        {
            SetCreatureBodyPart(CreaturePartType.LeftBicep, (int)CreatureModelType.Undead, pc);
            SetCreatureBodyPart(CreaturePartType.LeftForearm, (int)CreatureModelType.Undead, pc);
            SetCreatureBodyPart(CreaturePartType.LeftHand, (int)CreatureModelType.Undead, pc);
        }

        public static void SetEyes(this uint pc, string[] chatArray)
        {
            switch (chatArray[1])
            {
                case "cyan": ApplyEffectToObject(DurationType.Permanent, SetEyesCyan(pc), pc); break;
                case "green": ApplyEffectToObject(DurationType.Permanent, SetEyesGreen(pc), pc); break;
                case "orange": ApplyEffectToObject(DurationType.Permanent, SetEyesOrange(pc), pc); break;
                case "purple": ApplyEffectToObject(DurationType.Permanent, SetEyesPurple(pc), pc); break;
                case "red": ApplyEffectToObject(DurationType.Permanent, SetEyesRed(pc), pc); break;
                case "white": ApplyEffectToObject(DurationType.Permanent, SetEyesWhite(pc), pc); break;
                case "yellow": ApplyEffectToObject(DurationType.Permanent, SetEyesYellow(pc), pc); break;
                default: break;
            }
        }

        public static Effect SetEyesCyan(this uint pc)
        {
            Effect eyeColor;
            GenderType gender = GetGender(pc);
            RacialType race = GetRacialType(pc);

            if (race == RacialType.Dwarf)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Dwarf_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Dwarf_Male);
            }
            else if (race == RacialType.Elf)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Elf_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Elf_Male);
            }
            else if (race == RacialType.Gnome)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Gnome_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Gnome_Male);
            }
            else if (race == RacialType.Halfelf)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Troglodyte) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Troglodyte);
            }
            else if (race == RacialType.Halfling)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Halfling_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Halfling_Male);
            }
            else if (race == RacialType.Halforc)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Halforc_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Halforc_Male);
            }
            else if (race == RacialType.Human)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Human_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Cyn_Human_Male);
            }
            else
            {
                SendMessageToPC(pc, $"Invalid Race {race}. SetEyesCyan.");
                throw new ArgumentException($"Name:{GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change SetEyesCyan. Invalid race {race}.");
            }

            return eyeColor;
        }

        public static Effect SetEyesOrange(this uint pc)
        {
            Effect eyeColor;
            GenderType gender = GetGender(pc);
            RacialType race = GetRacialType(pc);

            if (race == RacialType.Dwarf)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Dwarf_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Dwarf_Male);
            }
            else if (race == RacialType.Elf)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Elf_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Elf_Male);
            }
            else if (race == RacialType.Gnome)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Gnome_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Gnome_Male);
            }
            else if (race == RacialType.Halfelf)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Troglodyte) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Troglodyte);
            }
            else if (race == RacialType.Halfling)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Halfling_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Halfling_Male);
            }
            else if (race == RacialType.Halforc)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Halforc_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Halforc_Male);
            }
            else if (race == RacialType.Human)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Human_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Org_Human_Male);
            }
            else
            {
                SendMessageToPC(pc, $"Invalid Race {race}. SetEyesOrange.");
                throw new ArgumentException($"Name:{GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change SetEyesOrange. Invalid race {race}.");
            }

            return eyeColor;
        }

        public static Effect SetEyesPurple(this uint pc)
        {
            Effect eyeColor;
            GenderType gender = GetGender(pc);
            RacialType race = GetRacialType(pc);

            if (race == RacialType.Dwarf)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Dwarf_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Dwarf_Male);
            }
            else if (race == RacialType.Elf)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Elf_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Elf_Male);
            }
            else if (race == RacialType.Gnome)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Gnome_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Gnome_Male);
            }
            else if (race == RacialType.Halfelf)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Troglodyte) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Troglodyte);
            }
            else if (race == RacialType.Halfling)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Halfling_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Halfling_Male);
            }
            else if (race == RacialType.Halforc)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Halforc_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Halforc_Male);
            }
            else if (race == RacialType.Human)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Human_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Pur_Human_Male);
            }
            else
            {
                SendMessageToPC(pc, $"Invalid Race {race}. SetEyesPurple.");
                throw new ArgumentException($"Name:{GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change SetEyesPurple. Invalid race {race}.");
            }

            return eyeColor;
        }

        public static Effect SetEyesWhite(this uint pc)
        {
            Effect eyeColor;
            GenderType gender = GetGender(pc);
            RacialType race = GetRacialType(pc);

            if (race == RacialType.Dwarf)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Dwarf_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Dwarf_Male);
            }
            else if (race == RacialType.Elf)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Elf_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Elf_Male);
            }
            else if (race == RacialType.Gnome)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Gnome_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Gnome_Male);
            }
            else if (race == RacialType.Halfelf)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Troglodyte) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Troglodyte);
            }
            else if (race == RacialType.Halfling)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Halfling_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Halfling_Male);
            }
            else if (race == RacialType.Halforc)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Halforc_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Halforc_Male);
            }
            else if (race == RacialType.Human)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Human_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Wht_Human_Male);
            }
            else
            {
                SendMessageToPC(pc, $"Invalid Race {race}. SetEyesWhite.");
                throw new ArgumentException($"Name:{GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change SetEyesWhite. Invalid race {race}.");
            }

            return eyeColor;
        }

        public static Effect SetEyesYellow(this uint pc)
        {
            Effect eyeColor;
            GenderType gender = GetGender(pc);
            RacialType race = GetRacialType(pc);

            if (race == RacialType.Dwarf)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Dwarf_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Dwarf_Male);
            }
            else if (race == RacialType.Elf)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Elf_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Elf_Male);
            }
            else if (race == RacialType.Gnome)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Gnome_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Gnome_Male);
            }
            else if (race == RacialType.Halfelf)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Troglodyte) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Troglodyte);
            }
            else if (race == RacialType.Halfling)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Halfling_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Halfling_Male);
            }
            else if (race == RacialType.Halforc)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Halforc_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Halforc_Male);
            }
            else if (race == RacialType.Human)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Human_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Yel_Human_Male);
            }
            else
            {
                SendMessageToPC(pc, $"Invalid Race {race}. SetEyesYellow.");
                throw new ArgumentException($"Name:{GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change SetEyesYellow. Invalid race {race}.");
            }

            return eyeColor;
        }

        public static Effect SetEyesGreen(this uint pc)
        {
            Effect eyeColor;
            GenderType gender = GetGender(pc);
            RacialType race = GetRacialType(pc);

            if (race == RacialType.Dwarf)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Dwarf_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Dwarf_Male);
            }
            else if (race == RacialType.Elf)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Elf_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Elf_Male);
            }
            else if (race == RacialType.Gnome)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Gnome_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Gnome_Male);
            }
            else if (race == RacialType.Halfelf)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Troglodyte) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Troglodyte);
            }
            else if (race == RacialType.Halfling)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Halfling_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Halfling_Male);
            }
            else if (race == RacialType.Halforc)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Halforc_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Halforc_Male);
            }
            else if (race == RacialType.Human)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Human_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Green_Human_Male);
            }
            else
            {
                SendMessageToPC(pc, $"Invalid Race {race}. SetEyesGreen.");
                throw new ArgumentException($"Name:{GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change SetEyesGreen. Invalid race {race}.");
            }

            return eyeColor;
        }

        public static Effect SetEyesRed(this uint pc)
        {
            Effect eyeColor;
            GenderType gender = GetGender(pc);
            RacialType race = GetRacialType(pc);

            if (race == RacialType.Dwarf)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Red_Flame_Dwarf_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Red_Flame_Dwarf_Male);
            }
            else if (race == RacialType.Elf)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Red_Flame_Elf_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Red_Flame_Elf_Male);
            }
            else if (race == RacialType.Gnome)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Red_Flame_Gnome_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Red_Flame_Gnome_Male);
            }
            else if (race == RacialType.Halfelf)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Red_Flame_Troglodyte) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Red_Flame_Troglodyte);
            }
            else if (race == RacialType.Halfling)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Red_Flame_Halfling_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Red_Flame_Halfling_Male);
            }
            else if (race == RacialType.Halforc)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Red_Flame_Halforc_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Red_Flame_Halforc_Male);
            }
            else if (race == RacialType.Human)
            {
                eyeColor = gender == GenderType.Female ? EffectVisualEffect(VisualEffectType.Vfx_Eyes_Red_Flame_Human_Female) : EffectVisualEffect(VisualEffectType.Vfx_Eyes_Red_Flame_Human_Male);
            }
            else
            {
                SendMessageToPC(pc, $"Invalid Race {race}. SetEyesRed.");
                throw new ArgumentException($"Name:{GetName(pc)} | BIC:{Player.GetBicFileName(pc)} failed to change SetEyesRed. Invalid race {race}.");
            }

            return eyeColor;
        }
    }
}