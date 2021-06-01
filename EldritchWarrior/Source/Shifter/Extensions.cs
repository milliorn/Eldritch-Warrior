using System;

using NWN.Framework.Lite.Enum;
using static NWN.Framework.Lite.NWScript;
using ItemProperty = NWN.Framework.Lite.ItemProperty;
using Effect = NWN.Framework.Lite.Effect;
using NWN.Framework.Lite.Bioware;

namespace EldritchWarrior.Source.Shifter
{
    public static class Extensions
    {
        /*
        What this script changes:
        
        - Melee Weapon properties now carry over to the unarmed forms' claws and bite attacks
        .
        1) Now, items with an AC bonus (or penalty) carry over to the shifted form as
        the correct type. This means if you wear an amulet of natural armor +4, and a
        cloak of protection +5, and you shift to a form that gets all item properties
        carried over, you will have the +4 natural armor bonus from the ammy, as well as
        the +5 deflection bonus from the cloak. No longer will the highest one override
        all the other AC bonuses even if they are a different type.
        
        2) Other "stackable" item properties, like ability bonuses, skill bonuses and
        saving throw bonuses, now correctly add up in shifted form. This means if you
        have a ring that gives +2 strength, and a ring with +3 strength, and you shift
        into a drow warrior, you get +5 strength in shifted form, where you used to get
        only +3. (the highest)

        This file contains the code that handles stacking item properties for the improved
        Shifter and Druid wildshape scripts.
        */
        //***************** GENERAL OPTIONS *********************

        // Set this to true to allow differing types of AC bonuses on items to stack.
        // (ie armor, deflection, natural) Warning: This can give shifters who multiclass
        // with monk a godly AC depending on your module.
        // With false, AC will transfer as it did with the default Bioware shifter script.
        public static readonly bool GW_ALLOW_AC_STACKING = false;//true;false

        //***************** FOR SHIFTER SHAPES ******************

        // Set this to true to merge properties of boots/rings/ammy/cloak/bracers regardless
        // of what polymorph.2da indicates.
        // false uses the polymorph.2da to decide whether to copy
        public static readonly bool GW_ALWAYS_COPY_ITEM_PROPS = true;//false;

        // Set this to true to merge armor/helmet properties regardless of what polymorph.2da indicates.
        // false uses the polymorph.2da to decide whether to copy
        public static readonly bool GW_ALWAYS_COPY_ARMOR_PROPS = true;

        // - Set this to 1 to copy over weapon properties to claw/bite attacks.
        // - Set this to 2 to copy over glove properties to claw/bite attacks.
        // - Set this to 3 to copy over from either weapon or gloves depending on whether a
        //   weapon was worn at the time of shifting.
        // - Set this to any other value ( eg 0 ) to not copy over anything to claw/bite attacks.
        public static readonly int GW_COPY_WEAPON_PROPS_TO_UNARMED = 3;


        //***************** FOR DRUID SHAPES ********************
        // These options do nothing if you have not imported the improved Druid wild-
        // and elemental shape scripts

        // Set this to true to merge properties of boots/rings/ammy/cloak/bracers regardless
        // of what polymorph.2da indicates.
        // false uses the polymorph.2da to decide whether to copy
        public static readonly bool WS_ALWAYS_COPY_ITEM_PROPS = true;//false;

        // Set this to true to merge armor/helmet properties regardless of what polymorph.2da indicates.
        // false uses the polymorph.2da to decide whether to copy
        public static readonly bool WS_ALWAYS_COPY_ARMOR_PROPS = true;

        // - Set this to 1 to copy over weapon properties to claw/bite attacks.
        // - Set this to 2 to copy over glove properties to claw/bite attacks.
        // - Set this to 3 to copy over from either weapon or gloves depending on whether a
        //   weapon was worn at the time of shifting.
        // - Set this to any other value ( eg 0 ) to not copy over anything to claw/bite attacks.
        public static readonly int WS_COPY_WEAPON_PROPS_TO_UNARMED = 3;

