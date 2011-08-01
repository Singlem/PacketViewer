using System;
using WowTools.Core;
using System.Text;


using Guid = WowTools.Core.Guid;

namespace WoWPacketViewer
{
    public class SessionHandler : Parser
    {
        public Guid LoginGuid;

        //public CharacterInfo LoggedInCharacter;

        [Parser(OpCodes.SMSG_AUTH_CHALLENGE)]
        public void HandleServerAuthChallenge(Parser packet)
        {
            var unk = packet.ReadInt32();
            WriteLine("Shuffle Count: " + unk);

            var seed = packet.ReadInt32();
            WriteLine("Server Seed: " + seed);

            for (var i = 0; i < 8; i++)
            {
                var rand = packet.ReadInt32();
                WriteLine("Server State " + i + ": " + rand);
            }
        }

        /*[Parser(OpCodes.CMSG_AUTH_SESSION)]
        public void HandleAuthSession(Parser packet)
        {
            var build = packet.ReadInt32();
            WriteLine("Client Build: " + build);

            var unk1 = packet.ReadInt32();
            WriteLine("Unk Int32 1: " + unk1);

            var account = packet.ReadCString();
            WriteLine("Account: " + account);

            var unk2 = packet.ReadInt32();
            WriteLine("Unk Int32 2: " + unk2);

            var clientSeed = packet.ReadInt32();
            WriteLine("Client Seed: " + clientSeed);

            var unk3 = packet.ReadInt64();
            WriteLine("Unk Int64: " + unk3);

            var digest = packet.ReadBytes(20);
            WriteLine("Proof SHA-1 Hash: " + digest.ToHexString());

            AddonHandler.ReadClientAddonsList(packet);
        }*/

        [Parser(OpCodes.SMSG_AUTH_RESPONSE)]
        public void HandleAuthResponse(Parser packet)
        {
            var code = (ResponseCode)packet.ReadByte();
            WriteLine("Auth Code: " + code);

            switch (code)
            {
                case ResponseCode.AUTH_OK:
                {
                    ReadAuthResponseInfo(packet);
                    break;
                }
                case ResponseCode.AUTH_WAIT_QUEUE:
                {
                    if (packet.GetSize() <= 6)
                    {
                        ReadQueuePositionInfo(packet);
                        break;
                    }

                    ReadAuthResponseInfo(packet);
                    ReadQueuePositionInfo(packet);
                    break;
                }
            }
        }

        public void ReadAuthResponseInfo(Parser packet)
        {
            var billingRemaining = packet.ReadInt32();
            WriteLine("Billing Time Remaining: " + billingRemaining);

            var billingFlags = (BillingFlag)packet.ReadByte();
            WriteLine("Billing Flags: " + billingFlags);

            var billingRested = packet.ReadInt32();
            WriteLine("Billing Time Rested: " + billingRested);

            var expansion = (ClientType)packet.ReadByte();
            WriteLine("Expansion: " + expansion);
        }

        public void ReadQueuePositionInfo(Parser packet)
        {
            var position = packet.ReadInt32();
            WriteLine("Queue Position: " + position);

            var unkByte = packet.ReadByte();
            WriteLine("Unk Byte: " + unkByte);
        }

        [Parser(OpCodes.CMSG_PLAYER_LOGIN)]
        public void HandlePlayerLogin(Parser packet)
        {
            var guid = packet.ReadGuid();
            WriteLine("GUID: " + guid);
            LoginGuid = guid;
        }

        [Parser(OpCodes.SMSG_CHARACTER_LOGIN_FAILED)]
        public void HandleLoginFailed(Parser packet)
        {
            var unk = packet.ReadByte();
            WriteLine("Unk Byte: " + unk);
        }

        [Parser(OpCodes.SMSG_LOGOUT_RESPONSE)]
        public void HandlePlayerLogoutResponse(Parser packet)
        {
            var unk1 = packet.ReadInt32();
            WriteLine("Unk Int32 1: " + unk1);

            var unk2 = packet.ReadInt32();
            WriteLine("Unk Int32 2: " + unk2);
        }

        [Parser(OpCodes.SMSG_LOGOUT_COMPLETE)]
        public void HandleLogoutComplete(Parser packet)
        {
            //LoggedInCharacter = null;
        }

        [Parser(OpCodes.SMSG_REDIRECT_CLIENT)]
        public void HandleRedirectClient(Parser packet)
        {
            var ip = packet.ReadIPAddress();
            WriteLine("IP Address: " + ip);

            var port = packet.ReadUInt16();
            WriteLine("Port: " + port);

            var unk = packet.ReadInt32();
            WriteLine("Token: " + unk);

            var hash = packet.ReadBytes(20);
            WriteLine("Address SHA-1 Hash: " + hash.ToHexString());
        }

        [Parser(OpCodes.CMSG_REDIRECTION_FAILED)]
        public void HandleRedirectFailed(Parser packet)
        {
            var token = packet.ReadInt32();
            WriteLine("Token: " + token);
        }

        [Parser(OpCodes.CMSG_REDIRECTION_AUTH_PROOF)]
        public void HandleRedirectionAuthProof(Parser packet)
        {
            var name = packet.ReadCString();
            WriteLine("Account: " + name);

            var unk = packet.ReadInt64();
            WriteLine("Unk Int64: " + unk);

            var hash = packet.ReadBytes(20);
            WriteLine("Proof SHA-1 Hash: " + hash.ToHexString());
        }

        [Parser(OpCodes.SMSG_KICK_REASON)]
        public void HandleKickReason(Parser packet)
        {
            var reason = (KickReason)packet.ReadByte();
            WriteLine("Reason: " + reason);

            if (packet.IsRead())
                return;

            var str = packet.ReadCString();
            WriteLine("Unk String: " + str);
        }

        [Parser(OpCodes.SMSG_MOTD)]
        public void HandleMessageOfTheDay(Parser packet)
        {
            For(ReadInt32("Lines count: {0}"), i => ReadCString("Line {0} text: {1}", i));
        }
    }
}
