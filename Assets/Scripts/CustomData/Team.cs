using System.Collections.Generic;

namespace Lobby.TeamInfo
{
    public class Team
    {
        public long TeamId;

        public TeamMemberInfo TeamOwner;

        public List<TeamMemberInfo> TeamMembers;
    }
}