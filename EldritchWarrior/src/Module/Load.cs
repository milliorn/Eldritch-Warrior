using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;
using System;
using System.Globalization;

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
            InitModuleVariables(mod);
            InitWeatherSystem(random);
        }

        private static void InitWeatherSystem(Random random)
        {
            uint area = NWScript.GetFirstArea();
            while (NWScript.GetIsObjectValid(area))
            {
                if (!NWScript.GetIsAreaInterior(area))
                {
                    NWScript.SetFogAmount(FogType.All, random.Next(0, 12), area);
                    NWScript.SetFogColor(FogType.All, (FogColorType)random.Next(0, 16), area);
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
