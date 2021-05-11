using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Globalization;
using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;
using NWN.Framework.Lite.NWNX;
using NWN.Framework.Lite.NWNX.Enum;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.Module
{
    public class Load
    {
        private static readonly int hours = 24;

        [ScriptHandler("mod_load")]
        public static void OnModuleLoad()
        {
            InitMonkWeapons();
            InitModuleVariables();
            InitWeatherSystem();
            InitAdministration();
            InitServerCalender();
            PrintBootTime();
        }

        private static void PrintBootTime() => Console.WriteLine($"SERVER LOADED:{DateTime.Now.ToString(@"yyyy/MM/dd hh:mm:ss tt", new CultureInfo("en-US"))}");
        private static void ServerMessage1439() => SpeakString($"Server reset in one minute.", TalkVolumeType.Shout);

        public static void InitServerCalender()
        {
            Schedule.Scheduler.ScheduleRepeating(InitWeatherSystem, TimeSpan.FromHours(1));
            Schedule.Scheduler.ScheduleRepeating(ServerMessageEveryHour, TimeSpan.FromHours(1));
            Schedule.Scheduler.ScheduleRepeating(ServerMessage1439, TimeSpan.FromMinutes(1439));
        }

        private static void InitAdministration()
        {
            Administration.SetPlayOption(AdministrationOption.EnforceLegalCharacters, 1);
            Administration.SetPlayOption(AdministrationOption.ExamineChallengeRating, 1);
            Administration.SetPlayOption(AdministrationOption.ExamineEffects, 1);
            Administration.SetPlayOption(AdministrationOption.ItemLevelRestrictions, 1);
            Administration.SetPlayOption(AdministrationOption.PauseAndPlay, 0);
            Administration.SetPlayOption(AdministrationOption.PvpSetting, 2);
            Administration.SetPlayOption(AdministrationOption.RestoreSpellUses, 1);
            Administration.SetPlayOption(AdministrationOption.UseMaxHitpoints, 1);
            Administration.SetPlayOption(AdministrationOption.ValidateSpells, 1);
            Administration.SetPlayOption(AdministrationOption.UseMaxHitpoints, 1);
        }

        private static void InitMonkWeapons()
        {
            Weapon.SetWeaponIsMonkWeapon(BaseItemType.Dart);
            Weapon.SetWeaponIsMonkWeapon(BaseItemType.HandAxe);
            Weapon.SetWeaponIsMonkWeapon(BaseItemType.LightHammer);
            Weapon.SetWeaponIsMonkWeapon(BaseItemType.LightMace);
            Weapon.SetWeaponIsMonkWeapon(BaseItemType.QuarterStaff);
            Weapon.SetWeaponIsMonkWeapon(BaseItemType.Sickle);
            Weapon.SetWeaponIsMonkWeapon(BaseItemType.Shuriken);
            Weapon.SetWeaponUnarmed(BaseItemType.Dart);
            Weapon.SetWeaponUnarmed(BaseItemType.Shuriken);
        }

        private static void InitWeatherSystem()
        {
            uint area = GetFirstArea();
            while (GetIsObjectValid(area))
            {
                if (!GetIsAreaInterior(area))
                {
                    area.InitFog();
                    area.InitSkyboxes();
                    area.InitSunMoonColors();
                    area.InitArea();
                }
                area = GetNextArea();
            }
        }

        private static void InitModuleVariables()
        {
            SetLocalString(GetModule(), GetModule().ToString(), "X2_SWITCH_ENABLE_TAGBASED_SCRIPTS");
            SetLocalString(GetModule(), GetModule().ToString(), "X2_L_STOP_EXPERTISE_ABUSE");
            SetLocalString(GetModule(), GetModule().ToString(), "X2_L_NOTREASURE");
            SetLocalString(GetModule(), GetModule().ToString(), "X3_MOUNTS_EXTERNAL_ONLY");
            SetLocalString(GetModule(), GetModule().ToString(), "X3_MOUNTS_NO_UNDERGROUND");
            SetLocalString(GetModule(), GetModule().ToString(), "X2_S_UD_SPELLSCRIPT");
        }

        private static void ServerMessageEveryHour()
        {
            switch (hours)
            {
                case >= 2:
                    SpeakString($"Server reset in {hours} hours.", TalkVolumeType.Shout);
                    break;
                case 1:
                    SpeakString($"Server reset in {hours} hour.", TalkVolumeType.Shout);
                    break;
                default:
                    ExportAllCharacters();
                    Console.WriteLine($"*** SERVER RESET ***");
                    Administration.ShutdownServer();
                    break;
            }
        }
    }
}
