using NWN.Framework.Lite;
using NWN.Framework.Lite.NWNX;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.Cooldown
{
    public class After
    {
        [ScriptHandler("feat_use_aft")]
        public static void CombatFeat()
        {
            string feat = Events.GetEventData("FEAT_ID");
            System.Console.WriteLine($"{feat}");

            switch (feat)
            {
                case "5":
                    {
                        if (GetLocalInt(OBJECT_SELF, feat) != 1)
                        {
                            SetLocalInt(OBJECT_SELF, feat, 1);
                            DelayCommand(6.0f, () => DeleteLocalInt(OBJECT_SELF, feat));

                            SendMessageToPC(OBJECT_SELF, "FEAT_CALLED_SHOT disabled for 6 seconds");
                            DelayCommand(6.0f, () => SendMessageToPC(OBJECT_SELF, "FEAT_CALLED_SHOT enabled."));
                        }
                    }
                    break;

                case "9":
                    {
                        if (GetLocalInt(OBJECT_SELF, feat) != 1)
                        {
                            SetLocalInt(OBJECT_SELF, feat, 1);
                            DelayCommand(6.0f, () => DeleteLocalInt(OBJECT_SELF, feat));
                            SendMessageToPC(OBJECT_SELF, "FEAT_DISARM disabled for 6 seconds");
                            DelayCommand(6.0f, () => SendMessageToPC(OBJECT_SELF, "FEAT_DISARM enabled."));
                        }
                    }
                    break;

                case "16":
                    {
                        if (GetLocalInt(OBJECT_SELF, feat) != 1)
                        {
                            SetLocalInt(OBJECT_SELF, feat, 1);
                            DelayCommand(6.0f, () => DeleteLocalInt(OBJECT_SELF, feat));
                            SendMessageToPC(OBJECT_SELF, "FEAT_IMPROVED_DISARM disabled for 6 seconds");
                            DelayCommand(6.0f, () => SendMessageToPC(OBJECT_SELF, "FEAT_IMPROVED_DISARM enabled."));
                        }
                    }
                    break;
                case "23":
                    {
                        if (GetLocalInt(OBJECT_SELF, feat) != 1)
                        {
                            SetLocalInt(OBJECT_SELF, feat, 1);
                            DelayCommand(6.0f, () => DeleteLocalInt(OBJECT_SELF, feat));
                            SendMessageToPC(OBJECT_SELF, "FEAT_KNOCKDOWN disabled for 6 seconds");
                            DelayCommand(6.0f, () => SendMessageToPC(OBJECT_SELF, "FEAT_KNOCKDOWN enabled."));
                        }
                    }
                    break;

                case "17":
                    {
                        if (GetLocalInt(OBJECT_SELF, feat) != 1)
                        {
                            SetLocalInt(OBJECT_SELF, feat, 1);
                            DelayCommand(6.0f, () => DeleteLocalInt(OBJECT_SELF, feat));
                            SendMessageToPC(OBJECT_SELF, "FEAT_IMPROVED_KNOCKDOWN disabled for 6 seconds");
                            DelayCommand(6.0f, () => SendMessageToPC(OBJECT_SELF, "FEAT_IMPROVED_KNOCKDOWN enabled."));
                        }
                    }
                    break;
            }
        }
    }
}