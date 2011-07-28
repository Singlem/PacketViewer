using System;
using System.Text;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class CombatHandler : Parser
    {
        [Parser(OpCodes.SMSG_AI_REACTION)]
        public void HandleAIReaction(Parser packet)
        {
            packet.ReadInt64("GUID");
            packet.ReadEnum<AIReaction>("Reaction");
        }

        [Parser(OpCodes.SMSG_UPDATE_COMBO_POINTS)]
        public void HandleUpdateComboPoints(Parser packet)
        {
            packet.ReadPackedGuid("GUID");
            packet.ReadByte("Combo Points");
        }
    }
}