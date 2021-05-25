using System;
using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;

using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.Shifter
{
    public class ElementalShape
    {
        public static void Shift()
        {
            bool elder = false;

            if (GetLevelByClass(ClassType.Druid) >= 20)
            {
                elder = true;
            }

            int spellID = GetSpellId();
            int polyID = 0;

            //Determine Polymorph subradial type
            if (elder == false)
            {
                if (spellID == 397)
                {
                    polyID = (int)PolymorphType.HugeFireElemental;
                }
                else if (spellID == 398)
                {
                    polyID = (int)PolymorphType.HugeWaterElemental;
                }
                else if (spellID == 399)
                {
                    polyID = (int)PolymorphType.HugeEarthElemental;
                }
                else if (spellID == 400)
                {
                    polyID = (int)PolymorphType.HugeAirElemental;
                }
            }
            else
            {
                if (spellID == 397)
                {
                    polyID = (int)PolymorphType.ElderFireElemental;
                }
                else if (spellID == 398)
                {
                    polyID = (int)PolymorphType.ElderWaterElemental;
                }
                else if (spellID == 399)
                {
                    polyID = (int)PolymorphType.ElderEarthElemental;
                }
                else if (spellID == 400)
                {
                    polyID = (int)PolymorphType.ElderAirElemental;
                }
            }

            bool armor = false;
            if (!Extensions.WS_ALWAYS_COPY_ARMOR_PROPS)
                armor = Convert.ToBoolean(StringToInt(Get2DAString("polymorph", "MergeA", polyID)) == 1);

            bool items = false;
            if (!Extensions.WS_ALWAYS_COPY_ITEM_PROPS)
                items = Convert.ToBoolean(StringToInt(Get2DAString("polymorph", "MergeI", polyID)) == 1);
            //--------------------------------------------------------------------------
            // Send message to PC about which items get merged to this form
            //--------------------------------------------------------------------------
            string merge = "Merged: ";
            if (Convert.ToBoolean(armor)) merge += "<cazþ>Armor, Helmet, Shield";
            if (Convert.ToBoolean(items)) merge += ",</c> <caþa>Rings, Amulet, Cloak, Boots, Belt, Bracers";

            //--------------------------------------------------------------------------
            // Determine which items get their item properties merged onto the shifters
            // new form.
            //--------------------------------------------------------------------------
            bool weapon = Convert.ToBoolean(StringToInt(Get2DAString("polymorph", "MergeW", polyID)) == 1);
            if (weapon || Extensions.WS_COPY_WEAPON_PROPS_TO_UNARMED == 1)
                merge += ",</c> <cþAA>Weapon";
            else if (Extensions.WS_COPY_WEAPON_PROPS_TO_UNARMED == 2)
                merge += ",</c> <cþAA>Gloves to unarmed attacks";
            else if (Extensions.WS_COPY_WEAPON_PROPS_TO_UNARMED == 3)
                merge += ",</c> <cþAA>Weapon (if you had one equipped) or gloves to unarmed attacks";
            else
                merge += ",</c> <cþAA>No weapon or gloves to unarmed attacks";

            uint spellTargetObject = GetSpellTargetObject();
            SendMessageToPC(spellTargetObject, merge + ".</c>");

            //--------------------------------------------------------------------------
            // Store the old objects so we can access them after the character has
            // changed into his new form
            //--------------------------------------------------------------------------
            uint oldWeapon;
            uint oldArmor;
            uint oldLeftRing;
            uint oldRightRing;
            uint oldAmulet;
            uint oldCloak;
            uint oldBoots;
            uint oldBelt;
            uint oldHelmet;
            uint oldShield;
            uint oldBracer;
            uint oldHide;

            if (!Convert.ToBoolean(GetLocalInt(OBJECT_SELF, "GW_ServerSave")))
            {
                //if not polymorphed get items worn and store on player.
                oldWeapon = GetItemInSlot(InventorySlotType.RightHand, OBJECT_SELF);
                oldArmor = GetItemInSlot(InventorySlotType.Chest, OBJECT_SELF);
                oldLeftRing = GetItemInSlot(InventorySlotType.LeftRing, OBJECT_SELF);
                oldRightRing = GetItemInSlot(InventorySlotType.RightRing, OBJECT_SELF);
                oldAmulet = GetItemInSlot(InventorySlotType.Neck, OBJECT_SELF);
                oldCloak = GetItemInSlot(InventorySlotType.Cloak, OBJECT_SELF);
                oldBoots = GetItemInSlot(InventorySlotType.Boots, OBJECT_SELF);
                oldBelt = GetItemInSlot(InventorySlotType.Belt, OBJECT_SELF);
                oldHelmet = GetItemInSlot(InventorySlotType.Head, OBJECT_SELF);
                oldShield = GetItemInSlot(InventorySlotType.LeftHand, OBJECT_SELF);
                oldBracer = GetItemInSlot(InventorySlotType.Arms, OBJECT_SELF);
                oldHide = GetItemInSlot(InventorySlotType.CreatureArmor, OBJECT_SELF);
                SetLocalObject(OBJECT_SELF, "GW_OldWeapon", oldWeapon);
                SetLocalObject(OBJECT_SELF, "GW_OldArmor", oldArmor);
                SetLocalObject(OBJECT_SELF, "GW_OldRing1", oldLeftRing);
                SetLocalObject(OBJECT_SELF, "GW_OldRing2", oldRightRing);
                SetLocalObject(OBJECT_SELF, "GW_OldAmulet", oldAmulet);
                SetLocalObject(OBJECT_SELF, "GW_OldCloak", oldCloak);
                SetLocalObject(OBJECT_SELF, "GW_OldBoots", oldBoots);
                SetLocalObject(OBJECT_SELF, "GW_OldBelt", oldBelt);
                SetLocalObject(OBJECT_SELF, "GW_OldHelmet", oldHelmet);
                SetLocalObject(OBJECT_SELF, "GW_OldBracer", oldBracer);
                SetLocalObject(OBJECT_SELF, "GW_OldHide", oldHide);
                if (GetIsObjectValid(oldShield))
                {
                    if (GetBaseItemType(oldShield) != BaseItemType.LargeShield &&
                        GetBaseItemType(oldShield) != BaseItemType.SmallShield &&
                        GetBaseItemType(oldShield) != BaseItemType.LargeShield)
                    {
                        oldShield = OBJECT_INVALID;
                    }
                }
                SetLocalObject(OBJECT_SELF, "GW_OldShield", oldShield);
            }
            else
            {
                //If server saving use items stored earlier.
                oldWeapon = GetLocalObject(OBJECT_SELF, "GW_OldWeapon");
                oldArmor = GetLocalObject(OBJECT_SELF, "GW_OldArmor");
                oldLeftRing = GetLocalObject(OBJECT_SELF, "GW_OldRing1");
                oldRightRing = GetLocalObject(OBJECT_SELF, "GW_OldRing2");
                oldAmulet = GetLocalObject(OBJECT_SELF, "GW_OldAmulet");
                oldCloak = GetLocalObject(OBJECT_SELF, "GW_OldCloak");
                oldBoots = GetLocalObject(OBJECT_SELF, "GW_OldBoots");
                oldBelt = GetLocalObject(OBJECT_SELF, "GW_OldBelt");
                oldHelmet = GetLocalObject(OBJECT_SELF, "GW_OldHelmet");
                oldShield = GetLocalObject(OBJECT_SELF, "GW_OldShield");
                oldBracer = GetLocalObject(OBJECT_SELF, "GW_OldBracer");
                oldHide = GetLocalObject(OBJECT_SELF, "GW_OldHide");
                SetLocalInt(OBJECT_SELF, "GW_ServerSave", 0);
            }

            Effect effectPolymorph = EffectPolymorph(polyID);

            //--------------------------------------------------------------------------
            // Iznoghoud: Handle stacking item properties here.
            effectPolymorph = Extensions.AddStackablePropertiesToPoly(OBJECT_SELF, effectPolymorph, Convert.ToBoolean(weapon), Convert.ToBoolean(items), Convert.ToBoolean(armor), oldArmor, oldLeftRing, oldRightRing, oldAmulet, oldCloak, oldBracer, oldBoots, oldBelt, oldHelmet, oldShield, oldWeapon, oldHide);
            //--------------------------------------------------------------------------

            effectPolymorph = ExtraordinaryEffect(effectPolymorph);
            //Fire cast spell at event for the specified target SPELLABILITY_ELEMENTAL_SHAPE
            SignalEvent(spellTargetObject, EventSpellCastAt(OBJECT_SELF, (SpellType)319, false));

            //Apply the VFX impact and effects
            ClearAllActions(); // prevents an exploit
            ApplyEffectToObject(DurationType.Instant, (Effect)EffectVisualEffect((VisualEffectType)VisualEffectType.Vfx_Imp_Polymorph), OBJECT_SELF);
            ApplyEffectToObject(DurationType.Temporary, effectPolymorph, OBJECT_SELF, HoursToSeconds((int)GetLevelByClass((ClassType)ClassType.Druid)));

            //--------------------------------------------------------------------------
            // This code handles the merging of item properties
            //--------------------------------------------------------------------------
            uint newWeapon = GetItemInSlot(InventorySlotType.RightHand, OBJECT_SELF);
            uint newArmor = GetItemInSlot(InventorySlotType.CreatureArmor, OBJECT_SELF);
            uint newLeftClaw = GetItemInSlot(InventorySlotType.CreatureLeft, OBJECT_SELF);
            uint newRightClaw = GetItemInSlot(InventorySlotType.CreatureRight, OBJECT_SELF);
            uint newBite = GetItemInSlot(InventorySlotType.CreatureBite, OBJECT_SELF);

            //identify weapon
            SetIdentified(newWeapon, true);

            //--------------------------------------------------------------------------
            // ...Weapons
            //--------------------------------------------------------------------------
            if (Convert.ToBoolean(weapon))
            {
                //------------------------------------------------------------------
                // Merge weapon properties...
                //------------------------------------------------------------------
                Extensions.WildshapeCopyWeaponProperties(spellTargetObject, oldWeapon, newWeapon);
            }
            else
            {
                switch (Extensions.GW_COPY_WEAPON_PROPS_TO_UNARMED)
                {
                    case 1: // Copy over weapon properties to claws/newBite
                        Extensions.WildshapeCopyNonStackProperties(oldWeapon, newLeftClaw, true);
                        Extensions.WildshapeCopyNonStackProperties(oldWeapon, newRightClaw, true);
                        Extensions.WildshapeCopyNonStackProperties(oldWeapon, newBite, true);
                        break;
                    case 2: // Copy over glove properties to claws/newBite
                        Extensions.WildshapeCopyNonStackProperties(oldBracer, newLeftClaw, false);
                        Extensions.WildshapeCopyNonStackProperties(oldBracer, newRightClaw, false);
                        Extensions.WildshapeCopyNonStackProperties(oldBracer, newBite, false);
                        break;
                    case 3: // Copy over weapon properties to claws/newBite if wearing a weapon, otherwise copy gloves
                        if (GetIsObjectValid(oldWeapon))
                        {
                            Extensions.WildshapeCopyNonStackProperties(oldWeapon, newLeftClaw, true);
                            Extensions.WildshapeCopyNonStackProperties(oldWeapon, newRightClaw, true);
                            Extensions.WildshapeCopyNonStackProperties(oldWeapon, newBite, true);
                        }
                        else
                        {
                            Extensions.WildshapeCopyNonStackProperties(oldBracer, newLeftClaw, false);
                            Extensions.WildshapeCopyNonStackProperties(oldBracer, newRightClaw, false);
                            Extensions.WildshapeCopyNonStackProperties(oldBracer, newBite, false);
                        }
                        break;
                    default: // Do not copy over anything
                        break;
                };
            }

            //--------------------------------------------------------------------------
            // ...Armor
            //--------------------------------------------------------------------------
            if (armor)
            {
                //----------------------------------------------------------------------
                // Merge item properties from armor and helmet...
                //----------------------------------------------------------------------
                Extensions.WildshapeCopyNonStackProperties(oldArmor, newArmor);
                Extensions.WildshapeCopyNonStackProperties(oldHelmet, newArmor);
                Extensions.WildshapeCopyNonStackProperties(oldShield, newArmor);
                Extensions.WildshapeCopyNonStackProperties(oldHide, newArmor);
            }

            //--------------------------------------------------------------------------
            // ...Magic Items
            //--------------------------------------------------------------------------
            if (items)
            {
                //----------------------------------------------------------------------
                // Merge item properties from from rings, amulets, cloak, boots, belt
                //----------------------------------------------------------------------
                Extensions.WildshapeCopyNonStackProperties(oldLeftRing, newArmor);
                Extensions.WildshapeCopyNonStackProperties(oldRightRing, newArmor);
                Extensions.WildshapeCopyNonStackProperties(oldAmulet, newArmor);
                Extensions.WildshapeCopyNonStackProperties(oldCloak, newArmor);
                Extensions.WildshapeCopyNonStackProperties(oldBoots, newArmor);
                Extensions.WildshapeCopyNonStackProperties(oldBelt, newArmor);
                Extensions.WildshapeCopyNonStackProperties(oldBracer, newArmor);
            }
        }
    }
}