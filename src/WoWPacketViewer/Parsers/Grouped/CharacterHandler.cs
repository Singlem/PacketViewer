using System;
using System.Text;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class CharacterHandler : Parser
    {
        [Parser(OpCodes.CMSG_CHAR_CREATE)]
        public void HandleClientCharCreate(Parser packet)
        {
            packet.ReadString("Name");

            var race = (Race)packet.ReadByte();
            Console.WriteLine("Race: " + race);

            var chClass = (Class)packet.ReadByte();
            Console.WriteLine("Class: " + chClass);

            var gender = (Gender)packet.ReadByte();
            Console.WriteLine("Gender: " + gender);

            packet.ReadByte("Face");
            packet.ReadByte("HairStyle");
            packet.ReadByte("HairColor");
            packet.ReadByte("FacialHair");
            packet.ReadByte("OutfitID");
        }

        [Parser(OpCodes.CMSG_CHAR_DELETE)]
        public void HandleClientCharDelete(Parser packet)
        {
            packet.ReadInt64("GUID");
        }

        [Parser(OpCodes.CMSG_CHAR_RENAME)]
        public void HandleClientCharRename(Parser packet)
        {
            packet.ReadInt64("GUID");
            packet.ReadString("NewName");
        }

        [Parser(OpCodes.SMSG_CHAR_RENAME)]
        public void HandleServerCharRename(Parser packet)
        {
            var result = (ResponseCode)packet.ReadByte();
            Console.WriteLine("Response: " + result);

            if (result != ResponseCode.RESPONSE_SUCCESS)
                return;

            packet.ReadInt64("GUID");
            packet.ReadString("Name");
        }

        [Parser(OpCodes.SMSG_CHAR_CREATE)]
        [Parser(OpCodes.SMSG_CHAR_DELETE)]
        public void HandleCharResponse(Parser packet)
        {
            var response = (ResponseCode)packet.ReadByte();
            Console.WriteLine("Response: " + response);
        }

        [Parser(OpCodes.CMSG_ALTER_APPEARANCE)]
        public void HandleAlterAppearance(Parser packet)
        {
            packet.ReadByte("HairStyle");
            packet.ReadByte("HairColor");
            packet.ReadByte("Facial Hair");
        }

        [Parser(OpCodes.SMSG_BARBER_SHOP_RESULT)]
        public void HandleBarberShopResult(Parser packet)
        {
            var status = (BarberShopResult)packet.ReadInt32();
            Console.WriteLine("Result: " + status);
        }

        [Parser(OpCodes.CMSG_CHAR_CUSTOMIZE)]
        public void HandleClientCharCustomize(Parser packet)
        {
            packet.ReadInt64("GUID");
            packet.ReadString("NewName");

            var gender = (Gender)packet.ReadByte();
            Console.WriteLine("Gender: " + gender);

            packet.ReadByte("Skin");
            packet.ReadByte("Face");
            packet.ReadByte("HairColor");
            packet.ReadByte("Hair Style");
            packet.ReadByte("Facial Hair");
        }

        [Parser(OpCodes.SMSG_CHAR_CUSTOMIZE)]
        public void HandleServerCharCustomize(Parser packet)
        {
            var response = (ResponseCode)packet.ReadByte();
            Console.WriteLine("Response: " + response);

            if (response != ResponseCode.RESPONSE_SUCCESS)
                return;

            packet.ReadInt64("GUID");
            packet.ReadString("New Name");

            var gender = (Gender)packet.ReadByte();
            Console.WriteLine("Gender: " + gender);

            packet.ReadByte("Skin");
            packet.ReadByte("Face");
            packet.ReadByte("HairColor");
            packet.ReadByte("Hair Style");
            packet.ReadByte("Facial Hair");
        }
    }
}