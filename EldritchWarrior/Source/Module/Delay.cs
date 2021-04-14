using System;
using System.Threading.Tasks;

namespace EldritchWarrior.Source.Module
{
    public static class Delay
    {
        /*
            https://nitinmanju.medium.com/a-simple-scheduled-task-using-c-and-net-c9d3230769ea
        */
        public static void ActionDays(Action action, int days)
        {
            if (action == null)
            {
                throw new Exception($"{action} is null.");
            }

            Task.Run(async () =>
            {
                action();
                await Task.Delay(TimeSpan.FromDays(days));
            });
        }

        public static void ActionHours(Action action, int hours)
        {
            if (action == null)
            {
                throw new Exception($"{action} is null.");
            }

            Task.Run(async () =>
            {
                action();
                await Task.Delay(TimeSpan.FromHours(hours));
            });
        }

        public static void ActionMilliseconds(Action action, int milliseconds)
        {
            if (action == null)
            {
                throw new Exception($"{action} is null.");
            }

            Task.Run(async () =>
            {
                action();
                await Task.Delay(TimeSpan.FromMilliseconds(milliseconds));
            });
        }

        public static void ActionMinutes(Action action, int Minutes)
        {
            if (action == null)
            {
                throw new Exception($"{action} is null.");
            }

            Task.Run(async () =>
            {
                action();
                await Task.Delay(TimeSpan.FromMinutes(Minutes));
            });
        }

        public static void ActionSeconds(Action action, int seconds)
        {
            if (action == null)
            {
                throw new Exception($"{action} is null.");
            }

            Task.Run(async () =>
            {
                action();
                await Task.Delay(TimeSpan.FromSeconds(seconds));
            });
        }

        public static void ActionTicks(Action action, int ticks)
        {
            if (action == null)
            {
                throw new Exception($"{action} is null.");
            }

            Task.Run(async () =>
            {
                action();
                await Task.Delay(TimeSpan.FromTicks(ticks));
            });
        }
    }
}