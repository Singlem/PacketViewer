using System;

namespace WoWPacketViewer
{
    [Flags]
    public enum FeatureStatusFlags
    {
        VoiceChatAllowed = 0x40,
        HasTravelPass = 0x80,
    }
}