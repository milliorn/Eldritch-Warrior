using System;
using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.Shifter
{
    public class GreaterWildShape
    {
        //:: Greater Wild Shape, Humanoid Shape
        //:: x2_s2_gwildshp
        /*
            Allows the character to shift into one of these
            forms, gaining special abilities

            Credits must be given to mr_bumpkin from the NWN
            community who had the idea of merging item properties
            from weapon and armor to the creatures new forms.

        */
        [ScriptHandler("x2_s2_gwildshp")]
        public static void Shift()
        {
            int spellID = GetSpellId();
            // Feb 13, 2004, Jon: Added scripting to take care of case where it's an NPC
            // using one of the feats. It will randomly pick one of the shapes associated
            // with the feat.
            switch (spellID)
            {
                // Greater Wildshape I
                case 646: spellID = Random(5) + 658; break;
                // Greater Wildshape II
                case 675:
                    switch (Random(3))
                    {
                        case 0: spellID = 672; break;
                        case 1: spellID = 678; break;
                        case 2: spellID = 680; break;
                    }
                    break;
                // Greater Wildshape III
                case 676:
                    switch (Random(3))
                    {
                        case 0: spellID = 670; break;
                        case 1: spellID = 673; break;
                        case 2: spellID = 674; break;
                    }
                    break;
                // Greater Wildshape IV
                case 677:
                    switch (Random(3))
                    {
                        case 0: spellID = 679; break;
                        case 1: spellID = 691; break;
                        case 2: spellID = 694; break;
                    }
                    break;
                // Humanoid Shape
                case 681: spellID = Random(3) + 682; break;
                // Undead Shape
                case 685: spellID = Random(3) + 704; break;
                // Dragon Shape
                case 725: spellID = Random(3) + 707; break;
                // Outsider Shape
                case 732: spellID = Random(3) + 733; break;
                // Construct Shape
                case 737: spellID = Random(3) + 738; break;
            }


            int levelByClass = GetLevelByClass(ClassType.Shifter);
            int polymorphtype = 0;
            // Determine which form to use based on spell id, gender and level

            switch (spellID)
            {

                // Greater Wildshape I - Wyrmling Shape
                case 658: polymorphtype = (int)PolymorphType.WyrmlingRed; break;
                case 659: polymorphtype = (int)PolymorphType.WyrmlingBlue; break;
                case 660: polymorphtype = (int)PolymorphType.WyrmlingBlack; break;
                case 661: polymorphtype = (int)PolymorphType.WyrmlingWhite; break;
                case 662: polymorphtype = (int)PolymorphType.WyrmlingGreen; break;

                // Greater Wildshape II  - Minotaur, Gargoyle, Harpy
                case 672:
                    if (levelByClass < 11) // X2_GW2_EPIC_THRESHOLD
                        polymorphtype = (int)PolymorphType.Harpy;
                    else
                        polymorphtype = 97;
                    break;

                case 678:
                    if (levelByClass < 11)
                        polymorphtype = (int)PolymorphType.Gargoyle;
                    else
                        polymorphtype = 98;
                    break;

                case 680:
                    if (levelByClass < 11)
                        polymorphtype = (int)PolymorphType.Minotaur;
                    else
                        polymorphtype = 96;
                    break;

                // Greater Wildshape III  - Drider, Basilisk, Manticore
                case 670:
                    if (levelByClass < 15) // X2_GW3_EPIC_THRESHOLD
                        polymorphtype = (int)PolymorphType.Basilisk;
                    else
                        polymorphtype = 99;
                    break;

                case 673:
                    if (levelByClass < 15)
                        polymorphtype = (int)PolymorphType.Drider;
                    else
                        polymorphtype = 100;
                    break;

                case 674:
                    if (levelByClass < 15)
                        polymorphtype = (int)PolymorphType.Manticore;
                    else
                        polymorphtype = 101;
                    break;

                // Greater Wildshape IV - Dire Tiger, Medusa, MindFlayer
                case 679: polymorphtype = (int)PolymorphType.Medusa; break;
                case 691: polymorphtype = (int)PolymorphType.Mindflayer; break; // Mindflayer
                case 694: polymorphtype = (int)PolymorphType.DireTiger; break; // DireTiger


                // Humanoid Shape - Kobold Commando, Drow, Lizard Crossbow Specialist
                case 682:
                    if (levelByClass < 17)
                    {
                        if (GetGender(OBJECT_SELF) == GenderType.Male) //drow
                            polymorphtype = 59;
                        else
                            polymorphtype = 70;
                    }
                    else
                    {
                        if (GetGender(OBJECT_SELF) == GenderType.Male) //drow
                            polymorphtype = 105;
                        else
                            polymorphtype = 106;
                    }
                    break;
                case 683:
                    if (levelByClass < 17)
                    {
                        polymorphtype = 82; break; // Lizard
                    }
                    else
                    {
                        polymorphtype = 104; break; // Epic Lizard
                    }
                case 684:
                    if (levelByClass < 17)
                    {
                        polymorphtype = 83; break; // Kobold Commando
                    }
                    else
                    {
                        polymorphtype = 103; break; // Kobold Commando
                    }

                // Undead Shape - Spectre, Risen Lord, Vampire
                case 704: polymorphtype = 75; break; // Risen lord

                case 705:
                    if (GetGender(OBJECT_SELF) == GenderType.Male) // vampire
                        polymorphtype = 74;
                    else
                        polymorphtype = 77;
                    break;

                case 706: polymorphtype = 76; break; /// spectre

                // Dragon Shape - Red Blue and Green Dragons
                case 707: polymorphtype = 72; break; // Ancient Red   Dragon
                case 708: polymorphtype = 71; break; // Ancient Blue  Dragon
                case 709: polymorphtype = 73; break; // Ancient Green Dragon


                // Outsider Shape - Rakshasa, Azer Chieftain, Black Slaad
                case 733:
                    if (GetGender(OBJECT_SELF) == GenderType.Male) //azer
                        polymorphtype = 85;
                    else // anything else is female
                        polymorphtype = 86;
                    break;

                case 734:
                    if (GetGender(OBJECT_SELF) == GenderType.Male) //rakshasa
                        polymorphtype = 88;
                    else // anything else is female
                        polymorphtype = 89;
                    break;

                case 735: polymorphtype = 87; break; // slaad

                // Construct Shape - Stone Golem, Iron Golem, Demonflesh Golem
                case 738: polymorphtype = 91; break; // stone golem
                case 739: polymorphtype = 92; break; // demonflesh golem
                case 740: polymorphtype = 90; break; // iron golem

            }

            bool isArmor;
            bool isItems;

            // Determine which items get their item properties merged onto the shifters
            // new form.

            bool isWeapon = Extensions.ShifterMergeWeapon(polymorphtype);

            if (Extensions.GW_ALWAYS_COPY_ARMOR_PROPS)
                isArmor = true;
            else
                isArmor = Extensions.ShifterMergeArmor(polymorphtype);

            if (Extensions.GW_ALWAYS_COPY_ITEM_PROPS)
                isItems = true;
            else
                isItems = Extensions.ShifterMergeItems(polymorphtype);
            // Send message to PC about which items get merged to this form

            string merge = "Merged: ";
            if (isArmor) merge += "<cazþ>Armor, Helmet, Shield";
            if (isItems) merge += ",</c> <caþa>Rings, Amulet, Cloak, Boots, Belt, Bracers";
            if (isWeapon || Extensions.GW_COPY_WEAPON_PROPS_TO_UNARMED == 1)
                merge += ",</c> <cþAA>Weapon";
            else if (Extensions.GW_COPY_WEAPON_PROPS_TO_UNARMED == 2)
                merge += ",</c> <cþAA>Gloves to unarmed attacks";
            else if (Extensions.GW_COPY_WEAPON_PROPS_TO_UNARMED == 3)
                merge += ",</c> <cþAA>Weapon (if you had one equipped) or gloves to unarmed attacks";
            else
                merge += ",</c> <cþAA>No weapon or gloves to unarmed attacks";

            uint spellTargetObject = GetSpellTargetObject();
            SendMessageToPC(spellTargetObject, merge + ".</c>");

            // Store the old objects so we can access them after the character has
            // changed into his new form

            uint weaponOld;
            uint armorOld;
            uint ring1Old;
            uint ring2Old;
            uint amuletOld;
            uint cloakOld;
            uint bootsOld;
            uint beltOld;
            uint helmetOld;
            uint shield;
            uint bracerOld;
            uint hideOld;

            if (Convert.ToBoolean(GetLocalInt(OBJECT_SELF, "GW_ServerSave")) != true)
            {
                //if not polymorphed get items worn and store on player.
                weaponOld = GetItemInSlot(InventorySlotType.RightHand, OBJECT_SELF);
                armorOld = GetItemInSlot(InventorySlotType.Chest, OBJECT_SELF);
                ring1Old = GetItemInSlot(InventorySlotType.LeftHand, OBJECT_SELF);
                ring2Old = GetItemInSlot(InventorySlotType.RightRing, OBJECT_SELF);
                amuletOld = GetItemInSlot(InventorySlotType.Neck, OBJECT_SELF);
                cloakOld = GetItemInSlot(InventorySlotType.Cloak, OBJECT_SELF);
                bootsOld = GetItemInSlot(InventorySlotType.Boots, OBJECT_SELF);
                beltOld = GetItemInSlot(InventorySlotType.Belt, OBJECT_SELF);
                helmetOld = GetItemInSlot(InventorySlotType.Head, OBJECT_SELF);
                shield = GetItemInSlot(InventorySlotType.LeftHand, OBJECT_SELF);
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

                if (GetIsObjectValid(shield))
                {
                    if (GetBaseItemType(shield) != BaseItemType.LargeShield &&
                        GetBaseItemType(shield) != BaseItemType.SmallShield &&
                        GetBaseItemType(shield) != BaseItemType.TowerShield)
                    {
                        shield = OBJECT_INVALID;
                    }
                }
                SetLocalObject(OBJECT_SELF, "GW_OldShield", shield);
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
                shield = GetLocalObject(OBJECT_SELF, "GW_OldShield");
                bracerOld = GetLocalObject(OBJECT_SELF, "GW_OldBracer");
                hideOld = GetLocalObject(OBJECT_SELF, "GW_OldHide");
                SetLocalInt(OBJECT_SELF, "GW_ServerSave", 0);
            }



            // Here the actual polymorphing is done
            Effect effectPolymorph = EffectPolymorph(polymorphtype);

            // Iznoghoud: Handle stacking item properties here.
            effectPolymorph = Extensions.AddStackablePropertiesToPoly(OBJECT_SELF, effectPolymorph, isWeapon, isItems, isArmor, armorOld, ring1Old, ring2Old, amuletOld, cloakOld, bracerOld, bootsOld, beltOld, helmetOld, shield, weaponOld, hideOld);

            effectPolymorph = ExtraordinaryEffect(effectPolymorph);
            ClearAllActions(); // prevents an exploit
            Effect visualEffect = EffectVisualEffect(VisualEffectType.Vfx_Imp_Polymorph);
            ApplyEffectToObject(DurationType.Instant, visualEffect, OBJECT_SELF);
            ApplyEffectToObject(DurationType.Permanent, effectPolymorph, OBJECT_SELF);
            SignalEvent(spellTargetObject, EventSpellCastAt(OBJECT_SELF, (SpellType)GetSpellId(), false));


            // This code handles the merging of item properties
            uint oWeaponNew = GetItemInSlot(InventorySlotType.RightHand, OBJECT_SELF);
            uint oArmorNew = GetItemInSlot(InventorySlotType.CreatureArmor, OBJECT_SELF);
            uint oClawLeft = GetItemInSlot(InventorySlotType.CreatureLeft, OBJECT_SELF);
            uint oClawRight = GetItemInSlot(InventorySlotType.CreatureRight, OBJECT_SELF);
            uint oBite = GetItemInSlot(InventorySlotType.CreatureBite, OBJECT_SELF);

            SetIdentified(oWeaponNew, true);

            if (isWeapon)
            {
                Extensions.WildshapeCopyWeaponProperties(spellTargetObject, weaponOld, oWeaponNew);
            }
            else
            {
                switch (Extensions.GW_COPY_WEAPON_PROPS_TO_UNARMED)
                {
                    case 1: // Copy over weapon properties to claws/bite
                        Extensions.WildshapeCopyNonStackProperties(weaponOld, oClawLeft, true);
                        Extensions.WildshapeCopyNonStackProperties(weaponOld, oClawRight, true);
                        Extensions.WildshapeCopyNonStackProperties(weaponOld, oBite, true);
                        break;
                    case 2: // Copy over glove properties to claws/bite
                        Extensions.WildshapeCopyNonStackProperties(bracerOld, oClawLeft, false);
                        Extensions.WildshapeCopyNonStackProperties(bracerOld, oClawRight, false);
                        Extensions.WildshapeCopyNonStackProperties(bracerOld, oBite, false);
                        break;
                    case 3: // Copy over weapon properties to claws/bite if wearing a weapon, otherwise copy gloves
                        if (GetIsObjectValid(weaponOld))
                        {
                            Extensions.WildshapeCopyNonStackProperties(weaponOld, oClawLeft, true);
                            Extensions.WildshapeCopyNonStackProperties(weaponOld, oClawRight, true);
                            Extensions.WildshapeCopyNonStackProperties(weaponOld, oBite, true);
                        }
                        else
                        {
                            Extensions.WildshapeCopyNonStackProperties(bracerOld, oClawLeft, false);
                            Extensions.WildshapeCopyNonStackProperties(bracerOld, oClawRight, false);
                            Extensions.WildshapeCopyNonStackProperties(bracerOld, oBite, false);
                        }
                        break;
                    default: // Do not copy over anything
                        break;
                }
            }

            if (isArmor)
            {
                // Merge item properties from armor and helmet...----
                Extensions.WildshapeCopyNonStackProperties(armorOld, oArmorNew);
                Extensions.WildshapeCopyNonStackProperties(helmetOld, oArmorNew);
                Extensions.WildshapeCopyNonStackProperties(shield, oArmorNew);
                Extensions.WildshapeCopyNonStackProperties(hideOld, oArmorNew);
            }


            if (isItems)
            {
                // Merge item properties from from rings, amulets, cloak, boots, belt----
                Extensions.WildshapeCopyNonStackProperties(ring1Old, oArmorNew);
                Extensions.WildshapeCopyNonStackProperties(ring2Old, oArmorNew);
                Extensions.WildshapeCopyNonStackProperties(amuletOld, oArmorNew);
                Extensions.WildshapeCopyNonStackProperties(cloakOld, oArmorNew);
                Extensions.WildshapeCopyNonStackProperties(bootsOld, oArmorNew);
                Extensions.WildshapeCopyNonStackProperties(beltOld, oArmorNew);
                Extensions.WildshapeCopyNonStackProperties(bracerOld, oArmorNew);
            }


            // Set artificial usage limits for special ability spells to work around
            // the engine limitation of not being able to set a number of uses for
            // spells in the polymorph radial

            Extensions.ShifterSetGWildshapeSpellLimits(spellID);
        }
    }
}