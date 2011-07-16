using System;
using WowTools.Core;

namespace WoWPacketViewer
{
    [Parser(OpCodes.SMSG_HEALTH_UPDATE)]
    class HealthUpdateParser : Parser
    {

        public override void Parse()
        {
            ReadUInt64("GUID: {0:X16}");
            ReadUInt16("Value");
        }
    }
}
