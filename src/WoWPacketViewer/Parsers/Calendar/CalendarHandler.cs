using System;
using WowTools.Core;

namespace WowPacketParser.Parsing.Parsers
{
    enum CalendarResponseResult : uint 
    {
        CALENDAR_ERROR_GUILD_EVENTS_EXCEEDED        = 1,
        CALENDAR_ERROR_EVENTS_EXCEEDED              = 2,        // max of 20 (?) exceeded
        CALENDAR_ERROR_SELF_INVITES_EXCEEDED        = 3,
        CALENDAR_ERROR_OTHER_INVITES_EXCEEDED       = 4,        // std::string
        CALENDAR_ERROR_PERMISSIONS                  = 5,
        CALENDAR_ERROR_EVENT_INVALID                = 6,        // Event not found.
        CALENDAR_ERROR_NOT_INVITED                  = 7,
        CALENDAR_ERROR_INTERNAL                     = 8,
        CALENDAR_ERROR_GUILD_PLAYER_NOT_IN_GUILD    = 9,
        CALENDAR_ERROR_ALREADY_INVITED_TO_EVENT_S   = 10,       // std::string
        CALENDAR_ERROR_PLAYER_NOT_FOUND             = 11,
        CALENDAR_ERROR_NOT_ALLIED                   = 12,
        CALENDAR_ERROR_IGNORING_YOU_S               = 13,       // std::string
        CALENDAR_ERROR_INVITES_EXCEEDED             = 14,
        // 15 ?
        CALENDAR_ERROR_INVALID_DATE                 = 16,
        CALENDAR_ERROR_INVALID_TIME                 = 17,
        // 18 ?
        CALENDAR_ERROR_NEEDS_TITLE                  = 19,
        CALENDAR_ERROR_EVENT_PASSED                 = 20,
        CALENDAR_ERROR_EVENT_LOCKED                 = 21,
        CALENDAR_ERROR_DELETE_CREATOR_FAILED        = 22,
        CALENDAR_ERROR_SYSTEM_DISABLED              = 24,
        CALENDAR_ERROR_RESTRICTED_ACCOUNT           = 25,
        CALENDAR_ERROR_ARENA_EVENTS_EXCEEDED        = 26,
        CALENDAR_ERROR_RESTRICTED_LEVEL             = 27,
        CALENDAR_ERROR_USER_SQUELCHED               = 28,
        CALENDAR_ERROR_NO_INVITE                    = 29,
        // 30-35 ?
        CALENDAR_ERROR_EVENT_WRONG_SERVER           = 36,
        CALENDAR_ERROR_INVITE_WRONG_SERVER          = 37,
        CALENDAR_ERROR_NO_GUILD_INVITES             = 38,
        CALENDAR_ERROR_INVALID_SIGNUP               = 39,
        CALENDAR_ERROR_NO_MODERATOR                 = 40,
    };

    public class CalendarHandler : Parser
    {
        [Parser(OpCodes.SMSG_CALENDAR_SEND_CALENDAR)]
        public void HandleSenddCalendar(Parser packet)
        {
            var inviteCount = packet.ReadInt32();
            WriteLine("InviteCount: " + inviteCount);
            for (var i = 0; i < inviteCount; ++i)
            {
                WriteLine("  EventID: " + packet.ReadInt64());
                WriteLine("  InviteID: " + packet.ReadInt64());
                WriteLine("  InviteStats: " + packet.ReadInt8());
                WriteLine("  Mod_Type: " + packet.ReadInt8());
                WriteLine("  Invite_Type: " + packet.ReadInt8());
                WriteLine("  InvitedBy: " + packet.ReadPackedGuid());
                WriteLine("");
            }

            var EventCount = packet.ReadInt32();
            WriteLine("EventCount: " + EventCount);
            for (var i = 0; i < EventCount; ++i)
            {
                WriteLine("  EventID: " + packet.ReadInt64());
                WriteLine("  EventName: " + packet.ReadString());
                WriteLine("  EventModFlags: " + packet.ReadInt32());
                WriteLine("  EventDate: " + packet.ReadTime());
                WriteLine("  EventFlags: " + packet.ReadInt32());
                WriteLine("  DungeonID: " + packet.ReadInt32());
                WriteLine("  InvitedBy: " + packet.ReadPackedGuid());
                WriteLine("");
            }

            WriteLine("CurrentUnixTime: " + packet.ReadTime());
            WriteLine("CurrentPacketTime: " + packet.ReadTime());

            var InstanceResetCount = packet.ReadInt32();
            WriteLine("InstanceResetCount: " + InstanceResetCount);
            for (var i = 0; i < InstanceResetCount; ++i)
            {
                WriteLine("  MapID: " + packet.ReadInt32());
                WriteLine("  Difficulty: " + packet.ReadInt32());
                WriteLine("  ResetTime: " + packet.ReadTime());
                WriteLine("  RaidID: " + packet.ReadInt64());
                WriteLine("");
            }

            WriteLine("BaseTime: " + packet.ReadTime());

            var RaidResetCount = packet.ReadInt32();
            WriteLine("RaidResetCount: " + RaidResetCount);
            for (var i = 0; i < RaidResetCount; ++i)
            {
                WriteLine("  MapID: " + packet.ReadInt32());
                WriteLine("  ResetTime: " + packet.ReadTime());
                WriteLine("  NegativeOffset: " + packet.ReadInt32());
                WriteLine("");
            }

            var Counter = packet.ReadInt32();
            WriteLine("Counter: " + Counter + "(Never seen packet larger that 344)");
            for (var i = 0; i < Counter; ++i)
            {

            }
        }
     
        [Parser(OpCodes.SMSG_CALENDAR_COMMAND_RESULT)]
        public void HandleCommandCalendar(Parser packet)
        {
            WriteLine("unk: " + packet.ReadInt32() + "(unused)");
            WriteLine("unk: " + packet.ReadInt8() + "(unused)");
            WriteLine("unk: " + packet.ReadString());
            packet.ReadEnum<CalendarResponseResult>("Result");
        }

        [Parser(OpCodes.CMSG_CALENDAR_GET_EVENT)]
        public void HandleGetEvent(Parser packet)
        {
            WriteLine("EventID: " + packet.ReadInt32());
        }

        [Parser(OpCodes.CMSG_CALENDAR_ADD_EVENT)]
        public void HandleAddEvent(Parser packet)
        {
            WriteLine("Name: " + packet.ReadString());
            WriteLine("Description: " + packet.ReadString());
            WriteLine("Type: " + packet.ReadInt8());
            WriteLine("Repeat_Option: " + packet.ReadInt8());
            WriteLine("maxSize: " + packet.ReadInt32());
            WriteLine("dungeonID: " + packet.ReadInt32());
            WriteLine("time: " + packet.ReadPackedTime());
            WriteLine("lockoutTime: " + packet.ReadInt32());
            WriteLine("flags: " + packet.ReadInt32());

            var inviteCount = packet.ReadInt32();
            WriteLine("inviteCount: " + inviteCount);
            WriteLine("");
            WriteLine("Invited Players");

            for (var i = 0; i < inviteCount; ++i)
            {
                WriteLine(" PlayerGuid: " + Reader.ReadPackedGuid());
                WriteLine(" inviteStatus: " + packet.ReadInt8());
                WriteLine(" modType: " + packet.ReadInt8());
                WriteLine("");
            }
        }
    }
}