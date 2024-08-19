using Dapper;
using GroupMeetingASP.NETCoreWebApp.Models;
using System.Data;
using System.Data.SqlClient;


namespace GroupMeetingASP.NETCoreWebApp.Models
{
   
    
        public class RegisterUser
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }

        private static string strConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=ProjectMeeting;Trusted_Connection=True;Integrated Security=SSPI;";

        public static bool EmailExists(string email)
        {
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var existingUser = con.QueryFirstOrDefault<int>("SELECT COUNT(1) FROM RegisterUser WHERE email = @Email", new { Email = email });
                return existingUser > 0;
            }
        }

        public static int AddUser(RegisterUser user)
        {
            int rowsAffected = 0;
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@userName", user.UserName);
                parameters.Add("@email", user.Email);
                parameters.Add("@password", user.Password);
                parameters.Add("@confirmpassword", user.ConfirmPassword);

                rowsAffected = con.Execute("signup", parameters, commandType: CommandType.StoredProcedure);
            }
            return rowsAffected;
        }
        public static RegisterUser Login(string email, string password)
        {
            RegisterUser user = null;
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Email", email);
                parameters.Add("@Password", password);

                user = con.QueryFirstOrDefault<RegisterUser>("Login", parameters, commandType: CommandType.StoredProcedure);
            }
            return user;
        }
    }

}

        
    


