using System;

using NWN.Framework.Lite.Enum;
using static NWN.Framework.Lite.NWScript;
using ItemProperty = NWN.Framework.Lite.ItemProperty;
using Effect = NWN.Framework.Lite.Effect;
using NWN.Framework.Lite.Bioware;

namespace EldritchWarrior.Source.Shifter
{
    public class Extensions
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
        void WildshapeCopyNonStackProperties(uint oldWeapon, uint newWeapon, bool weapon = false)
        {
            if (GetIsObjectValid(oldWeapon) && GetIsObjectValid(newWeapon))
            {
                ItemProperty ip = GetFirstItemProperty(oldWeapon);
                while (GetIsItemPropertyValid(ip)) // Loop through all the item properties.
                {
                    if (weapon) // If a weapon, then we must make sure not to transfer between ranged and non-ranged weapons!
                    {
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
                    ip = GetNextItemProperty(oldWeapon); // Get next property
                }
            }
        }

        // Returns true if ip is an item property that will stack with other properties
        // of the same type: Ability, AC, Saves, Skills.
        bool GetIsStackingProperty(ItemProperty ip) => GetItemPropertyType(ip) == ItemPropertyType.AbilityBonus || (GW_ALLOW_AC_STACKING && (GetItemPropertyType(ip) == ItemPropertyType.ACBonus)) ||
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
        ItemPropertyArmorClassModifierType GetItemACType(uint item)
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
        Effect ExamineStackableProperties(uint pc, Effect poly, uint item)
        {
            if (!GetIsObjectValid(item)) return poly; // If not valid, don't do any unnecessary work.

            ItemProperty ip = GetFirstItemProperty(item);
            while (GetIsItemPropertyValid(ip)) // Loop through all the item properties
            {
                if (GetIsStackingProperty(ip)) // See if it's a stacking property
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
                    if (GetItemPropertyType(ip) == ItemPropertyType.AbilityBonus) // Which type of property is it?
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
        Effect AddStackablePropertiesToPoly(uint pc, Effect poly, bool isWeapon, bool isItem, bool isArmor,
        uint oldArmor, uint oldRing1, uint oldRing2, uint oldAmulet, uint cloakOld, uint oldBracer, uint oldBoots, uint oldBelt, uint oldHelmet, uint oShield, uint oWeapon, uint oHideOld)
        {
            if (isArmor) // Armor properties get carried over
            {
                poly = ExamineStackableProperties(pc, poly, oldArmor);
                poly = ExamineStackableProperties(pc, poly, oldHelmet);
                poly = ExamineStackableProperties(pc, poly, oShield);
                poly = ExamineStackableProperties(pc, poly, oHideOld);
            }
            if (isItem) // Item properties get carried over
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
                if (j > 0) // Sum was Positive
                    poly = EffectLinkEffects(EffectAbilityIncrease((AbilityType)i, j), poly);
                else if (j < 0) // Sum was Negative
                    poly = EffectLinkEffects(EffectAbilityDecrease((AbilityType)i, -j), poly);
                DeleteLocalInt(pc, "ws_ability_" + IntToString(i));
            }
            for (i = 0; i <= 26; i++) // **** Handle Skill Bonuses ****
            {
                j = GetLocalInt(pc, "ws_skill_" + IntToString(i));
                // Add the sum of this skill bonus to the polymorph effect.
                if (j > 0) // Sum was Positive
                    poly = EffectLinkEffects(EffectSkillIncrease((SkillType)i, j), poly);
                else if (j < 0) // Sum was Negative
                    poly = EffectLinkEffects(EffectSkillDecrease(i, -j), poly);
                DeleteLocalInt(pc, "ws_skill_" + IntToString(i));
            }
            for (i = 0; i <= 21; i++) // **** Handle Saving Throw vs element Bonuses ****
            {
                j = GetLocalInt(pc, "ws_save_elem_" + IntToString(i));
                // Add the sum of this saving throw bonus to the polymorph effect.
                if (j > 0) // Sum was Positive
                    poly = EffectLinkEffects(EffectSavingThrowIncrease((int)SavingThrowType.All, j, (SavingThrowType)i), poly);
                else if (j < 0) // Sum was Negative
                    poly = EffectLinkEffects(EffectSavingThrowDecrease((int)SavingThrowType.All, -j, (SavingThrowType)i), poly);
                DeleteLocalInt(pc, "ws_save_elem_" + IntToString(i));
            }
            for (i = 0; i <= 3; i++) // **** Handle Saving Throw specific Bonuses ****
            {
                j = GetLocalInt(pc, "ws_save_spec_" + IntToString(i));
                // Add the sum of this saving throw bonus to the polymorph effect.
                if (j > 0) // Sum was Positive
                    poly = EffectLinkEffects(EffectSavingThrowIncrease(i, j, SavingThrowType.All), poly);
                else if (j < 0) // Sum was Negative
                    poly = EffectLinkEffects(EffectSavingThrowDecrease(i, -j, SavingThrowType.All), poly);
                DeleteLocalInt(pc, "ws_save_spec_" + IntToString(i));
            }
            j = GetLocalInt(pc, "ws_regen");
            if (j > 0)
            {
                poly = EffectLinkEffects(EffectRegenerate(j, 6.0f), poly);
                DeleteLocalInt(pc, "ws_regen");
            }
            for (i = 0; i <= 13; i++) // **** Handle Damage Immunity and Vulnerability ****
            {
                j = GetLocalInt(pc, "ws_dam_immun_" + IntToString(i));
                // Add the sum of this Damage Immunity/Vulnerability to the polymorph effect.
                if (j > 0) // Sum was Positive
                    poly = EffectLinkEffects(EffectDamageImmunityIncrease(ConvertNumToDamTypeConstant(i), j), poly);
                else if (j < 0) // Sum was Negative
                    poly = EffectLinkEffects(EffectDamageImmunityDecrease((int)ConvertNumToDamTypeConstant(i), -j), poly);
                DeleteLocalInt(pc, "ws_dam_immun_" + IntToString(i));
            }

            return poly; // Finally, we have the entire (possibly huge :P ) effect to be applied to the shifter.
        }

        // Returns the spell that applied a Polymorph Effect currently on the player.
        // -1 if it was no spell, -2 if no polymorph effect found.
        int ScanForPolymorphEffect(uint pc)
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
        DamageType ConvertNumToDamTypeConstant(int itemDamType)
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
        int ConvertNumToImmunePercentage(int immuneCost)
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

        void WildshapeCopyWeaponProperties(uint pc, uint oldWeapon, uint newWeapon)
        {
            if (GetIsObjectValid(oldWeapon) && GetIsObjectValid(newWeapon))
            {
                ItemProperty ip = GetFirstItemProperty(oldWeapon);
                // If both are Melee Weapons
                if (!GetWeaponRanged(oldWeapon) && !GetWeaponRanged(newWeapon))
                {
                    while (GetIsItemPropertyValid(ip))
                    {
                        AddItemProperty(DurationType.Permanent, ip, newWeapon);
                        ip = GetNextItemProperty(oldWeapon);
                    }// while
                }

                // If both are Ranged Weapons
                else if (GetWeaponRanged(oldWeapon) && GetWeaponRanged(newWeapon))
                {
                    bool unlimitedAmmoFound = false;
                    ItemProperty ipNew;
                    int oldMightyValue = 0;
                    uint ammo;

                    while (GetIsItemPropertyValid(ip))
                    {
                        if (GetItemPropertyType(ip) == ItemPropertyType.UnlimitedAmmunition) // 61 = Unlimited Ammo
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
                        else if (GetItemPropertyType(ip) == ItemPropertyType.Mighty) // 45 = Mighty
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
                            } // while
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
                    } // while
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
    }
}
