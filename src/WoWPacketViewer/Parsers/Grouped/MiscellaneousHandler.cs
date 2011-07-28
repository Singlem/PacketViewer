using System;
using System.Text;
using WowTools.Core;

namespace WoWPacketViewer
{
    public enum RealmSplitState
    {
        Normal = 0,
        Split = 1,
        Pending = 2
    }

    public enum WeatherState
    {
        Fine = 0,
        LightRain = 3,
        MediumRain = 4,
        HeavyRain = 5,
        LightSnow = 6,
        MediumSnow = 7,
        HeavySnow = 8,
        LightSandstorm = 22,
        MediumSandstorm = 41,
        HeavySandstorm = 42,
        Thunder = 86,
        BlackRain = 90
    }

    public class MiscellaneousHandler : Parser
    {
        [Parser(OpCodes.CMSG_PING)]
        public void Ping(Parser packet)
        {
            packet.ReadInt32("Ping");
            packet.ReadInt32("Latency");
        }

        [Parser(OpCodes.SMSG_PONG)]
        public void Pong(Parser packet)
        {
            packet.ReadInt32("Ping");
        }

        [Parser(OpCodes.CMSG_REALM_SPLIT)]
        public void HandleClientRealmSplit(Parser packet)
        {
            packet.ReadInt32("Unk");
        }

        [Parser(OpCodes.SMSG_REALM_SPLIT)]
        public void HandleServerRealmSplit(Parser packet)
        {
            packet.ReadInt32("Unk");
            packet.ReadEnum<RealmSplitState>("SplitState");
            packet.ReadString("Unk");
        }

        [Parser(OpCodes.SMSG_CLIENTCACHE_VERSION)]
        public void HandleClientCacheVersion(Parser packet)
        {
            packet.ReadInt32("Version");
        }

        [Parser(OpCodes.SMSG_TIME_SYNC_REQ)]
        public void HandleTimeSyncReq(Parser packet)
        {
            packet.ReadInt32("Count");
        }

        [Parser(OpCodes.SMSG_LEARNED_DANCE_MOVES)]
        public void HandleLearnedDanceMoves(Parser packet)
        {
            packet.ReadInt64("DanceMoveID");
        }

        [Parser(OpCodes.SMSG_TRIGGER_CINEMATIC)]
        [Parser(OpCodes.SMSG_TRIGGER_MOVIE)]
        public void HandleTriggerSequence(Parser packet)
        {
            packet.ReadInt32("SequenceID");
        }

        [Parser(OpCodes.SMSG_PLAY_SOUND)]
        [Parser(OpCodes.SMSG_PLAY_MUSIC)]
        [Parser(OpCodes.SMSG_PLAY_OBJECT_SOUND)]
        public void HandleSoundMessages(Parser packet)
        {
            packet.ReadInt32("Sound ID");

            if (packet.GetSize() > 4)
                packet.ReadInt64("GUID");
        }

        [Parser(OpCodes.SMSG_WEATHER)]
        public void HandleWeatherStatus(Parser packet)
        {
            packet.ReadEnum<WeatherState>("State");
            packet.ReadSingle("Grade");
            packet.ReadByte("Unk");
        }

        [Parser(OpCodes.CMSG_TUTORIAL_FLAG)]
        public void HandleTutorialFlag(Parser packet)
        {
            var flag = packet.ReadInt32();
            Console.WriteLine("Flag: 0x" + flag.ToString("X8"));
        }

        [Parser(OpCodes.SMSG_TUTORIAL_FLAGS)]
        public void HandleTutorialFlags(Parser packet)
        {
            For(8, i => ReadUInt32("Mask {0}: 0x{1:X8}", i));
        }

        [Parser(OpCodes.CMSG_AREATRIGGER)]
        public void HandleClientAreaTrigger(Parser packet)
        {
            packet.ReadInt32("AreaTriggerID");
        }

        [Parser(OpCodes.SMSG_PRE_RESURRECT)]
        public void HandlePreRessurect(Parser packet)
        {
            packet.ReadPackedGuid("GUID");
        }

        [Parser(OpCodes.CMSG_SET_ALLOW_LOW_LEVEL_RAID1)]
        [Parser(OpCodes.CMSG_SET_ALLOW_LOW_LEVEL_RAID2)]
        public void HandleLowLevelRaidPackets(Parser packet)
        {
            var unk = packet.ReadInt8("Allow");
        }
        
    }
}