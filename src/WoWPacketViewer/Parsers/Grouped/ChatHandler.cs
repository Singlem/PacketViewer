using System;
using WowTools.Core;



namespace WoWPacketViewer
{
    public class ChatHandler : Parser
    {
        [Parser(OpCodes.SMSG_EMOTE)]
        public void HandleEmote(Parser packet)
        {
            UInt32("EmoteID");
            ReadGuid("GUID");
        }

        [Parser(OpCodes.SMSG_MESSAGECHAT)]
        public void HandleServerChatMessage(Parser packet)
        {
            var type = (ChatMessageType)packet.ReadByte();
            WriteLine("Type: " + type);
            ReadEnum<Language>("Language");

            ReadGuid("GUID");
            UInt32("unk(uint32)");

            switch (type)
            {
                case ChatMessageType.Say:
                case ChatMessageType.Yell:
                case ChatMessageType.Party:
                case ChatMessageType.PartyLeader:
                case ChatMessageType.Raid:
                case ChatMessageType.RaidLeader:
                case ChatMessageType.RaidWarning:
                case ChatMessageType.Guild:
                case ChatMessageType.Officer:
                case ChatMessageType.Emote:
                case ChatMessageType.TextEmote:
                case ChatMessageType.Whisper:
                case ChatMessageType.WhisperInform:
                case ChatMessageType.System:
                case ChatMessageType.Channel:
                case ChatMessageType.Battleground:
                case ChatMessageType.BattlegroundNeutral:
                case ChatMessageType.BattlegroundAlliance:
                case ChatMessageType.BattlegroundHorde:
                case ChatMessageType.BattlegroundLeader:
                case ChatMessageType.Achievement:
                case ChatMessageType.GuildAchievement:
                {
                    if (type == ChatMessageType.Channel)
                        CString("Channel_Name");
                    
                    ReadGuid("Sender_GUID");
                    break;
                }
                case ChatMessageType.MonsterSay:
                case ChatMessageType.MonsterYell:
                case ChatMessageType.MonsterParty:
                case ChatMessageType.MonsterEmote:
                case ChatMessageType.MonsterWhisper:
                case ChatMessageType.RaidBossEmote:
                case ChatMessageType.RaidBossWhisper:
                case ChatMessageType.BattleNet:
                {
                    UInt32("Name_Length");
                    CString("Name");

                    var target = ReadGuid("Receiver_GUID");

                    if (target.Full != 0)
                    {
                        UInt32("Receiver Name Length");
                        CString("Receiver Name");
                    }
                    break;
                }
            }

            UInt32("Text_Length");
            CString("Text");

            var chatTag = (ChatTag)packet.ReadByte();
            WriteLine("Chat Tag: " + chatTag);

            if (type != ChatMessageType.Achievement && type != ChatMessageType.GuildAchievement)
                return;

            UInt32("Achievement ID");
        }

        [Parser(OpCodes.CMSG_MESSAGECHAT_AFK)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_BATTLEGROUND)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_BATTLEGROUND_LEADER)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_CHANNEL)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_DND)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_EMOTE)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_GUILD)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_OFFICER)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_PARTY)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_PARTY_LEADER)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_RAID)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_RAID_LEADER)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_RAID_WARNING)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_SAY)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_WHISPER)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_YELL)]
        public void HandleClientChatMessage(Parser packet)
        {
            var type = (ChatMessageType)packet.ReadInt32();
            WriteLine("Type: " + type);
            ReadEnum<Language>("Language");

            switch (type)
            {
                case ChatMessageType.Whisper:
                {
                    CString("Recipient");
                    goto default;
                }
                case ChatMessageType.Channel:
                {
                    CString("Channel");
                    goto default;
                }
                default:
                {
                    CString("Message");
                    break;
                }
            }
        }
    }
}
