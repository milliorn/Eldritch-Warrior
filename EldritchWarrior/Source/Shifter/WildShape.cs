using System;
using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;

using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.Shifter
{
    public class WildShape
    {
        [ScriptHandler("nw_s2_wildshape")]
        public static void Shift()
        {
            int spellID = GetSpellId();
            int polyID = 0;
            int duration = GetLevelByClass(ClassType.Druid);

            //Enter Metamagic conditions
            if (GetMetaMagicFeat() == (int)MetaMagicType.Extend)
            {
                duration = duration * 2;
            }

            //Determine Polymorph subradial type
            if (spellID == 401)
            {
                polyID = (int)PolymorphType.BrownBear;
                if (duration >= 12)
                {
                    polyID = (int)PolymorphType.DireBrownBear;
                }
            }
            else if (spellID == 402)
            {
                polyID = (int)PolymorphType.Panther;
                if (duration >= 12)
                {
                    polyID = (int)PolymorphType.DirePanther;
                }
            }
            else if (spellID == 403)
            {
                polyID = (int)PolymorphType.Wolf;

                if (duration >= 12)
                {
                    polyID = (int)PolymorphType.DireWolf;
                }
            }
            else if (spellID == 404)
            {
                polyID = (int)PolymorphType.Boar;
                if (duration >= 12)
                {
                    polyID = (int)PolymorphType.DireBoar;
                }
            }
            else if (spellID == 405)
            {
                polyID = (int)PolymorphType.Badger;
                if (duration >= 12)
                {
                    polyID = (int)PolymorphType.DireBadger;
                }
            }

            bool isArmor;
            bool isItems;

            //--------------------------------------------------------------------------
            // Determine which items get their item properties merged onto the shifters
            // new form.
            //--------------------------------------------------------------------------
            bool isWeapon = StringToInt(Get2DAString("polymorph", "MergeW", polyID)) == 1;

            if (Extensions.WS_ALWAYS_COPY_ARMOR_PROPS)
                isArmor = true;
            else
                isArmor = Convert.ToBoolean(StringToInt(Get2DAString("polymorph", "MergeA", polyID)) == 1);

            if (Extensions.WS_ALWAYS_COPY_ITEM_PROPS)
                isItems = true;
            else
                isItems = Convert.ToBoolean(StringToInt(Get2DAString("polymorph", "MergeI", polyID)) == 1);
            //--------------------------------------------------------------------------
            // Send message to PC about which items get merged to this form
            //--------------------------------------------------------------------------
            string merge = "Merged: ";
            if (isArmor) merge += "<cazþ>Armor, Helmet, Shield";
            if (isItems) merge += ",</c> <caþa>Rings, Amulet, Cloak, Boots, Belt, Bracers";
            if (isWeapon || Extensions.WS_COPY_WEAPON_PROPS_TO_UNARMED == 1)
                merge += ",</c> <cþAA>Weapon";
            else if (Extensions.WS_COPY_WEAPON_PROPS_TO_UNARMED == 2)
                merge += ",</c> <cþAA>Gloves to unarmed attacks";
            else if (Extensions.WS_COPY_WEAPON_PROPS_TO_UNARMED == 3)
                merge += ",</c> <cþAA>Weapon (if you had one equipped) or gloves to unarmed attacks";
            else
                merge += ",</c> <cþAA>No weapon or gloves to unarmed attacks";

            uint spellTargetObject = GetSpellTargetObject();
            SendMessageToPC(spellTargetObject, $"{merge}.</c>");


            //--------------------------------------------------------------------------
            // Store the old objects so we can access them after the character has
            // changed into his new form
            //--------------------------------------------------------------------------
            uint weaponOld;
            uint armorOld;
            uint ring1Old;
            uint ring2Old;
            uint amuletOld;
            uint cloakOld;
            uint bootsOld;
            uint beltOld;
            uint helmetOld;
            uint shieldOld;
            uint bracerOld;
            uint hideOld;

            bool serverSaving = Convert.ToBoolean(GetLocalInt(OBJECT_SELF, "GW_ServerSave"));
            if (!serverSaving)
            {
                //if not polymorphed get items worn and store on player.
                weaponOld = GetItemInSlot(InventorySlotType.RightHand, OBJECT_SELF);
                armorOld = GetItemInSlot(InventorySlotType.Chest, OBJECT_SELF);
                ring1Old = GetItemInSlot(InventorySlotType.LeftRing, OBJECT_SELF);
                ring2Old = GetItemInSlot(InventorySlotType.RightRing, OBJECT_SELF);
                amuletOld = GetItemInSlot(InventorySlotType.Neck, OBJECT_SELF);
                cloakOld = GetItemInSlot(InventorySlotType.Cloak, OBJECT_SELF);
                bootsOld = GetItemInSlot(InventorySlotType.Boots, OBJECT_SELF);
                beltOld = GetItemInSlot(InventorySlotType.Belt, OBJECT_SELF);
                helmetOld = GetItemInSlot(InventorySlotType.Head, OBJECT_SELF);
                shieldOld = GetItemInSlot(InventorySlotType.LeftHand, OBJECT_SELF);
                bracerOld = GetItemInSlot(InventorySlotType.Arms, OBJECT_SELF);
                hideOld = GetItemInSlot(InventorySlotType.CreatureArmor, OBJECT_SELF);
                SetLocalObject(OBJECT_SELF, "GW_OldWeapon", weaponOld);
                SetLocalObject(OBJECT_SELF, "GW_OldArmor", armorOld);
                SetLocalObject(OBJECT_SELF, "GW_OldRing1", ring1Old);
                SetLocalObject(OBJECT_SELF, "GW_OldRing2", ring2Old);
                SetLocalObject(OBJECT_SELF, "GW_OldAmulet", amuletOld);
                SetLocalObject(OBJECT_SELF, "GW_OldCloak", cloakOld);
                SetLocalObject(OBJECT_SELF, "GW_OldBoots", bootsOld);
                SetLocalObject(OBJECT_SELF, "GW_OldBelt", beltOld);
                SetLocalObject(OBJECT_SELF, "GW_OldHelmet", helmetOld);
                SetLocalObject(OBJECT_SELF, "GW_OldBracer", bracerOld);
                SetLocalObject(OBJECT_SELF, "GW_OldHide", hideOld);

                if (GetIsObjectValid(shieldOld) && GetBaseItemType(shieldOld) != BaseItemType.LargeShield && GetBaseItemType(shieldOld) != BaseItemType.SmallShield && GetBaseItemType(shieldOld) != BaseItemType.TowerShield)
                {
                    shieldOld = OBJECT_INVALID;
                }
                SetLocalObject(OBJECT_SELF, "GW_OldShield", shieldOld);
            }
            else
            {
                //If server saving use items stored earlier.
                weaponOld = GetLocalObject(OBJECT_SELF, "GW_OldWeapon");
                armorOld = GetLocalObject(OBJECT_SELF, "GW_OldArmor");
                ring1Old = GetLocalObject(OBJECT_SELF, "GW_OldRing1");
                ring2Old = GetLocalObject(OBJECT_SELF, "GW_OldRing2");
                amuletOld = GetLocalObject(OBJECT_SELF, "GW_OldAmulet");
                cloakOld = GetLocalObject(OBJECT_SELF, "GW_OldCloak");
                bootsOld = GetLocalObject(OBJECT_SELF, "GW_OldBoots");
                beltOld = GetLocalObject(OBJECT_SELF, "GW_OldBelt");
                helmetOld = GetLocalObject(OBJECT_SELF, "GW_OldHelmet");
                shieldOld = GetLocalObject(OBJECT_SELF, "GW_OldShield");
                bracerOld = GetLocalObject(OBJECT_SELF, "GW_OldBracer");
                hideOld = GetLocalObject(OBJECT_SELF, "GW_OldHide");
                SetLocalObject(OBJECT_SELF, "GW_OldShield", shieldOld);
            }

            Effect effectPolymorph = EffectPolymorph(polyID);

            //--------------------------------------------------------------------------
            // Iznoghoud: Handle sta(VisualEffectType.Vfx_Imp_Polymorph);s here.
            effectPolymorph = Extensions.AddStackablePropertiesToPoly(OBJECT_SELF, effectPolymorph, isWeapon, isItems, isArmor, armorOld, ring1Old, ring2Old, amuletOld, cloakOld, bracerOld, bootsOld, beltOld, helmetOld, shieldOld, weaponOld, hideOld);
            //------------------------------------------------(VisualEffectType.Vfx_Imp_Polymorph);------

            effectPolymorph = ExtraordinaryEffect(effectPolymorph);
            //Fire cast spell at event for the specified target SPELLABILITY_WILD_SHAPE
            SignalEvent(spellTargetObject, EventSpellCastAt(OBJECT_SELF, (SpellType)320, false));

            //Apply the VFX impact and effects
            ClearAllActions(); // prevents an exploit
            ApplyEffectToObject(DurationType.Instant, (Effect)EffectVisualEffect((VisualEffectType)VisualEffectType.Vfx_Imp_Polymorph), OBJECT_SELF);
            ApplyEffectToObject(DurationType.Temporary, effectPolymorph, OBJECT_SELF, HoursToSeconds(duration));

            //--------------------------------------------------------------------------
            // This code handles the merging of item properties
            //--------------------------------------------------------------------------
            uint weaponNew = GetItemInSlot(InventorySlotType.RightHand, OBJECT_SELF);
            uint armorNew = GetItemInSlot(InventorySlotType.CreatureArmor, OBJECT_SELF);
            uint clawLeft = GetItemInSlot(InventorySlotType.CreatureLeft, OBJECT_SELF);
            uint clawRight = GetItemInSlot(InventorySlotType.CreatureRight, OBJECT_SELF);
            uint biteNew = GetItemInSlot(InventorySlotType.CreatureBite, OBJECT_SELF);

            //identify weapon
            SetIdentified(weaponNew, true);

            //--------------------------------------------------------------------------
            // ...Weapons
            //--------------------------------------------------------------------------
            if (isWeapon)
            {
                //------------------------------------------------------------------
                // Merge weapon properties...
                //------------------------------------------------------------------
                Extensions.WildshapeCopyWeaponProperties(spellTargetObject, weaponOld, weaponNew);
            }
            else
            {
                switch (Extensions.GW_COPY_WEAPON_PROPS_TO_UNARMED)
                {
                    case 1: // Copy over weapon properties to claws/bite
                        Extensions.WildshapeCopyNonStackProperties(weaponOld, clawLeft, true);
                        Extensions.WildshapeCopyNonStackProperties(weaponOld, clawRight, true);
                        Extensions.WildshapeCopyNonStackProperties(weaponOld, biteNew, true);
                        break;
                    case 2: // Copy over glove properties to claws/bite
                        Extensions.WildshapeCopyNonStackProperties(bracerOld, clawLeft, false);
                        Extensions.WildshapeCopyNonStackProperties(bracerOld, clawRight, false);
                        Extensions.WildshapeCopyNonStackProperties(bracerOld, biteNew, false);
                        break;
                    case 3: // Copy over weapon properties to claws/bite if wearing a weapon, otherwise copy gloves
                        if (GetIsObjectValid(weaponOld))
                        {
                            Extensions.WildshapeCopyNonStackProperties(weaponOld, clawLeft, true);
                            Extensions.WildshapeCopyNonStackProperties(weaponOld, clawRight, true);
                            Extensions.WildshapeCopyNonStackProperties(weaponOld, biteNew, true);
                        }
                        else
                        {
                            Extensions.WildshapeCopyNonStackProperties(bracerOld, clawLeft, false);
                            Extensions.WildshapeCopyNonStackProperties(bracerOld, clawRight, false);
                            Extensions.WildshapeCopyNonStackProperties(bracerOld, biteNew, false);
                        }
                        break;
                    default: // Do not copy over anything
                        break;
                };
            }

            //--------------------------------------------------------------------------
            // ...Armor
            //--------------------------------------------------------------------------
            if (isArmor)
            {
                //----------------------------------------------------------------------
                // Merge item properties from armor and helmet...
                //----------------------------------------------------------------------
                Extensions.WildshapeCopyNonStackProperties(armorOld, armorNew);
                Extensions.WildshapeCopyNonStackProperties(helmetOld, armorNew);
                Extensions.WildshapeCopyNonStackProperties(shieldOld, armorNew);
                Extensions.WildshapeCopyNonStackProperties(hideOld, armorNew);
            }

            //--------------------------------------------------------------------------
            // ...Magic Items
            //--------------------------------------------------------------------------
            if (isItems)
            {
                //----------------------------------------------------------------------
                // Merge item properties from from rings, amulets, cloak, boots, belt
                //----------------------------------------------------------------------
                Extensions.WildshapeCopyNonStackProperties(ring1Old, armorNew);
                Extensions.WildshapeCopyNonStackProperties(ring2Old, armorNew);
                Extensions.WildshapeCopyNonStackProperties(amuletOld, armorNew);
                Extensions.WildshapeCopyNonStackProperties(cloakOld, armorNew);
                Extensions.WildshapeCopyNonStackProperties(bootsOld, armorNew);
                Extensions.WildshapeCopyNonStackProperties(beltOld, armorNew);
                Extensions.WildshapeCopyNonStackProperties(bracerOld, armorNew);
            }
        }
    }
}