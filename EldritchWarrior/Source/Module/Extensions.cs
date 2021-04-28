using System.Collections.Generic;
using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;
using NWN.Framework.Lite.NWNX;

namespace EldritchWarrior.Source.Module
{
    public static class Extensions
    {
        /* List of DM Public Keys */
        public static readonly Dictionary<string, string> DMList = new()
        {
            { "QR4JFL9A", "milliorn" },
            { "QRMXQ6GM", "milliorn" },
        };

        public static bool GetIsClient(this uint pc) => NWScript.GetIsPC(pc) || NWScript.GetIsPC(pc);

        public static void InitArea(this uint area)
        {
            Area.SetWindPower(area, Random.Next(2));
            Area.SetWeatherChance(area, (WeatherEffectType)Random.Next(2), Random.Next(100));
            Area.SetShadowOpacity(area, Random.Next(100));
        }

        public static void InitSunMoonColors(this uint area)
        {
            Area.SetSunMoonColors(area, ColorType.MoonAmbient, Random.Next(16));
            Area.SetSunMoonColors(area, ColorType.MoonDiffuse, Random.Next(16));
            Area.SetSunMoonColors(area, ColorType.SunAmbient, Random.Next(16));
            Area.SetSunMoonColors(area, ColorType.SunDiffuse, Random.Next(16));
        }

        public static void InitFog(this uint area)
        {
            NWScript.SetFogAmount(FogType.All, Random.Next(12), area);
            NWScript.SetFogColor(FogType.All, (FogColorType)Random.Next(16), area);
        }

        public static void InitSkyboxes(this uint area)
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
    }
}