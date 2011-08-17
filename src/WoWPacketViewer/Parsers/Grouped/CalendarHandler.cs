using System;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class CalendarHandler : Parser
    {
        [Parser(OpCodes.SMSG_CALENDAR_SEND_CALENDAR)]
        public void HandleSenddCalendar(Parser packet)
        {
            var inviteCount = packet.ReadInt32("InviteCount");
            for (var i = 0; i < inviteCount; ++i)
            {
                UInt64("EventID");
                UInt64("InviteID");
                UInt8("InviteStats");
                UInt8("Mod_Type");
                UInt8("Invite_Type");
                ReadPackedGuid("InvitedBy");
                WriteLine("");
            }

            var EventCount = packet.ReadInt32("EventCount");
            for (var i = 0; i < EventCount; ++i)
            {
                UInt64("EventID");
                CString("EventName");
                UInt32("EventModFlags");
                PackedTime("EventDate");
                UInt32("EventFlags");
                UInt32("DungeonID");
                UInt64("unk");
                ReadPackedGuid("InvitedBy");
                WriteLine("");
            }

            Time("CurrentUnixTime");
            ReadPackedTime("CurrentPacketTime");

            var InstanceResetCount = packet.ReadInt32("InstanceResetCount");
            for (var i = 0; i < InstanceResetCount; ++i)
            {
                UInt32("MapID");
                UInt32("Difficulty");
                Time("ResetTime");
                UInt64("RaidID");
                WriteLine("");
            }

            Time("BaseTime");

            var RaidResetCount = packet.ReadInt32("RaidResetCount");
            for (var i = 0; i < RaidResetCount; ++i)
            {
                UInt32("MapID");
                Time("ResetTime");
                UInt32("NegativeOffset");
                WriteLine("");
            }

            var Counter = packet.ReadInt32();
            WriteLine("Counter: " + Counter + "(Never seen this larger than 0)");
        }
     
        [Parser(OpCodes.SMSG_CALENDAR_COMMAND_RESULT)]
        public void HandleCommandCalendar(Parser packet)
        {
            UInt32("unk");
            UInt8("unk");
            CString("unk");
            packet.ReadEnum<CalendarResponseResult>("Result");
        }

        [Parser(OpCodes.CMSG_CALENDAR_GET_EVENT)]
        public void HandleGetEvent(Parser packet)
        {
            UInt32("EventID");

            if (packet.GetSize() == 8)
                UInt32("unk");
        }

        [Parser(OpCodes.CMSG_CALENDAR_ADD_EVENT)]
        public void HandleAddEvent(Parser packet)
        {
            CString("Name");
            CString("Description");
            UInt8("Type");
            UInt8("Repeat_Option");
            UInt32("maxSize");
            UInt32("dungeonID");
            ReadPackedTime("time");
            UInt32("lockoutTime");
            UInt32("flags");

            var inviteCount = packet.ReadInt32("inviteCount");
            WriteLine("");
            WriteLine("Invited Players");

            for (var i = 0; i < inviteCount; ++i)
            {
                ReadPackedGuid(" PlayerGuid");
                UInt8(" inviteStatus");
                UInt8(" modType");
                WriteLine("");
            }
        }

        [Parser(OpCodes.SMSG_CALENDAR_EVENT_INVITE_ALERT)]
        public void HandleEventInviteAlert(Parser packet)
        {
            UInt64("EventID");
            CString("EventName");
            Time("EventTime");
            UInt32("EventFlags");
            UInt32("EventType");
            UInt32("DungeonID");
            UInt32("unk");
            UInt64("InviteID");
            UInt8("InviteStatus");
            UInt8("Mod_Type");
            UInt32("unk");
            ReadPackedGuid("Inviter_1");
            ReadPackedGuid("Inviter_2");
        }

        [Parser(OpCodes.SMSG_CALENDAR_SEND_EVENT)]
        public void HandleSendEvent(Parser packet)
        {
            UInt8("Invite_Type");
            ReadPackedGuid("Creator");
            UInt32("EventID");
            UInt32("unk");
            CString("Name");
            CString("Description");
            UInt8("Event_Type");
            UInt8("Repeat_Option");
            UInt32("maxSize");
            UInt32("DungeonID");
            UInt32("Eventflags");
            Time("EventTime");
            UInt32("lockoutTime");
            UInt32("unk");
            UInt32("unk");
            
            var inviteCount = packet.ReadInt32("InviteCount");
            for (var i = 0; i < inviteCount; ++i)
            {
                ReadPackedGuid(" PlayerGuid");
                UInt8(" PlayerLevel");
                UInt8(" InviteStatus");
                UInt8(" Mod_Type");
                UInt8(" unk");
                UInt64(" inviteID");
                UInt8(" unk");
                UInt32(" unk");
                WriteLine("");
            }
        }

        [Parser(OpCodes.CMSG_CALENDAR_EVENT_REMOVE_INVITE)]
        public void HandleRemove_Invite(Parser packet)
        {
            ReadPackedGuid("Removee'sGuid");
            UInt64("Removee'sInviteID");
            UInt64("unk");
            UInt64("EventID");
        }
    }
}