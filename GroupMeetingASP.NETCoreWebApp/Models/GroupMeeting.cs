using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using Dapper;

namespace GroupMeetingASP.NETCoreWebApp.Models
{
    public class GroupMeeting
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string GroupMeetingLeadName { get; set; }
        public string TeamLeadName { get; set; }
        public string Description { get; set; }
        public DateTime GroupMeetingDate { get; set; }
        public string GroupMeetingMode { get; set; }

        static string strConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=ProjectMeeting;Trusted_Connection=True;Integrated Security=SSPI;";


        public static IEnumerable<GroupMeetingMode> GetGroupMeetingModes()
        {
            List<GroupMeetingMode> modesList = new List<GroupMeetingMode>();
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                modesList = con.Query<GroupMeetingMode>("GetGroupMeetingModes").ToList();
            }
            return modesList;
        }



        public static IEnumerable<GroupMeeting> GetGroupMeetings()
        {
            List<GroupMeeting> groupMeetingsList = new List<GroupMeeting>();
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                groupMeetingsList = con.Query<GroupMeeting>("GetGroupMeetingDetails").ToList();
            }
            return groupMeetingsList;
        }

        public static GroupMeeting GetGroupMeetingById(int? id)
        {
            GroupMeeting groupMeeting = new GroupMeeting();
            if (id == null)
                return groupMeeting;

            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@Id", id);
                groupMeeting = con.Query<GroupMeeting>("GetGroupMeetingByID", parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            return groupMeeting;
        }

        public static int AddGroupMeeting(GroupMeeting groupMeeting)
        {
            int rowAffected = 0;
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ProjectName", groupMeeting.ProjectName);
                parameters.Add("@GroupMeetingLeadName", groupMeeting.GroupMeetingLeadName);
                parameters.Add("@TeamLeadName", groupMeeting.TeamLeadName);
                parameters.Add("@Description", groupMeeting.Description);
                parameters.Add("@GroupMeetingDate", groupMeeting.GroupMeetingDate);
                parameters.Add("@GroupMeetingMode", groupMeeting.GroupMeetingMode);
                rowAffected = con.Execute("InsertGroupMeeting", parameters, commandType: CommandType.StoredProcedure);
            }
            return rowAffected;
        }

        public static int UpdateGroupMeeting(GroupMeeting groupMeeting)
        {
            int rowAffected = 0;

            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", groupMeeting.Id);
                parameters.Add("@ProjectName", groupMeeting.ProjectName);
                parameters.Add("@GroupMeetingLeadName", groupMeeting.GroupMeetingLeadName);
                parameters.Add("@TeamLeadName", groupMeeting.TeamLeadName);
                parameters.Add("@Description", groupMeeting.Description);
                parameters.Add("@GroupMeetingDate", groupMeeting.GroupMeetingDate);
                parameters.Add("@GroupMeetingMode", groupMeeting.GroupMeetingMode);
                rowAffected = con.Execute("UpdateGroupMeeting", parameters, commandType: CommandType.StoredProcedure);
            }
            return rowAffected;
        }

        public static int DeleteGroupMeeting(int id)
        {
            int rowAffected = 0;
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                rowAffected = con.Execute("DeleteGroupMeeting", parameters, commandType: CommandType.StoredProcedure);
            }
            return rowAffected;
        }

        public static IEnumerable<GroupMeeting> GetInactiveGroupMeetings(int pageNumber, int pageSize)
        {
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var sql = @"SELECT * FROM GroupMeeting
                            WHERE IsActive = 0
                            ORDER BY Id
                            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                var parameters = new { Offset = (pageNumber - 1) * pageSize, PageSize = pageSize };

                return con.Query<GroupMeeting>(sql, parameters).ToList();
            }
        }

        public static int GetInactiveTotalCount()
        {
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                return con.ExecuteScalar<int>("SELECT COUNT(1) FROM GroupMeeting WHERE IsActive = 0");
            }
        }

        public static IEnumerable<MeetingModeCount> GetInactiveMeetingModeCounts()
        {
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var sql = @"SELECT GroupMeetingMode, COUNT(*) AS Count
                    FROM GroupMeeting
                    WHERE IsActive = 0
                    GROUP BY GroupMeetingMode";

                return con.Query<MeetingModeCount>(sql).ToList();
            }
        }


        public static int AddMeetingBack(int id)
        {
            int rowAffected = 0;
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                rowAffected = con.Execute("AddMeetingBack", parameters, commandType: CommandType.StoredProcedure);
            }
            return rowAffected;
        }

    }

    public class GroupMeetingMode
    {
        public int Id { get; set; }
        public string Mode { get; set; }
    }

    public class MeetingModeCount
    {
        public string GroupMeetingMode { get; set; }
        public int Count { get; set; }
    }
}
