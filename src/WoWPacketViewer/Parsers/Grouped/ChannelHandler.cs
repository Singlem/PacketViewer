using System;
using System.Text;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class ChannelHandler : Parser
    {
        [Parser(OpCodes.CMSG_CHANNEL_VOICE_ON)]
        [Parser(OpCodes.CMSG_CHANNEL_VOICE_OFF)]
        public void HandleChannelSetVoice(Parser packet)
        {
            packet.ReadString("ChannelName");
        }

        [Parser(OpCodes.CMSG_CHANNEL_SILENCE_VOICE)]
        [Parser(OpCodes.CMSG_CHANNEL_UNSILENCE_VOICE)]
        [Parser(OpCodes.CMSG_CHANNEL_SILENCE_ALL)]
        [Parser(OpCodes.CMSG_CHANNEL_UNSILENCE_ALL)]
        public void HandleChannelSilencing(Parser packet)
        {
            packet.ReadString("ChannelName");
            packet.ReadString("PlayerName");
        }
    }
}