using System;
using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;
using NWN.Framework.Lite.NWNX;
using static NWN.Framework.Lite.NWScript;
using ItemProperty = NWN.Framework.Lite.ItemProperty;
using Effect = NWN.Framework.Lite.Effect;

namespace EldritchWarrior.Source.Shifter
{
    public class Extensions
    {
        //:://////////////////////////////
        //:: Created By: Iznoghoud
        //:: Last modified: January 19 2004
        /*
	What this script changes:
	- Melee Weapon properties now carry over to the unarmed forms' claws and bite
        attacks.
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

        //******** End Options ***********

        // Includes for various shifter and item related functions
        //# include "x2_inc_itemprop"
        //# include "x2_inc_shifter"

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
            else if (GetBaseItemType(item) == BaseItemType.Belt ||
                GetBaseItemType(item) == BaseItemType.Cloak ||
                GetBaseItemType(item) == BaseItemType.Gloves ||
                GetBaseItemType(item) == BaseItemType.Helmet ||
                GetBaseItemType(item) == BaseItemType.Ring ||
                GetBaseItemType(item) == BaseItemType.Torch)
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
            else if (GetBaseItemType(item) == BaseItemType.LargeShield ||
                GetBaseItemType(item) == BaseItemType.SmallShield ||
                GetBaseItemType(item) == BaseItemType.TowerShield)
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
            if (!GetIsObjectValid(item)) // If not valid, don't do any unnecessary work.
            {
                return poly;
            }

            ItemProperty ip = GetFirstItemProperty(item);
            while (GetIsItemPropertyValid(ip)) // Loop through all the item properties
            {
                if (GetIsStackingProperty(ip)) // See if it's a stacking property
                {
                    int iSubType = GetItemPropertySubType(ip);
                    // This contains whether a bonus is str, dex,
                    // concentration skill, universal saving throws, etc.
                    switch (GetItemPropertyType(ip)) // Which type of property is it?
                    {
                        // In the case of AC modifiers, add it directly to the Polymorphing effect.
                        // For the other cases, set local variables on the player to
                        // make a sum of all the bonuses/penalties. We use local
                        // variables here because there are no arrays in NWScript, and
                        // declaring a variable for every skill, ability type and saving
                        // throw type in here is a little overboard.
                        case  : // Ability Bonus
                            SetLocalInt(pc, "ws_ability_" + IntToString(iSubType), GetLocalInt(pc, "ws_ability_" + IntToString(iSubType)) + GetItemPropertyCostTableValue(ip));
                            break;
                        case 1: // AC Bonus
                            poly = EffectLinkEffects(EffectACIncrease(GetItemPropertyCostTableValue(ip), GetItemACType(item), poly);
                            break;
                        case 27: // Ability Penalty
                            SetLocalInt(pc, "ws_ability_" + IntToString(iSubType), GetLocalInt(pc, "ws_ability_" + IntToString(iSubType)) - GetItemPropertyCostTableValue(ip));
                            break;
                        case 28: // AC penalty
                            poly = EffectLinkEffects(EffectACDecrease(GetItemPropertyCostTableValue(ip)), poly);
                            break;
                        case 52: // Skill Bonus
                            SetLocalInt(pc, "ws_skill_" + IntToString(iSubType), GetLocalInt(pc, "ws_skill_" + IntToString(iSubType)) + GetItemPropertyCostTableValue(ip));
                            break;
                        case 29: // Skill Penalty
                            SetLocalInt(pc, "ws_skill_" + IntToString(iSubType), GetLocalInt(pc, "ws_skill_" + IntToString(iSubType)) - GetItemPropertyCostTableValue(ip));
                            break;
                        case 40: // Saving Throw Bonus vs Element(or universal)
                            SetLocalInt(pc, "ws_save_elem_" + IntToString(iSubType), GetLocalInt(pc, "ws_save_elem_" + IntToString(iSubType)) + GetItemPropertyCostTableValue(ip));
                            break;
                        case 41: // Saving Throw Bonus specific (fort/reflex/will)
                            SetLocalInt(pc, "ws_save_spec_" + IntToString(iSubType), GetLocalInt(pc, "ws_save_spec_" + IntToString(iSubType)) + GetItemPropertyCostTableValue(ip));
                            break;
                        case 49: // Saving Throw Penalty vs Element(or universal)
                            SetLocalInt(pc, "ws_save_elem_" + IntToString(iSubType), GetLocalInt(pc, "ws_save_elem_" + IntToString(iSubType)) - GetItemPropertyCostTableValue(ip));
                            break;
                        case 50: // Saving Throw Penalty specific (fort/reflex/will)
                            SetLocalInt(pc, "ws_save_spec_" + IntToString(iSubType), GetLocalInt(pc, "ws_save_spec_" + IntToString(iSubType)) - GetItemPropertyCostTableValue(ip));
                            break;
                        case 51: // Regeneration
                            SetLocalInt(pc, "ws_regen", GetLocalInt(OBJECT_SELF, "ws_regen") + GetItemPropertyCostTableValue(ip));
                            break;
                        case 20: // Damage Immunity
                            SetLocalInt(pc, "ws_dam_immun_" + IntToString(iSubType), GetLocalInt(pc, "ws_dam_immun_" + IntToString(iSubType)) + ConvertNumToImmunePercentage(GetItemPropertyCostTableValue(ip)));
                            break;
                        case 24: // Damage Vulnerability
                            SetLocalInt(pc, "ws_dam_immun_" + IntToString(iSubType), GetLocalInt(pc, "ws_dam_immun_" + IntToString(iSubType)) - ConvertNumToImmunePercentage(GetItemPropertyCostTableValue(ip)));
                            break;
                    };
                }
                ip = GetNextItemProperty(item);
            }
            return poly;
        }
        // if bItems is true, Adds all the stackable properties on all the objects given to poly.
        // if bItems is false, Adds only the stackable properties on armor and helmet to poly.
        effect AddStackablePropertiesToPoly(object pc, effect poly, int weapon, int bItems, int bArmor, object oArmorOld, object oRing1Old,
                                              object oRing2Old, object oAmuletOld, object oCloakOld, object oBracerOld,
                                              object oBootsOld, object oBeltOld, object oHelmetOld, object oShield, object oWeapon, object oHideOld)
        {
            if (bArmor) // Armor properties get carried over
            {
                poly = ExamineStackableProperties(pc, poly, oArmorOld);
                poly = ExamineStackableProperties(pc, poly, oHelmetOld);
                poly = ExamineStackableProperties(pc, poly, oShield);
                poly = ExamineStackableProperties(pc, poly, oHideOld);
            }
            if (bItems) // Item properties get carried over
            {
                poly = ExamineStackableProperties(pc, poly, oRing1Old);
                poly = ExamineStackableProperties(pc, poly, oRing2Old);
                poly = ExamineStackableProperties(pc, poly, oAmuletOld);
                poly = ExamineStackableProperties(pc, poly, oCloakOld);
                poly = ExamineStackableProperties(pc, poly, oBootsOld);
                poly = ExamineStackableProperties(pc, poly, oBeltOld);
                poly = ExamineStackableProperties(pc, poly, oBracerOld);
            }
            // AC bonuses are attached to poly inside ExamineStackableProperties
            int i; // This will loop over all the different ability subtypes (eg str, dex, con, etc)
            int j; // This will contain the sum of the stackable bonus type we're looking at
            for (i = 0; i <= 5; i++) // **** Handle Ability Bonuses ****
            {
                j = GetLocalInt(pc, "ws_ability_" + IntToString(i));
                // Add the sum of this ability bonus to the polymorph effect.
                if (j > 0) // Sum was Positive
                    poly = EffectLinkEffects(EffectAbilityIncrease(i, j), poly);
                else if (j < 0) // Sum was Negative
                    poly = EffectLinkEffects(EffectAbilityDecrease(i, -j), poly);
                DeleteLocalInt(pc, "ws_ability_" + IntToString(i));
            }
            for (i = 0; i <= 26; i++) // **** Handle Skill Bonuses ****
            {
                j = GetLocalInt(pc, "ws_skill_" + IntToString(i));
                // Add the sum of this skill bonus to the polymorph effect.
                if (j > 0) // Sum was Positive
                    poly = EffectLinkEffects(EffectSkillIncrease(i, j), poly);
                else if (j < 0) // Sum was Negative
                    poly = EffectLinkEffects(EffectSkillDecrease(i, -j), poly);
                DeleteLocalInt(pc, "ws_skill_" + IntToString(i));
            }
            for (i = 0; i <= 21; i++) // **** Handle Saving Throw vs element Bonuses ****
            {
                j = GetLocalInt(pc, "ws_save_elem_" + IntToString(i));
                // Add the sum of this saving throw bonus to the polymorph effect.
                if (j > 0) // Sum was Positive
                    poly = EffectLinkEffects(EffectSavingThrowIncrease(SAVING_THROW_ALL, j, i), poly);
                else if (j < 0) // Sum was Negative
                    poly = EffectLinkEffects(EffectSavingThrowDecrease(SAVING_THROW_ALL, -j, i), poly);
                DeleteLocalInt(pc, "ws_save_elem_" + IntToString(i));
            }
            for (i = 0; i <= 3; i++) // **** Handle Saving Throw specific Bonuses ****
            {
                j = GetLocalInt(pc, "ws_save_spec_" + IntToString(i));
                // Add the sum of this saving throw bonus to the polymorph effect.
                if (j > 0) // Sum was Positive
                    poly = EffectLinkEffects(EffectSavingThrowIncrease(i, j), poly);
                else if (j < 0) // Sum was Negative
                    poly = EffectLinkEffects(EffectSavingThrowDecrease(i, -j), poly);
                DeleteLocalInt(pc, "ws_save_spec_" + IntToString(i));
            }
            j = GetLocalInt(pc, "ws_regen");
            if (j > 0)
            {
                poly = EffectLinkEffects(EffectRegenerate(j, 6.0), poly);
                DeleteLocalInt(pc, "ws_regen");
            }
            for (i = 0; i <= 13; i++) // **** Handle Damage Immunity and Vulnerability ****
            {
                j = GetLocalInt(pc, "ws_dam_immun_" + IntToString(i));
                // Add the sum of this Damage Immunity/Vulnerability to the polymorph effect.
                if (j > 0) // Sum was Positive
                    poly = EffectLinkEffects(EffectDamageImmunityIncrease(ConvertNumToDamTypeConstant(i), j), poly);
                else if (j < 0) // Sum was Negative
                    poly = EffectLinkEffects(EffectDamageImmunityDecrease(ConvertNumToDamTypeConstant(i), -j), poly);
                DeleteLocalInt(pc, "ws_dam_immun_" + IntToString(i));
            }

            return poly; // Finally, we have the entire (possibly huge :P ) effect to be applied to the shifter.
        }

        // Returns the spell that applied a Polymorph Effect currently on the player.
        // -1 if it was no spell, -2 if no polymorph effect found.
        int ScanForPolymorphEffect(object pc)
        {
            effect eEffect = GetFirstEffect(pc);
            while (GetIsEffectValid(eEffect))
            {
                if (GetEffectType(eEffect) == EFFECT_TYPE_POLYMORPH)
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
                case 0:
                    return DamageType.Bludgeoning;
                case 1:
                    return DamageType.Piercing;
                case 2:
                    return DamageType.Slashing;
                case 5:
                    return DamageType.Magical;
                case 6:
                    return DamageType.Acid;
                case 7:
                    return DamageType.Cold;
                case 8:
                    return DamageType.Divine;
                case 9:
                    return DamageType.Electrical;
                case 10:
                    return DamageType.Fire;
                case 11:
                    return DamageType.Negative;
                case 12:
                    return DamageType.Positive;
                case 13:
                    return DamageType.Sonic;
                default:
                    return DamageType.Positive;
            };
        }

        // Converts a number from iprp_immuncost.2da to the corresponding percentage of immunity
        int ConvertNumToImmunePercentage(int immuneCost)
        {
            switch (immuneCost)
            {
                case 1:
                    return 5;
                case 2:
                    return 10;
                case 3:
                    return 25;
                case 4:
                    return 50;
                case 5:
                    return 75;
                case 6:
                    return 90;
                case 7:
                    return 100;
                default:
                    return 0;
            };
        }

        void WildshapeCopyWeaponProperties(object pc, object oldWeapon, object newWeapon)
        {
            if (GetIsObjectValid(oldWeapon) && GetIsObjectValid(newWeapon))
            {
                itemproperty ip = GetFirstItemProperty(oldWeapon);
                // If both are Melee Weapons
                if (!GetWeaponRanged(oldWeapon) && !GetWeaponRanged(newWeapon))
                {
                    while (GetIsItemPropertyValid(ip))
                    {
                        AddItemProperty(DURATION_TYPE_PERMANENT, ip, newWeapon);
                        ip = GetNextItemProperty(oldWeapon);
                    }// while
                }

                // If both are Ranged Weapons
                else if (GetWeaponRanged(oldWeapon) && GetWeaponRanged(newWeapon))
                {
                    int bUnlimitedAmmoFound = false;
                    itemproperty ipNew;
                    int iOldMightyValue = 0;
                    object oAmmo;
                    while (GetIsItemPropertyValid(ip))
                    {
                        if (GetItemPropertyType(ip) == 61) // 61 = Unlimited Ammo
                        {
                            // For some reason, when removing/replacing an unlimited
                            // ammo property, the corresponding missile type will get
                            // dropped in the player's inventory, so we have to remove
                            // that missile again to prevent abuse.
                            bUnlimitedAmmoFound = true;
                            oAmmo = GetItemInSlot(INVENTORY_SLOT_ARROWS, pc);
                            if (!GetIsObjectValid(oAmmo))
                                oAmmo = GetItemInSlot(INVENTORY_SLOT_BOLTS, pc);
                            if (!GetIsObjectValid(oAmmo))
                                oAmmo = GetItemInSlot(INVENTORY_SLOT_BULLETS, pc);
                            IPRemoveMatchingItemProperties(newWeapon, ITEM_PROPERTY_UNLIMITED_AMMUNITION, DURATION_TYPE_PERMANENT);
                            AddItemProperty(DURATION_TYPE_PERMANENT, ip, newWeapon);
                            DestroyObject(oAmmo);
                        }
                        else if (GetItemPropertyType(ip) == 45) // 45 = Mighty
                        {
                            ipNew = GetFirstItemProperty(newWeapon);
                            // Find the mighty value of the Polymorph's weapon
                            while (GetIsItemPropertyValid(ipNew))
                            {
                                if (GetItemPropertyType(ipNew) == 45)
                                {
                                    iOldMightyValue = GetItemPropertyCostTableValue(ipNew);
                                    break;
                                }
                                ipNew = GetNextItemProperty(newWeapon);
                            } // while
                              // If new mighty value bigger, remove old one and add new one.
                            if (GetItemPropertyCostTableValue(ip) > iOldMightyValue)
                            {
                                RemoveItemProperty(newWeapon, ipNew);
                                AddItemProperty(DURATION_TYPE_PERMANENT, ip, newWeapon);
                            }
                        }
                        else
                            AddItemProperty(DURATION_TYPE_PERMANENT, ip, newWeapon);

                        ip = GetNextItemProperty(oldWeapon);
                    } // while
                      // Add basic unlimited ammo if necessary
                    if (bUnlimitedAmmoFound == false && !GetItemHasItemProperty(newWeapon, ITEM_PROPERTY_UNLIMITED_AMMUNITION))
                        AddItemProperty(DURATION_TYPE_PERMANENT, ItemPropertyUnlimitedAmmo(), newWeapon);
                }
            }
            else if (GetWeaponRanged(newWeapon))
            {
                // Add basic unlimited ammo if necessary
                if (!GetItemHasItemProperty(newWeapon, ITEM_PROPERTY_UNLIMITED_AMMUNITION))
                    AddItemProperty(DURATION_TYPE_PERMANENT, ItemPropertyUnlimitedAmmo(), newWeapon);
            }
        }

        // Returns true if item is a creature claw or bite.
        bool GetIsCreatureWeapon(uint item) => GetBaseItemType(item) == BaseItemType.CreatureBludgeWeapon ||
            GetBaseItemType(item) == BaseItemType.CreaturePierceWeapon ||
            GetBaseItemType(item) == BaseItemType.CreatureSlashPierceWeapon ||
            GetBaseItemType(item) == BaseItemType.CreatureSlashWeapon;

        // **** End Functions, added by Iznoghoud **** 
    }
}