        ///<summary>
        // Copies oldWeapon's Properties to newWeapon, but only properties that do not stack
        // with properties of the same type. If oldWeapon is a weapon, then weapon must be true.
        ///</summary>
        public static void WildshapeCopyNonStackProperties(uint oldWeapon, uint newWeapon, bool weapon = false)
        {
            if (GetIsObjectValid(oldWeapon) && GetIsObjectValid(newWeapon))
            {
                ItemProperty ip = GetFirstItemProperty(oldWeapon);
                // Loop through all the item properties.
                while (GetIsItemPropertyValid(ip))
                {
                    if (weapon)
                    {
                        // If a weapon, then we must make sure not to transfer between ranged and non-ranged weapons!
                        if (GetWeaponRanged(oldWeapon) == GetWeaponRanged(newWeapon))
                        {
                            AddItemProperty(DurationType.Instant, ip, newWeapon);
                        }
                    }
                    else
                    {
                        // If not a stacking property, copy over the property and don't copy on hit cast spell property unless the target is a claw/bite.
                        if (!GetIsStackingProperty(ip) && Convert.ToBoolean(GetIsCreatureWeapon(newWeapon)) || GetItemPropertyType(ip) != ItemPropertyType.OnHitCastSpell)
                        {
                            AddItemProperty(DurationType.Permanent, ip, newWeapon);
                        }
                    }
                    ip = GetNextItemProperty(oldWeapon);
                }
            }
        }

        // Returns true if ip is an item property that will stack with other properties of the same type: Ability, AC, Saves, Skills.
        public static bool GetIsStackingProperty(ItemProperty ip) => GetItemPropertyType(ip) == ItemPropertyType.AbilityBonus || (GW_ALLOW_AC_STACKING && (GetItemPropertyType(ip) == ItemPropertyType.ACBonus)) ||
                    GetItemPropertyType(ip) == ItemPropertyType.DecreasedAbilityScore || (GW_ALLOW_AC_STACKING && (GetItemPropertyType(ip) == ItemPropertyType.DecreasedAC)) ||
                    GetItemPropertyType(ip) == ItemPropertyType.SavingThrowBonus ||
                    GetItemPropertyType(ip) == ItemPropertyType.SavingThrowBonusSpecific ||
                    GetItemPropertyType(ip) == ItemPropertyType.DecreasedSavingThrows ||
                    GetItemPropertyType(ip) == ItemPropertyType.DecreasedSavingThrows ||
                    GetItemPropertyType(ip) == ItemPropertyType.SkillBonus ||
                    GetItemPropertyType(ip) == ItemPropertyType.DecreasedSkillModifier ||
                    GetItemPropertyType(ip) == ItemPropertyType.Regeneration ||
                    GetItemPropertyType(ip) == ItemPropertyType.ImmunityDamageType ||
                    GetItemPropertyType(ip) == ItemPropertyType.DamageVulnerability;

        // Returns the AC bonus type of item: AC_*_BONUS
        public static ItemPropertyArmorClassModifierType GetItemACType(uint item)
        {
            if (GetBaseItemType(item) == BaseItemType.Armor || GetBaseItemType(item) == BaseItemType.Bracer)
            {
                return ItemPropertyArmorClassModifierType.Armor;
            }
            else if (GetBaseItemType(item) == BaseItemType.Belt || GetBaseItemType(item) == BaseItemType.Cloak || GetBaseItemType(item) == BaseItemType.Gloves ||
                    GetBaseItemType(item) == BaseItemType.Helmet || GetBaseItemType(item) == BaseItemType.Ring || GetBaseItemType(item) == BaseItemType.Torch)
            {
                return ItemPropertyArmorClassModifierType.Deflection;
            }
            else if (GetBaseItemType(item) == BaseItemType.Boots)
            {
                return ItemPropertyArmorClassModifierType.Dodge;
            }
            else if (GetBaseItemType(item) == BaseItemType.Amulet)
            {
                return ItemPropertyArmorClassModifierType.Natural;
            }
            else if (GetBaseItemType(item) == BaseItemType.LargeShield || GetBaseItemType(item) == BaseItemType.SmallShield || GetBaseItemType(item) == BaseItemType.TowerShield)
            {
                return ItemPropertyArmorClassModifierType.Shield;
            }
            else
            {
                return ItemPropertyArmorClassModifierType.Deflection;
            }
        }

