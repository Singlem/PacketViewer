using System;
using System.Text;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class ActionBarHandler : Parser
    {
        public enum ActionButtonType
        {
            Spell = 0,
            Click = 1,
            EquipSet = 32,
            Macro = 64,
            Item = 128
        }

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