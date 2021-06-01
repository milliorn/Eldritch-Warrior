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
            int polymorphtype;
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

            bool isWeapon = ShifterMergeWeapon(polymorphtype);

            if (GW_ALWAYS_COPY_ARMOR_PROPS)
                isArmor = TRUE;
            else
                isArmor = ShifterMergeArmor(polymorphtype);

            if (GW_ALWAYS_COPY_ITEM_PROPS)
                isItems = TRUE;
            else
                isItems = ShifterMergeItems(polymorphtype);

            
            // Send message to PC about which items get merged to this form
            
            string sMerge;
            sMerge = "Merged: "; // <c~¬þ>: This is a color code that makes the text behind it sort of light blue.
            if (isArmor) sMerge += "<cazþ>Armor, Helmet, Shield";
            if (isItems) sMerge += ",</c> <caþa>Rings, Amulet, Cloak, Boots, Belt, Bracers";
            if (isWeapon || GW_COPY_WEAPON_PROPS_TO_UNARMED == 1)
                sMerge += ",</c> <cþAA>Weapon";
            else if (GW_COPY_WEAPON_PROPS_TO_UNARMED == 2)
                sMerge += ",</c> <cþAA>Gloves to unarmed attacks";
            else if (GW_COPY_WEAPON_PROPS_TO_UNARMED == 3)
                sMerge += ",</c> <cþAA>Weapon (if you had one equipped) or gloves to unarmed attacks";
            else
                sMerge += ",</c> <cþAA>No weapon or gloves to unarmed attacks";
            uint spellTargetObject = GetSpellTargetObject();
            SendMessageToPC(spellTargetObject, sMerge + ".</c>");


            // Store the old objects so we can access them after the character has
            // changed into his new form

            uint oWeaponOld;
            uint oArmorOld;
            uint oRing1Old;
            uint oRing2Old;
            uint oAmuletOld;
            uint oCloakOld;
            uint oBootsOld;
            uint oBeltOld;
            uint oHelmetOld;
            uint oShield;
            uint oBracerOld;
            uint oHideOld;

            int nServerSaving = GetLocalInt(OBJECT_SELF, "GW_ServerSave");
            if (nServerSaving != TRUE)
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



            // Here the actual polymorphing is done

            Effect effectPolymorph = EffectPolymorph(polymorphtype);

            // Iznoghoud: Handle stacking item properties here.
            effectPolymorph = AddStackablePropertiesToPoly(OBJECT_SELF, effectPolymorph, isWeapon, isItems, isArmor, oArmorOld, oRing1Old, oRing2Old, oAmuletOld, oCloakOld, oBracerOld, oBootsOld, oBeltOld, oHelmetOld, oShield, oWeaponOld, oHideOld);
            
            effectPolymorph = ExtraordinaryEffect(effectPolymorph);
            ClearAllActions(); // prevents an exploit
            Effect visualEffect = EffectVisualEffect(VisualEffectType.Vfx_Imp_Polymorph);
            ApplyEffectToObject(DURATION_TYPE_INSTANT, visualEffect, OBJECT_SELF);
            ApplyEffectToObject(DURATION_TYPE_PERMANENT, effectPolymorph, OBJECT_SELF);
            SignalEvent(spellTargetObject, EventSpellCastAt(OBJECT_SELF, GetSpellId(), false));

            
            // This code handles the merging of item properties
            
            uint oWeaponNew = GetItemInSlot(INVENTORY_SLOT_RIGHTHAND, OBJECT_SELF);
            uint oArmorNew = GetItemInSlot(INVENTORY_SLOT_CARMOUR, OBJECT_SELF);
            uint oClawLeft = GetItemInSlot(INVENTORY_SLOT_CWEAPON_L, OBJECT_SELF);
            uint oClawRight = GetItemInSlot(INVENTORY_SLOT_CWEAPON_R, OBJECT_SELF);
            uint oBite = GetItemInSlot(INVENTORY_SLOT_CWEAPON_B, OBJECT_SELF);

            //identify weapon
            SetIdentified(oWeaponNew, TRUE);

            
            // ...Weapons
            
            if (isWeapon)
            {
                //------------------------------------------------------------------
                // Merge weapon properties...
                //------------------------------------------------------------------
                WildshapeCopyWeaponProperties(spellTargetObject, oWeaponOld, oWeaponNew);
            }
            else
            {
                bool copyGlovesToClaws = false;
                switch (GW_COPY_WEAPON_PROPS_TO_UNARMED)
                {
                    case 1: // Copy over weapon properties to claws/bite
                        WildshapeCopyNonStackProperties(oWeaponOld, oClawLeft, TRUE);
                        WildshapeCopyNonStackProperties(oWeaponOld, oClawRight, TRUE);
                        WildshapeCopyNonStackProperties(oWeaponOld, oBite, TRUE);
                        break;
                    case 2: // Copy over glove properties to claws/bite
                        WildshapeCopyNonStackProperties(oBracerOld, oClawLeft, false);
                        WildshapeCopyNonStackProperties(oBracerOld, oClawRight, false);
                        WildshapeCopyNonStackProperties(oBracerOld, oBite, false);
                        copyGlovesToClaws = TRUE;
                        break;
                    case 3: // Copy over weapon properties to claws/bite if wearing a weapon, otherwise copy gloves
                        if (GetIsObjectValid(oWeaponOld))
                        {
                            WildshapeCopyNonStackProperties(oWeaponOld, oClawLeft, TRUE);
                            WildshapeCopyNonStackProperties(oWeaponOld, oClawRight, TRUE);
                            WildshapeCopyNonStackProperties(oWeaponOld, oBite, TRUE);
                        }
                        else
                        {
                            WildshapeCopyNonStackProperties(oBracerOld, oClawLeft, false);
                            WildshapeCopyNonStackProperties(oBracerOld, oClawRight, false);
                            WildshapeCopyNonStackProperties(oBracerOld, oBite, false);
                            copyGlovesToClaws = TRUE;
                        }
                        break;
                    default: // Do not copy over anything
                        break;
                };
            }

            
            // ...Armor
            
            if (isArmor)
            {
                //----------------------------------------------------------------------
                // Merge item properties from armor and helmet...
                //----------------------------------------------------------------------
                WildshapeCopyNonStackProperties(oArmorOld, oArmorNew);
                WildshapeCopyNonStackProperties(oHelmetOld, oArmorNew);
                WildshapeCopyNonStackProperties(oShield, oArmorNew);
                WildshapeCopyNonStackProperties(oHideOld, oArmorNew);
            }

            
            // ...Magic Items
            
            if (isItems)
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

            
            // Set artificial usage limits for special ability spells to work around
            // the engine limitation of not being able to set a number of uses for
            // spells in the polymorph radial
            
            ShifterSetGWildshapeSpellLimits(spellID);

        }








    }
}