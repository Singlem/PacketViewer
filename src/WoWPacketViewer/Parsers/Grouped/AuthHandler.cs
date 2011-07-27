using System.IO;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class AuthHandler : Parser
    {
        //[Parser(OpCodes.CMSG_AUTH_SESSION)]
        public void AuthSessionHandler(Parser packet)
        {
            WriteLine("Client Build: " + packet.ReadUInt32());
            WriteLine("Unk1: " + packet.ReadUInt32());
            WriteLine("Account: " + packet.ReadCString());
            WriteLine("Unk2: " + packet.ReadUInt32());
            WriteLine("clientSeed: " + packet.ReadUInt32());
            WriteLine("Unk3: " + packet.ReadUInt32());
            WriteLine("Unk4: " + packet.ReadUInt32());
            WriteLine("Unk5: " + packet.ReadUInt32());
            WriteLine("Unk6: " + packet.ReadUInt32());
            WriteLine("Digest: " + packet.ReadBytes(2).ToHexString());

            // addon info
            var addonData = Reader.ReadBytes((int)Reader.BaseStream.Length - (int)Reader.BaseStream.Position);
            var decompressed = addonData.Decompress();

            AppendFormatLine("Decompressed addon data:");
            AppendFormat(decompressed.HexLike(0, decompressed.Length));

            using (var reader = new BinaryReader(new MemoryStream(decompressed)))
            {
                var count = reader.ReadUInt32();
                AppendFormatLine("Addons Count: {0}", count);
                for (var i = 0; i < count; ++i)
                {
                    var addonName = reader.ReadCString();
                    var enabled = reader.ReadByte();
                    var crc = reader.ReadUInt32();
                    var unk7 = reader.ReadUInt32();
                    AppendFormatLine("Addon {0}: name {1}, enabled {2}, crc {3}, unk7 {4}", i, addonName, enabled, crc, unk7);
                }

                var unk8 = reader.ReadUInt32();
                AppendFormatLine("Unk5: {0}", unk8);
            }
            // addon info end
        }
    }
}