        // Looks for Stackable Properties on item, and sets local variables to count the total bonus.
        // Also links any found AC bonuses/penalties to poly.
        public static Effect ExamineStackableProperties(uint pc, Effect poly, uint item)
        {
            // If not valid, don't do any unnecessary work.
            if (!GetIsObjectValid(item)) return poly;

            ItemProperty ip = GetFirstItemProperty(item);
            while (GetIsItemPropertyValid(ip))
            {
                if (GetIsStackingProperty(ip))
                {
                    int subType = GetItemPropertySubType(ip);
                    // This contains whether a bonus is str, dex,
                    // concentration skill, universal saving throws, etc.

                    // In the case of AC modifiers, add it directly to the Polymorphing effect.
                    // For the other cases, set local variables on the player to
                    // make a sum of all the bonuses/penalties. We use local
                    // variables here because there are no arrays in NWScript, and
                    // declaring a variable for every skill, ability type and saving
                    // throw type in here is a little overboard.
                    if (GetItemPropertyType(ip) == ItemPropertyType.AbilityBonus)
                    {
                        SetLocalInt(pc, "ws_ability_" + IntToString(subType), GetLocalInt(pc, "ws_ability_" + IntToString(subType)) + GetItemPropertyCostTableValue(ip));
                    }
                    else if (GetItemPropertyType(ip) == ItemPropertyType.ACBonus)
                    {
                        poly = EffectLinkEffects(EffectACIncrease(GetItemPropertyCostTableValue(ip), GetItemACType(item), ACType.VsDamageTypeAll), poly);
                    }
                    else if (GetItemPropertyType(ip) == ItemPropertyType.DecreasedAbilityScore)
                    {
                        SetLocalInt(pc, "ws_ability_" + IntToString(subType), GetLocalInt(pc, "ws_ability_" + IntToString(subType)) - GetItemPropertyCostTableValue(ip));
                    }
                    else if (GetItemPropertyType(ip) == ItemPropertyType.DecreasedAbilityScore)
                    {
                        int value = GetItemPropertyCostTableValue(ip);
                        ItemPropertyArmorClassModifierType modifyType = ItemPropertyArmorClassModifierType.Dodge;
                        ACType damageType = ACType.VsDamageTypeAll;
                        Effect childEffect = EffectACDecrease(value, modifyType, damageType);
                        poly = EffectLinkEffects(childEffect, poly);
                    }
                    else if (GetItemPropertyType(ip) == ItemPropertyType.SkillBonus)
                    {
                        SetLocalInt(pc, "ws_skill_" + IntToString(subType), GetLocalInt(pc, "ws_skill_" + IntToString(subType)) + GetItemPropertyCostTableValue(ip));
                    }
                    else if (GetItemPropertyType(ip) == ItemPropertyType.DecreasedSkillModifier)
                    {
                        SetLocalInt(pc, "ws_skill_" + IntToString(subType), GetLocalInt(pc, "ws_skill_" + IntToString(subType)) - GetItemPropertyCostTableValue(ip));
                    }
                    else if (GetItemPropertyType(ip) == ItemPropertyType.SavingThrowBonus)
                    {
                        SetLocalInt(pc, "ws_save_elem_" + IntToString(subType), GetLocalInt(pc, "ws_save_elem_" + IntToString(subType)) + GetItemPropertyCostTableValue(ip));
                    }
                    else if (GetItemPropertyType(ip) == ItemPropertyType.SavingThrowBonusSpecific)
                    {
                        SetLocalInt(pc, "ws_save_spec_" + IntToString(subType), GetLocalInt(pc, "ws_save_spec_" + IntToString(subType)) + GetItemPropertyCostTableValue(ip));
                    }
                    else if (GetItemPropertyType(ip) == ItemPropertyType.DecreasedSavingThrows)
                    {
                        SetLocalInt(pc, "ws_save_elem_" + IntToString(subType), GetLocalInt(pc, "ws_save_elem_" + IntToString(subType)) - GetItemPropertyCostTableValue(ip));
                    }
                    else if (GetItemPropertyType(ip) == ItemPropertyType.DecreasedSavingThrowsSpecific)
                    {
                        SetLocalInt(pc, "ws_save_spec_" + IntToString(subType), GetLocalInt(pc, "ws_save_spec_" + IntToString(subType)) - GetItemPropertyCostTableValue(ip));
                    }
                    else if (GetItemPropertyType(ip) == ItemPropertyType.Regeneration)
                    {
                        SetLocalInt(pc, "ws_regen", GetLocalInt(OBJECT_SELF, "ws_regen") + GetItemPropertyCostTableValue(ip));
                    }
                    else if (GetItemPropertyType(ip) == ItemPropertyType.ImmunityDamageType)
                    {
                        SetLocalInt(pc, "ws_dam_immun_" + IntToString(subType), GetLocalInt(pc, "ws_dam_immun_" + IntToString(subType)) + ConvertNumToImmunePercentage(GetItemPropertyCostTableValue(ip)));
                    }
                    else if (GetItemPropertyType(ip) == ItemPropertyType.DamageVulnerability)
                    {
                        SetLocalInt(pc, "ws_dam_immun_" + IntToString(subType), GetLocalInt(pc, "ws_dam_immun_" + IntToString(subType)) - ConvertNumToImmunePercentage(GetItemPropertyCostTableValue(ip)));
                    }
                }
                ip = GetNextItemProperty(item);
            }
            return poly;
        }

