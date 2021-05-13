using NWN.Framework.Lite;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.Bank
{
    public static class Extensions
    {
        public static readonly int maxItems = 30;

        public static string dbLocationCDKEY(string cdkey) => $"{GetModuleName()}:{cdKey}";
    }
}