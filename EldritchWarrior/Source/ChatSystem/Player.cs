using System;
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
            uint pc = NWScript.GetPCChatSpeaker();
            
            if (TriggerChatTools(message))
            {
                message = message[1..].ToLower();
                Router(pc, message);
            }
        }

        private static void Router(uint pc, string message)
        {
            throw new NotImplementedException();
        }

        private static bool TriggerChatTools(string message) => message.StartsWith(wildcard);
    }
}