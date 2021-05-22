using System;
using System.ComponentModel.Design;
using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;
using NWN.Framework.Lite.Bioware;

using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.Shifter
{
    public class ElementalShape
    {
        public static void Shift()
        {
            int spellID = GetSpellId();
            uint spellTargetObject = GetSpellTargetObject();
            Effect effectVisual = EffectVisualEffect(VisualEffectType.Vfx_Imp_Polymorph);
            var polyID;
            int duration = GetLevelByClass(ClassType.Druid); //GetCasterLevel(OBJECT_SELF);
            bool elder = false;

            if (GetLevelByClass(ClassType.Druid) >= 20)
            {
                elder = true;
            }
            //Determine Polymorph subradial type
            if (elder == false)
            {
                if (spellID == 397)
                {
                    polyID = EffectPolymorph(nPolymorphSelection:{})
                }
                else if (spellID == 398)
                {
                    polyID = POLYMORPH_TYPE_HUGE_WATER_ELEMENTAL;
                }
                else if (spellID == 399)
                {
                    polyID = POLYMORPH_TYPE_HUGE_EARTH_ELEMENTAL;
                }
                else if (spellID == 400)
                {
                    polyID = POLYMORPH_TYPE_HUGE_AIR_ELEMENTAL;
                }
            }
            else
            {
                if (spellID == 397)
                {
                    polyID = POLYMORPH_TYPE_ELDER_FIRE_ELEMENTAL;
                }
                else if (spellID == 398)
                {
                    polyID = POLYMORPH_TYPE_ELDER_WATER_ELEMENTAL;
                }
                else if (spellID == 399)
                {
                    polyID = POLYMORPH_TYPE_ELDER_EARTH_ELEMENTAL;
                }
                else if (spellID == 400)
                {
                    polyID = POLYMORPH_TYPE_ELDER_AIR_ELEMENTAL;
                }
            }

            //--------------------------------------------------------------------------
            // Determine which items get their item properties merged onto the shifters
            // new form.
            //--------------------------------------------------------------------------
            int bWeapon;
            int bArmor;
            int bItems;
            int bCopyGlovesToClaws = false;

            bWeapon = StringToInt(Get2DAString("polymorph", "MergeW", polyID)) == 1;

            if (WS_ALWAYS_COPY_ARMOR_PROPS)
                bArmor = true;
            else
                bArmor = StringToInt(Get2DAString("polymorph", "MergeA", polyID)) == 1;

            if (WS_ALWAYS_COPY_ITEM_PROPS)
                bItems = true;
            else
                bItems = StringToInt(Get2DAString("polymorph", "MergeI", polyID)) == 1;

            //--------------------------------------------------------------------------
            // Send message to PC about which items get merged to this form
            //--------------------------------------------------------------------------
            string sMerge;
            sMerge = "Merged: "; // <cazþ>: This is a color code that makes the text behind it blue.
            if (bArmor) sMerge += "<cazþ>Armor, Helmet, Shield";
            if (bItems) sMerge += ",</c> <caþa>Rings, Amulet, Cloak, Boots, Belt, Bracers";
            if (bWeapon || WS_COPY_WEAPON_PROPS_TO_UNARMED == 1)
                sMerge += ",</c> <cþAA>Weapon";
            else if (WS_COPY_WEAPON_PROPS_TO_UNARMED == 2)
                sMerge += ",</c> <cþAA>Gloves to unarmed attacks";
            else if (WS_COPY_WEAPON_PROPS_TO_UNARMED == 3)
                sMerge += ",</c> <cþAA>Weapon (if you had one equipped) or gloves to unarmed attacks";
            else
                sMerge += ",</c> <cþAA>No weapon or gloves to unarmed attacks";
            SendMessageToPC(spellTargetObject, sMerge + ".</c>");

            //--------------------------------------------------------------------------
            // Store the old objects so we can access them after the character has
            // changed into his new form
            //--------------------------------------------------------------------------
            object oWeaponOld;
            object oArmorOld;
            object oRing1Old;
            object oRing2Old;
            object oAmuletOld;
            object oCloakOld;
            object oBootsOld;
            object oBeltOld;
            object oHelmetOld;
            object oShield;
            object oBracerOld;
            object oHideOld;

            int nServerSaving = GetLocalInt(OBJECT_SELF, "GW_ServerSave");
            if (nServerSaving != true)
            {
                //if not polymorphed get items worn and store on player.
                oWeaponOld = GetItemInSlot(INVENTORY_SLOT_RIGHTHAND, OBJECT_SELF);
                oArmorOld = GetItemInSlot(INVENTORY_SLOT_CHEST, OBJECT_SELF);
                oRing1Old = GetItemInSlot(INVENTORY_SLOT_LEFTRING, OBJECT_SELF);
                oRing2Old = GetItemInSlot(INVENTORY_SLOT_RIGHTRING, OBJECT_SELF);
                oAmuletOld = GetItemInSlot(INVENTORY_SLOT_NECK, OBJECT_SELF);
                oCloakOld = GetItemInSlot(INVENTORY_SLOT_CLOAK, OBJECT_SELF);
                oBootsOld = GetItemInSlot(INVENTORY_SLOT_BOOTS, OBJECT_SELF);
                oBeltOld = GetItemInSlot(INVENTORY_SLOT_BELT, OBJECT_SELF);
                oHelmetOld = GetItemInSlot(INVENTORY_SLOT_HEAD, OBJECT_SELF);
                oShield = GetItemInSlot(INVENTORY_SLOT_LEFTHAND, OBJECT_SELF);
                oBracerOld = GetItemInSlot(INVENTORY_SLOT_ARMS, OBJECT_SELF);
                oHideOld = GetItemInSlot(INVENTORY_SLOT_CARMOUR, OBJECT_SELF);
                SetLocalObject(OBJECT_SELF, "GW_OldWeapon", oWeaponOld);
                SetLocalObject(OBJECT_SELF, "GW_OldArmor", oArmorOld);
                SetLocalObject(OBJECT_SELF, "GW_OldRing1", oRing1Old);
                SetLocalObject(OBJECT_SELF, "GW_OldRing2", oRing2Old);
                SetLocalObject(OBJECT_SELF, "GW_OldAmulet", oAmuletOld);
                SetLocalObject(OBJECT_SELF, "GW_OldCloak", oCloakOld);
                SetLocalObject(OBJECT_SELF, "GW_OldBoots", oBootsOld);
                SetLocalObject(OBJECT_SELF, "GW_OldBelt", oBeltOld);
                SetLocalObject(OBJECT_SELF, "GW_OldHelmet", oHelmetOld);
                SetLocalObject(OBJECT_SELF, "GW_OldBracer", oBracerOld);
                SetLocalObject(OBJECT_SELF, "GW_OldHide", oHideOld);
                if (GetIsObjectValid(oShield))
                {
                    if (GetBaseItemType(oShield) != BASE_ITEM_LARGESHIELD &&
                        GetBaseItemType(oShield) != BASE_ITEM_SMALLSHIELD &&
                        GetBaseItemType(oShield) != BASE_ITEM_TOWERSHIELD)
                    {
                        oShield = OBJECT_INVALID;
                    }
                }
                SetLocalObject(OBJECT_SELF, "GW_OldShield", oShield);
            }
            else
            {
                //If server saving use items stored earlier.
                oWeaponOld = GetLocalObject(OBJECT_SELF, "GW_OldWeapon");
                oArmorOld = GetLocalObject(OBJECT_SELF, "GW_OldArmor");
                oRing1Old = GetLocalObject(OBJECT_SELF, "GW_OldRing1");
                oRing2Old = GetLocalObject(OBJECT_SELF, "GW_OldRing2");
                oAmuletOld = GetLocalObject(OBJECT_SELF, "GW_OldAmulet");
                oCloakOld = GetLocalObject(OBJECT_SELF, "GW_OldCloak");
                oBootsOld = GetLocalObject(OBJECT_SELF, "GW_OldBoots");
                oBeltOld = GetLocalObject(OBJECT_SELF, "GW_OldBelt");
                oHelmetOld = GetLocalObject(OBJECT_SELF, "GW_OldHelmet");
                oShield = GetLocalObject(OBJECT_SELF, "GW_OldShield");
                oBracerOld = GetLocalObject(OBJECT_SELF, "GW_OldBracer");
                oHideOld = GetLocalObject(OBJECT_SELF, "GW_OldHide");
                SetLocalInt(OBJECT_SELF, "GW_ServerSave", false);
            }

            Effect ePoly = EffectPolymorph(polyID);

            //--------------------------------------------------------------------------
            // Iznoghoud: Handle stacking item properties here.
            ePoly = AddStackablePropertiesToPoly(OBJECT_SELF, ePoly, bWeapon, bItems, bArmor, oArmorOld, oRing1Old, oRing2Old, oAmuletOld, oCloakOld, oBracerOld, oBootsOld, oBeltOld, oHelmetOld, oShield, oWeaponOld, oHideOld);
            //--------------------------------------------------------------------------

            ePoly = ExtraordinaryEffect(ePoly);
            //Fire cast spell at event for the specified target
            SignalEvent(spellTargetObject, EventSpellCastAt(OBJECT_SELF, SPELLABILITY_ELEMENTAL_SHAPE, false));

            //Apply the VFX impact and effects
            ClearAllActions(); // prevents an exploit
            ApplyEffectToObject(DURATION_TYPE_INSTANT, effectVisual, OBJECT_SELF);
            ApplyEffectToObject(DURATION_TYPE_TEMPORARY, ePoly, OBJECT_SELF, HoursToSeconds(duration));

            //--------------------------------------------------------------------------
            // This code handles the merging of item properties
            //--------------------------------------------------------------------------
            object oWeaponNew = GetItemInSlot(INVENTORY_SLOT_RIGHTHAND, OBJECT_SELF);
            object oArmorNew = GetItemInSlot(INVENTORY_SLOT_CARMOUR, OBJECT_SELF);
            object oClawLeft = GetItemInSlot(INVENTORY_SLOT_CWEAPON_L, OBJECT_SELF);
            object oClawRight = GetItemInSlot(INVENTORY_SLOT_CWEAPON_R, OBJECT_SELF);
            object oBite = GetItemInSlot(INVENTORY_SLOT_CWEAPON_B, OBJECT_SELF);

            //identify weapon
            SetIdentified(oWeaponNew, true);

            //--------------------------------------------------------------------------
            // ...Weapons
            //--------------------------------------------------------------------------
            if (bWeapon)
            {
                //------------------------------------------------------------------
                // Merge weapon properties...
                //------------------------------------------------------------------
                WildshapeCopyWeaponProperties(spellTargetObject, oWeaponOld, oWeaponNew);
            }
            else
            {
                switch (GW_COPY_WEAPON_PROPS_TO_UNARMED)
                {
                    case 1: // Copy over weapon properties to claws/bite
                        WildshapeCopyNonStackProperties(oWeaponOld, oClawLeft, true);
                        WildshapeCopyNonStackProperties(oWeaponOld, oClawRight, true);
                        WildshapeCopyNonStackProperties(oWeaponOld, oBite, true);
                        break;
                    case 2: // Copy over glove properties to claws/bite
                        WildshapeCopyNonStackProperties(oBracerOld, oClawLeft, false);
                        WildshapeCopyNonStackProperties(oBracerOld, oClawRight, false);
                        WildshapeCopyNonStackProperties(oBracerOld, oBite, false);
                        bCopyGlovesToClaws = true;
                        break;
                    case 3: // Copy over weapon properties to claws/bite if wearing a weapon, otherwise copy gloves
                        if (GetIsObjectValid(oWeaponOld))
                        {
                            WildshapeCopyNonStackProperties(oWeaponOld, oClawLeft, true);
                            WildshapeCopyNonStackProperties(oWeaponOld, oClawRight, true);
                            WildshapeCopyNonStackProperties(oWeaponOld, oBite, true);
                        }
                        else
                        {
                            WildshapeCopyNonStackProperties(oBracerOld, oClawLeft, false);
                            WildshapeCopyNonStackProperties(oBracerOld, oClawRight, false);
                            WildshapeCopyNonStackProperties(oBracerOld, oBite, false);
                            bCopyGlovesToClaws = true;
                        }
                        break;
                    default: // Do not copy over anything
                        break;
                };
            }

            //--------------------------------------------------------------------------
            // ...Armor
            //--------------------------------------------------------------------------
            if (bArmor)
            {
                //----------------------------------------------------------------------
                // Merge item properties from armor and helmet...
                //----------------------------------------------------------------------
                WildshapeCopyNonStackProperties(oArmorOld, oArmorNew);
                WildshapeCopyNonStackProperties(oHelmetOld, oArmorNew);
                WildshapeCopyNonStackProperties(oShield, oArmorNew);
                WildshapeCopyNonStackProperties(oHideOld, oArmorNew);
            }

            //--------------------------------------------------------------------------
            // ...Magic Items
            //--------------------------------------------------------------------------
            if (bItems)
            {
                //----------------------------------------------------------------------
                // Merge item properties from from rings, amulets, cloak, boots, belt
                //----------------------------------------------------------------------
                WildshapeCopyNonStackProperties(oRing1Old, oArmorNew);
                WildshapeCopyNonStackProperties(oRing2Old, oArmorNew);
                WildshapeCopyNonStackProperties(oAmuletOld, oArmorNew);
                WildshapeCopyNonStackProperties(oCloakOld, oArmorNew);
                WildshapeCopyNonStackProperties(oBootsOld, oArmorNew);
                WildshapeCopyNonStackProperties(oBeltOld, oArmorNew);
                WildshapeCopyNonStackProperties(oBracerOld, oArmorNew);
            }
        }
    }
}