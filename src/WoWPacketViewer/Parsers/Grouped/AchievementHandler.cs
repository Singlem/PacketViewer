using System;
using System.Text;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class AchievementHandler : Parser
    {
        [Parser(OpCodes.SMSG_ACHIEVEMENT_DELETED)]
        [Parser(OpCodes.SMSG_CRITERIA_DELETED)]
        public void HandleDeleted(Parser packet)
        {
            packet.ReadInt32("ID");
        }

        [Parser(OpCodes.SMSG_SERVER_FIRST_ACHIEVEMENT)]
        public void HandleServerFirstAchievement(Parser packet)
        {
            packet.ReadString("PlayerName");
            packet.ReadInt64("PlayerGUID");
            packet.ReadInt32("Achievement");
            packet.ReadInt32("LinkedName");
        }

        [Parser(OpCodes.SMSG_ACHIEVEMENT_EARNED)]
        public void HandleAchievementEarned(Parser packet)
        {
            packet.ReadPackedGuid("PlayerGUID");
            packet.ReadInt32("Achievement");
            packet.ReadPackedTime("Time");
            packet.ReadInt32("unk");
        }

        [Parser(OpCodes.SMSG_CRITERIA_UPDATE)]
        public void HandleCriteriaUpdate(Parser packet)
        {
            packet.ReadInt32("CriteriaID");
            packet.ReadPackedGuid("CriteriaCounter");
            packet.ReadPackedGuid("PlayerGUID");
            packet.ReadInt32("unk");
            packet.ReadPackedTime("CriteriaTime");

            for (var i = 0; i < 2; i++)
            {
                packet.ReadInt32("Timer");
            }
        }

        [Parser(OpCodes.SMSG_ALL_ACHIEVEMENT_DATA)]
        public void HandleAllAchievementData(Parser packet)
        {
            packet.ReadInt32("AchievementID");
            packet.ReadPackedTime("AchievementTime");
            
            packet.ReadInt32("CriteriaID");
            packet.ReadPackedGuid("CriteriaCounter");
            packet.ReadPackedGuid("PlayerGUID");
            packet.ReadInt32("unk");
            packet.ReadPackedTime("CriteriaTime");

            for (var i = 0; i < 2; i++)
            {
                packet.ReadInt32("Timer");
            }
        }

        [Parser(OpCodes.CMSG_QUERY_INSPECT_ACHIEVEMENTS)]
        public void HandleInspectAchievementData(Parser packet)
        {
            packet.ReadPackedGuid("GUID");
        }

        [Parser(OpCodes.SMSG_RESPOND_INSPECT_ACHIEVEMENTS)]
        public void HandleInspectAchievementDataResponse(Parser packet)
        {
            packet.ReadPackedGuid("PlayerGUID");

            packet.ReadInt32("AchievementID");
            packet.ReadPackedTime("AchievementTime");

            packet.ReadInt32("CriteriaID");
            packet.ReadPackedGuid("CriteriaCounter");
            packet.ReadPackedGuid("PlayerGUID");
            packet.ReadInt32("unk");
            packet.ReadPackedTime("CriteriaTime");

            for (var i = 0; i < 2; i++)
            {
                packet.ReadInt32("Timer");
            }
        }
    }
}