        // if isItem is true, Adds all the stackable properties on all the objects given to poly.
        // if isItem is false, Adds only the stackable properties on armor and helmet to poly.
        public static Effect AddStackablePropertiesToPoly(uint pc, Effect poly, bool isWeapon, bool isItem, bool isArmor,
        uint oldArmor, uint oldRing1, uint oldRing2, uint oldAmulet, uint cloakOld, uint oldBracer, uint oldBoots, uint oldBelt, uint oldHelmet, uint oShield, uint oWeapon, uint oHideOld)
        {
            // Armor properties get carried over
            if (isArmor)
            {
                poly = ExamineStackableProperties(pc, poly, oldArmor);
                poly = ExamineStackableProperties(pc, poly, oldHelmet);
                poly = ExamineStackableProperties(pc, poly, oShield);
                poly = ExamineStackableProperties(pc, poly, oHideOld);
            }
            // Item properties get carried over
            if (isItem)
            {
                poly = ExamineStackableProperties(pc, poly, oldRing1);
                poly = ExamineStackableProperties(pc, poly, oldRing2);
                poly = ExamineStackableProperties(pc, poly, oldAmulet);
                poly = ExamineStackableProperties(pc, poly, cloakOld);
                poly = ExamineStackableProperties(pc, poly, oldBoots);
                poly = ExamineStackableProperties(pc, poly, oldBelt);
                poly = ExamineStackableProperties(pc, poly, oldBracer);
            }

            // AC bonuses are attached to poly inside ExamineStackableProperties
            int i; // This will loop over all the different ability subtypes (eg str, dex, con, etc)
            int j; // This will contain the sum of the stackable bonus type we're looking at
            for (i = 0; i <= 5; i++) // **** Handle Ability Bonuses ****
            {
                j = GetLocalInt(pc, "ws_ability_" + IntToString(i));
                // Add the sum of this ability bonus to the polymorph effect.
                if (j > 0)
                    poly = EffectLinkEffects(EffectAbilityIncrease((AbilityType)i, j), poly);
                else if (j < 0)
                    poly = EffectLinkEffects(EffectAbilityDecrease((AbilityType)i, -j), poly);
                DeleteLocalInt(pc, "ws_ability_" + IntToString(i));
            }
            // **** Handle Skill Bonuses ****
            for (i = 0; i <= 26; i++)
            {
                j = GetLocalInt(pc, "ws_skill_" + IntToString(i));
                // Add the sum of this skill bonus to the polymorph effect.
                if (j > 0)
                    poly = EffectLinkEffects(EffectSkillIncrease((SkillType)i, j), poly);
                else if (j < 0)
                    poly = EffectLinkEffects(EffectSkillDecrease(i, -j), poly);
                DeleteLocalInt(pc, "ws_skill_" + IntToString(i));
            }
            // **** Handle Saving Throw vs element Bonuses ****
            for (i = 0; i <= 21; i++)
            {
                j = GetLocalInt(pc, "ws_save_elem_" + IntToString(i));
                // Add the sum of this saving throw bonus to the polymorph effect.
                if (j > 0)
                    poly = EffectLinkEffects(EffectSavingThrowIncrease((int)SavingThrowType.All, j, (SavingThrowType)i), poly);
                else if (j < 0)
                    poly = EffectLinkEffects(EffectSavingThrowDecrease((int)SavingThrowType.All, -j, (SavingThrowType)i), poly);
                DeleteLocalInt(pc, "ws_save_elem_" + IntToString(i));
            }
            // **** Handle Saving Throw specific Bonuses ****
            for (i = 0; i <= 3; i++)
            {
                j = GetLocalInt(pc, "ws_save_spec_" + IntToString(i));
                // Add the sum of this saving throw bonus to the polymorph effect.
                if (j > 0)
                    poly = EffectLinkEffects(EffectSavingThrowIncrease(i, j, SavingThrowType.All), poly);
                else if (j < 0)
                    poly = EffectLinkEffects(EffectSavingThrowDecrease(i, -j, SavingThrowType.All), poly);
                DeleteLocalInt(pc, "ws_save_spec_" + IntToString(i));
            }
            j = GetLocalInt(pc, "ws_regen");
            if (j > 0)
            {
                poly = EffectLinkEffects(EffectRegenerate(j, 6.0f), poly);
                DeleteLocalInt(pc, "ws_regen");
            }
            // **** Handle Damage Immunity and Vulnerability ****
            for (i = 0; i <= 13; i++)
            {
                j = GetLocalInt(pc, "ws_dam_immun_" + IntToString(i));
                // Add the sum of this Damage Immunity/Vulnerability to the polymorph effect.
                if (j > 0)
                    poly = EffectLinkEffects(EffectDamageImmunityIncrease(ConvertNumToDamTypeConstant(i), j), poly);
                else if (j < 0)
                    poly = EffectLinkEffects(EffectDamageImmunityDecrease((int)ConvertNumToDamTypeConstant(i), -j), poly);
                DeleteLocalInt(pc, "ws_dam_immun_" + IntToString(i));
            }

            return poly;
        }

