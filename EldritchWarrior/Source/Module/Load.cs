using System;
using System.Globalization;
using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;
using NWN.Framework.Lite.NWNX;
using NWN.Framework.Lite.NWNX.Enum;

namespace Source.Module
{
    public class Load
    {
        // This method will be run whenever the script "x2_mod_def_load" is run. 
        // In our example module, this happens when the server finishes loading the module file.
        // Script names must adhere to the NWN restrictions (alphanumeric with some special characters and no longer than 16 characters)
        // The method name is arbitrary and can be called whatever you want.
        // Methods must be public and static so that the framework can pick them up when the module loads.

        private static readonly int hours = 24;

        [ScriptHandler("x2_mod_def_load")]
        public static void OnModuleLoad()
        {
            Chat.RegisterChatScript("");
            Entrypoints.MainLoopEvent += (sender, args) => Schedule.Scheduler.Process();

            InitScheduler();
            InitMonkWeapons();
            InitModuleVariables();
            InitWeatherSystem();
            InitAdministration();
            InitServerCalender();

            PrintBootTime();
        }

        private static void InitScheduler() => Entrypoints.MainLoopEvent += (sender, args) => Schedule.Scheduler.Process();
        private static void PrintBootTime() => Console.WriteLine($"SERVER LOADED:{DateTime.Now.ToString(@"yyyy/MM/dd hh:mm:ss tt", new CultureInfo("en-US"))}");
        private static void ServerMessage1439() => NWScript.SpeakString($"Server reset in one minute.", TalkVolumeType.Shout);


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
            uint area = NWScript.GetFirstArea();
            while (NWScript.GetIsObjectValid(area))
            {
                if (!NWScript.GetIsAreaInterior(area))
                {
                    InitFog(area);
                    InitSkyboxes(area);
                    InitSunMoonColors(area);
                    InitArea(area);
                }
                area = NWScript.GetNextArea();
            }
        }

        private static void InitArea(uint area)
        {
            Area.SetWindPower(area, Random.Next(2));
            Area.SetWeatherChance(area, (WeatherEffectType)Random.Next(2), Random.Next(100));
            Area.SetShadowOpacity(area, Random.Next(100));
        }

        private static void InitSunMoonColors(uint area)
        {
            Area.SetSunMoonColors(area, ColorType.MoonAmbient, Random.Next(16));
            Area.SetSunMoonColors(area, ColorType.MoonDiffuse, Random.Next(16));
            Area.SetSunMoonColors(area, ColorType.SunAmbient, Random.Next(16));
            Area.SetSunMoonColors(area, ColorType.SunDiffuse, Random.Next(16));
        }

        private static void InitFog(uint area)
        {
            NWScript.SetFogAmount(FogType.All, Random.Next(12), area);
            NWScript.SetFogColor(FogType.All, (FogColorType)Random.Next(16), area);
        }

        private static void InitSkyboxes(uint area)
        {
            if (NWScript.GetSkyBox(area) == SkyboxType.None)
            {
                NWScript.SetSkyBox((SkyboxType)Random.Next(4));

                if (NWScript.GetSkyBox(area) == SkyboxType.GrassStorm)
                {
                    NWScript.SetWeather(area, WeatherType.Rain);
                }
                if (NWScript.GetSkyBox(area) == SkyboxType.Icy)
                {
                    NWScript.SetWeather(area, WeatherType.Snow);
                }
            }
        }

        private static void InitModuleVariables()
        {
            NWScript.SetLocalString(NWScript.GetModule(), NWScript.GetModule().ToString(), "X2_SWITCH_ENABLE_TAGBASED_SCRIPTS");
            NWScript.SetLocalString(NWScript.GetModule(), NWScript.GetModule().ToString(), "X2_L_STOP_EXPERTISE_ABUSE");
            NWScript.SetLocalString(NWScript.GetModule(), NWScript.GetModule().ToString(), "X2_L_NOTREASURE");
            NWScript.SetLocalString(NWScript.GetModule(), NWScript.GetModule().ToString(), "X3_MOUNTS_EXTERNAL_ONLY");
            NWScript.SetLocalString(NWScript.GetModule(), NWScript.GetModule().ToString(), "X3_MOUNTS_NO_UNDERGROUND");
            NWScript.SetLocalString(NWScript.GetModule(), NWScript.GetModule().ToString(), "X2_S_UD_SPELLSCRIPT");
        }

        private static void ServerMessageEveryHour()
        {
            switch (hours)
            {
                case >= 2:
                    NWScript.SpeakString($"Server reset in {hours} hours.", TalkVolumeType.Shout);
                    break;
                case 1:
                    NWScript.SpeakString($"Server reset in {hours} hour.", TalkVolumeType.Shout);
                    break;
                default:
                    NWScript.ExportAllCharacters();
                    Console.WriteLine($"*** SERVER RESET ***");
                    Administration.ShutdownServer();
                    break;
            }
        }
    }
}
