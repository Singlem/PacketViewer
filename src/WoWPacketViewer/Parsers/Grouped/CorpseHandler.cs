using System;
using System.Text;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class CorpseHandler : Parser
    {
        [Parser(OpCodes.CMSG_CORPSE_MAP_POSITION_QUERY)]
        public void HandleCorpseMapPositionQuery(Parser packet)
        {
            packet.ReadInt32("LowGUID");
        }

        [Parser(OpCodes.SMSG_CORPSE_RECLAIM_DELAY)]
        public void HandleCorpseReclaimDelay(Parser packet)
        {
            packet.ReadInt32("Delay");
        }

        [Parser(OpCodes.CMSG_RECLAIM_CORPSE)]
        public void HandleReclaimCorpse(Parser packet)
        {
            var guid = packet.ReadInt64("CorpseGUID");
        }
    }
}