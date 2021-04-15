using NWN.Framework.Lite;

namespace EldritchWarrior.Source.Items
{
    public class Acquire
    {
        [ScriptHandler("x2_mod_def_aqu")]
        public static void OnAcquireItem()
        {
            var acquiredBy = NWScript.GetModuleItemAcquiredBy();
            var from = NWScript.GetModuleItemAcquiredFrom();
            var acquired = NWScript.GetModuleItemAcquired();

            from.PrintGPValue();
        }
    }
}