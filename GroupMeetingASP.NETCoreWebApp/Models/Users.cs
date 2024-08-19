using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace GroupMeetingASP.NETCoreWebApp.Models
{
    public class Users
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime CreatedAt { get; set; }

        static string strConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=ProjectMeeting;Trusted_Connection=True;Integrated Security=SSPI;";

        public static int AddUser(Users user)
        {
            int rowsAffected = 0;
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@FirstName", user.FirstName);
                parameters.Add("@LastName", user.LastName);
                parameters.Add("@Email", user.Email);
                parameters.Add("@Gender", user.Gender);
                parameters.Add("@CreatedAt", user.CreatedAt);

                rowsAffected = con.Execute("InsertUser", parameters, commandType: CommandType.StoredProcedure);
            }
            return rowsAffected;
        }

        public static IEnumerable<Users> GetUsers()
        {
            List<Users> usersList = new List<Users>();
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                usersList = con.Query<Users>("GetUserDetail").ToList();
            }
            return usersList;
        }

        public static Users GetUserById(int userId)
        {
            Users user = null;
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                user = con.QuerySingleOrDefault<Users>("GetUserById", new { UserID = userId }, commandType: CommandType.StoredProcedure);
            }
            return user;
        }

        public static int UpdateUser(Users user)
        {
            int rowsAffected = 0;
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserID", user.UserID);
                parameters.Add("@FirstName", user.FirstName);
                parameters.Add("@LastName", user.LastName);
                parameters.Add("@Email", user.Email);
                parameters.Add("@Gender", user.Gender);
                parameters.Add("@CreatedAt", user.CreatedAt);

                rowsAffected = con.Execute("UpdateUser", parameters, commandType: CommandType.StoredProcedure);
            }
            return rowsAffected;
        }

        public static int DeleteUser(int userId)
        {
            int rowsAffected = 0;
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                rowsAffected = con.Execute("DeleteUser", new { UserID = userId }, commandType: CommandType.StoredProcedure);
            }
            return rowsAffected;
        }
    }
}