        // Returns the spell that applied a Polymorph Effect currently on the player.
        // -1 if it was no spell, -2 if no polymorph effect found.
        public static int ScanForPolymorphEffect(this uint pc)
        {
            Effect eEffect = GetFirstEffect(pc);
            while (GetIsEffectValid(eEffect))
            {
                if (GetEffectType(eEffect) == EffectScriptType.Polymorph)
                {
                    return GetEffectSpellId(eEffect);
                }
                eEffect = GetNextEffect(pc);
            }
            return -2;
        }

        // Converts a number from iprp_damagetype.2da to the corresponding
        // DAMAGE_TYPE_* constants.
        public static DamageType ConvertNumToDamTypeConstant(int itemDamType)
        {
            switch (itemDamType)
            {
                case 0: return DamageType.Bludgeoning;
                case 1: return DamageType.Piercing;
                case 2: return DamageType.Slashing;
                case 5: return DamageType.Magical;
                case 6: return DamageType.Acid;
                case 7: return DamageType.Cold;
                case 8: return DamageType.Divine;
                case 9: return DamageType.Electrical;
                case 10: return DamageType.Fire;
                case 11: return DamageType.Negative;
                case 12: return DamageType.Positive;
                case 13: return DamageType.Sonic;
                default: return DamageType.Positive;
            }
        }

        // Converts a number from iprp_immuncost.2da to the corresponding percentage of immunity
        public static int ConvertNumToImmunePercentage(int immuneCost)
        {
            switch (immuneCost)
            {
                case 1: return 5;
                case 2: return 10;
                case 3: return 25;
                case 4: return 50;
                case 5: return 75;
                case 6: return 90;
                case 7: return 100;
                default: return 0;
            }
        }

