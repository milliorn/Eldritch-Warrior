using NWN.Framework.Lite;

namespace EldritchWarrior.Source.Schedule
{
    public class Run
    {
        [ScriptHandler("mod_load")]
        public static void OnModuleLoad() => Entrypoints.MainLoopEvent += (sender, args) => Schedule.Scheduler.Process();
    }
}