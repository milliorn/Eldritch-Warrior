using System.Linq.Expressions;
using System;
using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;

namespace EldritchWarrior.Source.PC
{
    public class Dying
    {
        [ScriptHandler("test")]
        public static void OnDying()
        {
            uint pc = NWScript.GetLastPlayerDying();
            int stabilize = Module.Random.D10(1);

            NWScript.SendMessageToPC(pc, $"Stabilize roll:{stabilize.ToString()}");

            pc.Scream();

            TryStabilizing(pc, stabilize);
        }

        private static void TryStabilizing(uint pc, int stabilize)
        {
            if (NWScript.GetCurrentHitPoints(pc) <= -127)
            {
                pc.PlayerHasDied();
                return;
            }
            else if (stabilize == 1)
            {

            }
            else
            {
                NWScript.DelayCommand(1.0f, () => OnDying());
            }
        }
    }
}