        public static void WildshapeCopyWeaponProperties(uint pc, uint oldWeapon, uint newWeapon)
        {
            if (GetIsObjectValid(oldWeapon) && GetIsObjectValid(newWeapon))
            {
                ItemProperty ip = GetFirstItemProperty(oldWeapon);

                if (!GetWeaponRanged(oldWeapon) && !GetWeaponRanged(newWeapon))
                {
                    while (GetIsItemPropertyValid(ip))
                    {
                        AddItemProperty(DurationType.Permanent, ip, newWeapon);
                        ip = GetNextItemProperty(oldWeapon);
                    }
                }

                else if (GetWeaponRanged(oldWeapon) && GetWeaponRanged(newWeapon))
                {
                    bool unlimitedAmmoFound = false;
                    ItemProperty ipNew;
                    int oldMightyValue = 0;
                    uint ammo;

                    while (GetIsItemPropertyValid(ip))
                    {
                        if (GetItemPropertyType(ip) == ItemPropertyType.UnlimitedAmmunition)
                        {
                            // For some reason, when removing/replacing an unlimited
                            // ammo property, the corresponding missile type will get
                            // dropped in the player's inventory, so we have to remove
                            // that missile again to prevent abuse.
                            unlimitedAmmoFound = true;
                            ammo = GetItemInSlot(InventorySlotType.Arrows, pc);

                            if (!GetIsObjectValid(ammo))
                                ammo = GetItemInSlot(InventorySlotType.Bolts, pc);
                            if (!GetIsObjectValid(ammo))
                                ammo = GetItemInSlot(InventorySlotType.Bullets, pc);

                            BiowareXP2.IPRemoveMatchingItemProperties(newWeapon, ItemPropertyType.UnlimitedAmmunition, DurationType.Permanent, -1);
                            AddItemProperty(DurationType.Permanent, ip, newWeapon);
                            DestroyObject(ammo);
                        }
                        else if (GetItemPropertyType(ip) == ItemPropertyType.Mighty)
                        {
                            ipNew = GetFirstItemProperty(newWeapon);
                            // Find the mighty value of the Polymorph's weapon
                            while (GetIsItemPropertyValid(ipNew))
                            {
                                if (GetItemPropertyType(ipNew) == ItemPropertyType.Mighty)
                                {
                                    oldMightyValue = GetItemPropertyCostTableValue(ipNew);
                                    break;
                                }
                                ipNew = GetNextItemProperty(newWeapon);
                            }
                            // If new mighty value bigger, remove old one and add new one.
                            if (GetItemPropertyCostTableValue(ip) > oldMightyValue)
                            {
                                RemoveItemProperty(newWeapon, ipNew);
                                AddItemProperty(DurationType.Permanent, ip, newWeapon);
                            }
                        }
                        else
                        {
                            AddItemProperty(DurationType.Permanent, ip, newWeapon);
                        }
                        ip = GetNextItemProperty(oldWeapon);
                    }
                    // Add basic unlimited ammo if necessary
                    if (unlimitedAmmoFound == false && !GetItemHasItemProperty(newWeapon, ItemPropertyType.UnlimitedAmmunition))
                        AddItemProperty(DurationType.Permanent, ItemPropertyUnlimitedAmmo(ItemPropertyUnlimitedType.Basic), newWeapon);
                }
            }
            else if (GetWeaponRanged(newWeapon))
            {
                // Add basic unlimited ammo if necessary
                if (!GetItemHasItemProperty(newWeapon, ItemPropertyType.UnlimitedAmmunition))
                    AddItemProperty(DurationType.Permanent, ItemPropertyUnlimitedAmmo(ItemPropertyUnlimitedType.Basic), newWeapon);
            }
        }

        // Returns true if item is a creature claw or bite.
        public static bool GetIsCreatureWeapon(uint item) =>
            GetBaseItemType(item) == BaseItemType.CreatureBludgeWeapon ||
            GetBaseItemType(item) == BaseItemType.CreaturePierceWeapon ||
            GetBaseItemType(item) == BaseItemType.CreatureSlashPierceWeapon ||
            GetBaseItemType(item) == BaseItemType.CreatureSlashWeapon;

