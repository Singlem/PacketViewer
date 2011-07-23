using System;
using WowTools.Core;

namespace WowPacketParser.Parsing.Parsers
{
    public class EmptyPackets : Parser
    {
        [Parser(OpCodes.CMSG_CANCEL_TRADE)]
        [Parser(OpCodes.CMSG_WORLD_STATE_UI_TIMER_UPDATE)]
        [Parser(OpCodes.CMSG_CALENDAR_GET_CALENDAR)]
        public void HandleEmptyPacket(Parser packet)
        {
            WriteLine("CMSG Packet that wants responce from server");
        }
    }
}
 