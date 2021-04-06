using System;
using System.Globalization;
using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;
using NWN.Framework.Lite.NWNX;

namespace Source.Module
{
    class Load
    {
        // This method will be run whenever the script "x2_mod_def_load" is run. 
        // In our example module, this happens when the server finishes loading the module file.
        // Script names must adhere to the NWN restrictions (alphanumeric with some special characters and no longer than 16 characters)
        // The method name is arbitrary and can be called whatever you want.
        // Methods must be public and static so that the framework can pick them up when the module loads.
        [ScriptHandler("x2_mod_def_load")]
        public static void OnModuleLoad()
        {
            uint mod = NWScript.GetModule();
            Random random = new();
            Load load = new();

            PrintBootTime();
            InitMonkWeapons();
            InitModuleVariables(mod);
            InitWeatherSystem(random);
            InitAdministration();
        }

        private static void InitAdministration()
        {
            Administration.SetModuleName("Eldrtich Warrior");
            Administration.SetServerName("");
            Administration.ClearPlayerPassword();
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

        private static void InitWeatherSystem(Random random)
        {
            uint area = NWScript.GetFirstArea();
            while (NWScript.GetIsObjectValid(area))
            {
                if (!NWScript.GetIsAreaInterior(area))
                {
                    InitFog(random, area);
                    InitSkyboxes(random, area);
                    InitSunMoonColors(random, area);
                    Area.SetWindPower(area, random.Next(0, 2));
                    Area.SetWeatherChance(area, (WeatherEffectType)random.Next(0, 2), random.Next(0, 100));
                }
                area = NWScript.GetNextArea();
            }
        }

        private static void InitSunMoonColors(Random random, uint area)
        {
            Area.SetSunMoonColors(area, ColorType.MoonAmbient, random.Next(0, 16));
            Area.SetSunMoonColors(area, ColorType.MoonDiffuse, random.Next(0, 16));
            Area.SetSunMoonColors(area, ColorType.SunAmbient, random.Next(0, 16));
            Area.SetSunMoonColors(area, ColorType.SunDiffuse, random.Next(0, 16));
        }

        private static void InitFog(Random random, uint area)
        {
            NWScript.SetFogAmount(FogType.All, random.Next(0, 12), area);
            NWScript.SetFogColor(FogType.All, (FogColorType)random.Next(0, 16), area);
        }

        private static void InitSkyboxes(Random random, uint area)
        {
            if (NWScript.GetSkyBox(area) == SkyboxType.None)
            {
                NWScript.SetSkyBox((SkyboxType)random.Next(4));

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

        private static void InitModuleVariables(uint mod)
        {
            NWScript.SetLocalString(mod, mod.ToString(), "X2_SWITCH_ENABLE_TAGBASED_SCRIPTS");
            NWScript.SetLocalString(mod, mod.ToString(), "X2_L_STOP_EXPERTISE_ABUSE");
            NWScript.SetLocalString(mod, mod.ToString(), "X2_L_NOTREASURE");
            NWScript.SetLocalString(mod, mod.ToString(), "X3_MOUNTS_EXTERNAL_ONLY");
            NWScript.SetLocalString(mod, mod.ToString(), "X3_MOUNTS_NO_UNDERGROUND");
            NWScript.SetLocalString(mod, mod.ToString(), "X2_S_UD_SPELLSCRIPT");
        }

        private static void PrintBootTime() => Console.WriteLine($"SERVER LOADED:{DateTime.Now.ToString(@"yyyy/MM/dd hh:mm:ss tt", new CultureInfo("en-US"))}");
    }
}
