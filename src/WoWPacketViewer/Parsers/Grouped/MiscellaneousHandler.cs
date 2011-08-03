using System;
using System.Text;
using WowTools.Core;

namespace WoWPacketViewer
{
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
            WriteLine("Flag: 0x" + flag.ToString("X8"));
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

        [Parser(OpCodes.SMSG_SET_FACTION_STANDING)]
        public void HandleSetFactionStanding(Parser packet)
        {
            var unk1 = packet.ReadSingle();
            WriteLine("Unk Float: " + unk1);

            var unk2 = packet.ReadByte();
            WriteLine("Unk UInt8: " + unk2);

            var amount = packet.ReadInt32();
            WriteLine("Count: " + amount);

            for (int i = 0; i < amount; i++)
            {
                var listId = packet.ReadInt32();
                WriteLine("Faction List ID: " + listId);

                var standing = packet.ReadInt32();
                WriteLine("Standing: " + standing);
            }
        }

        [Parser(OpCodes.SMSG_SET_PROFICIENCY)]
        public void HandleSetProficiency(Parser packet)
        {
            var itemclass = (ItemClass)packet.ReadByte();
            WriteLine("ItemClass: " + itemclass);

            UInt32("ItemSubClassMask");
        }

        [Parser(OpCodes.SMSG_BINDPOINTUPDATE)]
        public void HandleSetBindPointUpdate(Parser packet)
        {
            var curr = Reader.ReadCoords3();
            WriteLine("Current Position: {0}", curr);

            UInt32("MapID");
            UInt32("ZoneID");
        }

        [Parser(OpCodes.SMSG_LOGIN_SETTIMESPEED)]
        public void HandleLoginSetTimeSpeed(Parser packet)
        {
            PackedTime("GameTime");
            UInt32("GameSpeed");
            UInt32("unk");
        }

        [Parser(OpCodes.SMSG_FEATURE_SYSTEM_STATUS)]
        public void HandleFeatureSystemStatus(Parser packet)
        {
            var HaveTravelPass = packet.ReadBit();
            var VoiceChatAllowed = packet.ReadBit();
            WriteLine("Have Travel Pass: " + HaveTravelPass);
            WriteLine("Voice Chat Allowed: " + VoiceChatAllowed);

            Byte("Complain System Status");
            UInt32("Unknown Mail Url Related Value (SR)");
        }

        [Parser(OpCodes.SMSG_HEALTH_UPDATE)]
        public void HandleHealthUpdate(Parser packet)
        {
            ReadPackedGuid("GUID: {0:X16}");
            ReadUInt16("Value");
        }

        [Parser(OpCodes.CMSG_WORLD_LOGIN)]
        public void HanleWorldLogin(Parser packet)
        {
            UInt32("unk");
            UInt8("unk");
        }
    }
}