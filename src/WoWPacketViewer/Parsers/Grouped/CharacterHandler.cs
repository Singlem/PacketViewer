using System;
using System.Text;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class CharacterHandler : Parser
    {
        public enum Race
        {
            Human = 1,
            Orc = 2,
            Dwarf = 3,
            NightElf = 4,
            Scourge = 5,
            Tauren = 6,
            Gnome = 7,
            Troll = 8,
            Goblin = 9,
            BloodElf = 10,
            Draenei = 11,
            FelOrc = 12,
            Naga = 13,
            Broken = 14,
            Skeleton = 15,
            Vrykul = 16,
            Tuskarr = 17,
            ForestTroll = 18,
            Taunka = 19,
            NorthrendSkeleton = 20,
            IceTroll = 21
        }

        public enum Class
        {
            Warrior = 1,
            Paladin = 2,
            Hunter = 3,
            Rogue = 4,
            Priest = 5,
            DeathKnight = 6,
            Shaman = 7,
            Mage = 8,
            Warlock = 9,
            Unknown = 10,
            Druid = 11
        }

        public enum Gender
        {
            Male = 0,
            Female = 1,
            None = 2
        }

        [Parser(OpCodes.CMSG_CHAR_CREATE)]
        public void HandleClientCharCreate(Parser packet)
        {
            packet.ReadString("Name");
            packet.ReadEnum<Race>("Race");
            packet.ReadEnum<Class>("Class");
            packet.ReadEnum<Gender>("Gender");
            packet.ReadByte("Face");
            packet.ReadByte("HairStyle");
            packet.ReadByte("HairColor");
            packet.ReadByte("FacialHair");
            packet.ReadByte("OutfitID");
        }

        [Parser(OpCodes.CMSG_CHAR_RENAME)]
        public static void HandleClientCharRename(Parser packet)
        {
            packet.ReadInt64("GUID");
            packet.ReadString("NewName");
        }

        [Parser(OpCodes.SMSG_CHAR_RENAME)]
        public static void HandleServerCharRename(Parser packet)
        {
            packet.ReadByte("Response");
            packet.ReadInt64("GUID");
            packet.ReadString("Name");
        }
    }
}