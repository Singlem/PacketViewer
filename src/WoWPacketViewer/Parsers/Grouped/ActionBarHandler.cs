using System;
using System.Text;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class ActionBarHandler : Parser
    {
        [Parser(OpCodes.SMSG_ACTION_BUTTONS)]
        public void HandleInitialButtons(Parser packet)
        {
            packet.ReadByte("TalentSpec");

            for (var i = 0; i < 144; i++)
            {
                var packed = packet.ReadInt32();

                if (packed == 0)
                    continue;
                
                var action = packed & 0x00FFFFFF;
                Console.WriteLine("Action " + i + ": " + action);

                var type = (ActionButtonType)((packed & 0xFF000000) >> 24);
                Console.WriteLine("Type " + i + ": " + type);
            }
        }
    }
}