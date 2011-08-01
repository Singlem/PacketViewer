using System;
using WowTools.Core;



namespace WoWPacketViewer
{
    public class MailHandler : Parser
    {
        [Parser(OpCodes.SMSG_RECEIVED_MAIL)]
        public void HandleReceivedMail(Parser packet)
        {
            var unkInt = packet.ReadInt32();
            WriteLine("Unk Int32: " + unkInt);
        }
    }
}
