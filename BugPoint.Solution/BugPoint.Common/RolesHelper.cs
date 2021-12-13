using System.Diagnostics.CodeAnalysis;

namespace BugPoint.Common
{
    public static class RolesHelper
    {
        public enum Roles
        {
            SuperAdmin=1,
            Admin=2,
            ProjectManager=3,
            Developer=4,
            Tester=5,
            DeveloperTeamLead=6,
            TesterTeamLead=7,
            Reporter=8
        }

    }

    public static class StatusHelper
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum Status
        {

            NEW = 1,
            CONFIRMED = 2,
            IN_PROGRESS = 3,
            RE_OPENED = 4,
            RESOLVED = 5,
            IN_TESTING = 6,
            CLOSED = 7,
            ON_HOLD = 8,
            REJECTED = 9,
            REPLY = 10,
            DUPLICATE = 11,
            UNCONFIRMED = 12,
        }
    }

    public static class DesignationHelper
    {
        public enum Designation
        {
            Technical_Lead=1,
            Sr_Software_Developer,
            Software_Developer,
            UI_Developer,
            SQL_Developer,
            Project_Manager,
            Tester,
            Developer_Team_Lead,
            UI_Developer_Team_Lead,
            Tester_Team_Lead,
            Support_User,
            Business_analyst,
            Support_Team_Lead,
            Consultant,
            Database_Administrator,
            Network_engineers,
            Security_Auditor,
            External_User,
            Administrator,
            Developer_Support
        }

    }
}