        public static void ReFireSpell(this uint pc, int spell)
        {
            // This is necessary so the spell can be re-fired on the player.
            // Otherwise, if the shifter was in combat, it would wait in the action
            // queue until the player stopped fighting. When in combat, it will make
            // the shifter start attacking again.
            uint attackee = GetAttackTarget(pc);

            // We clear all actions if the player is not resting.
            if (!GetIsResting(pc))
            {
                AssignCommand(pc, () => ClearAllActions());
            }

            // Re-fire the spell on the shifter.
            SetLocalInt(pc, "GW_ServerSave", 1);

            //ActionCastSpellAtObject(spell, pc, METAMAGIC_ANY, TRUE, 0, PROJECTILE_PATH_TYPE_DEFAULT, TRUE);
            AssignCommand(pc, () => ActionCastSpellAtObject((SpellType)spell, pc, MetaMagicType.Any, true, 0, ProjectilePathType.Default, true));

            // Start attacking our target again if we had one.
            if (GetIsObjectValid(attackee))
            {
                // If we do not delaycommand here again, the stackable properties won't be re-applied.
                AssignCommand(pc, () => DelayCommand(0.0f, () => ActionAttack(attackee)));
            }
        }

        // Returns TRUE if the shifter's current weapon should be merged onto his
        // newly equipped melee weapon
        public static bool ShifterMergeWeapon(int polymorphConstant) => StringToInt(Get2DAString("polymorph", "MergeW", polymorphConstant)) == 1;

        // Returns TRUE if the shifter's current armor should be merged onto his
        // creature hide after shifting.
        public static bool ShifterMergeArmor(int polymorphConstant) => StringToInt(Get2DAString("polymorph", "MergeA", polymorphConstant)) == 1;

        // Returns TRUE if the shifter's current items (gloves, belt, etc) should
        // be merged onto his creature hide after shifting.
        public static bool ShifterMergeItems(int polymorphConstant) => StringToInt(Get2DAString("polymorph", "MergeI", polymorphConstant)) == 1;

        // Introduces an artificial limit on the special abilities of the Greater
        // Wildshape forms, in order to work around the engine limitation
        // of being able to cast any assigned spell an unlimited number of times
        //------------------------------------------------------------------------------
        public static void ShifterSetGWildshapeSpellLimits(int spellId)
        {
            string id;
            int levelByClass = GetLevelByClass(ClassType.Shifter);
            switch (spellId)
            {
                case 673:       // Drider Shape
                    id = "688"; // SpellIndex of Drider Darkness Ability
                    SetLocalInt(OBJECT_SELF, "X2_GWILDSHP_LIMIT_" + id, 1 + levelByClass / 10);
                    break;

                case 670:     // Basilisk Shape
                    id = "687"; // SpellIndex of Petrification Gaze Ability
                    SetLocalInt(OBJECT_SELF, "X2_GWILDSHP_LIMIT_" + id, 1 + levelByClass / 5);
                    break;

                case 679:      // Medusa Shape
                    id = "687"; // SpellIndex of Petrification Gaze Ability
                    SetLocalInt(OBJECT_SELF, "X2_GWILDSHP_LIMIT_" + id, 1 + levelByClass / 5);
                    break;

                case 682:      // Drow shape
                    id = "688"; // Darkness Ability
                    SetLocalInt(OBJECT_SELF, "X2_GWILDSHP_LIMIT_" + id, 1 + levelByClass / 10);
                    break;

                case 691:      // Mindflayer shape
                    id = "693"; // SpellIndex Mind Blast Ability
                    SetLocalInt(OBJECT_SELF, "X2_GWILDSHP_LIMIT_" + id, 1 + levelByClass / 3);
                    break;

                case 705:       // Vampire Domination Gaze
                    id = "800";
                    SetLocalInt(OBJECT_SELF, "X2_GWILDSHP_LIMIT_" + id, 1 + levelByClass / 5);
                    break;

            }
        }
    }
}