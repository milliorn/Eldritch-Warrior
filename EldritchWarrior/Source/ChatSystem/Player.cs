using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;
using NWN.Framework.Lite.NWNX;
using NWN.Framework.Lite.NWNX.Enum;

namespace Source.ChatSystem
{
    public class Player
    {
        private static readonly char wildcard = '!';

        [ScriptHandler("on_player_chat")]
        public static void OnPlayerChat()
        {
            string message = NWScript.GetPCChatMessage();
            if (TriggerChatTools(message))
            {
                message = message[1..].ToLower();

            }
        }

        private static bool TriggerChatTools(string message) => message.StartsWith(wildcard);
    }
}