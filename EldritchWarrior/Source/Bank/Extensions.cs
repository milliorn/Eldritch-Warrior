using NWN.Framework.Lite;
using static NWN.Framework.Lite.NWScript;
using NWN.Framework.Lite.Enum;

namespace EldritchWarrior.Source.Bank
{
    public static class Extensions
    {
        public static readonly int maxItems = 30;

        public static string DBLocationCDKEY(this string cdkey) => $"{GetModuleName()}:{cdkey}";

        public static uint CreateBankObject(uint pc, string chestTag)
        {
            uint bank = CreateObject(ObjectType.Placeable, "_bank_", GetLocation(pc));
            SetLocalString(bank, "CHEST_TAG", chestTag);
            return bank;
        }
    }
}