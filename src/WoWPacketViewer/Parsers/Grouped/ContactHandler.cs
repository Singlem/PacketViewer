using System;
using WowTools.Core;



namespace WoWPacketViewer
{
    public class ContactHandler : Parser
    {
        public void ReadSingleContactBlock(Parser packet, bool onlineCheck)
        {
            var status = (ContactStatus)packet.ReadByte();
            WriteLine("Status: " + status);

            if (onlineCheck && status != ContactStatus.Online)
                return;

            var area = packet.ReadInt32();
            WriteLine("Area ID: " + area);

            var level = packet.ReadInt32();
            WriteLine("Level: " + level);

            var clss = (Class)packet.ReadInt32();
            WriteLine("Class: " + clss);
        }

        [Parser(OpCodes.SMSG_CONTACT_LIST)]
        public void HandleContactList(Parser packet)
        {
            var flags = (ContactListFlag)packet.ReadInt32();
            WriteLine("List Flags: " + flags);

            var count = packet.ReadInt32();
            WriteLine("Count: " + count);

            for (var i = 0; i < count; i++)
            {
                var guid = packet.ReadGuid();
                WriteLine("GUID: " + guid);

                var cflags = (ContactEntryFlag)packet.ReadInt32();
                WriteLine("Flags: " + cflags);

                var note = packet.ReadCString();
                WriteLine("Note: " + note);

                if (!cflags.HasFlag(ContactEntryFlag.Friend))
                    continue;

                ReadSingleContactBlock(packet, true);
            }

            //if (!packet.IsRead())
            //    WardenHandler.ReadCheatCheckDecryptionBlock(packet);
        }

        [Parser(OpCodes.SMSG_FRIEND_STATUS)]
        public void HandleFriendStatus(Parser packet)
        {
            var result = (ContactResult)packet.ReadByte();
            WriteLine("Result: " + result);

            var guid = packet.ReadGuid();
            WriteLine("GUID: " + guid);

            switch (result)
            {
                case ContactResult.FriendAddedOnline:
                case ContactResult.FriendAddedOffline:
                case ContactResult.Online:
                {
                    if (result != ContactResult.Online)
                    {
                        var note = packet.ReadCString();
                        WriteLine("Note: " + note);
                    }

                    ReadSingleContactBlock(packet, false);
                    break;
                }
            }
        }
    }
}
