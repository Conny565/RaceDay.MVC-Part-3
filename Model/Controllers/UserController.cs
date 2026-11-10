using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
namespace testingPOE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {//start of class

        [HttpGet]
        public string getAllUsers()
        {// start of method
            string message = "no users found";
            try
            {//start of try
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"select * from Users";
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                SqlDataReader data_found = run_query.ExecuteReader();
                message = "";
                while (data_found.Read())
                {//start of while
                    message += "UserID: " + data_found["userID"].ToString() + " Name: " + data_found["fullName"].ToString() + " Email: " + data_found["email"].ToString() + " Role: " + data_found["role"].ToString() + "\n";
                }//end of while
                if (message == "") message = "no users found";
                connect.Close();
            }//end of try
            catch (Exception error)
            {//start of catch
                message = error.Message;
            }//end of catch
            return message;
        }//end of method

        [HttpGet]
        [Route("getUser/{userID}")]
        public string getUser(int userID)
        {// start of method
            string message = "user not found";
            try
            {//start of try
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"select * from Users where userID=" + userID;
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                SqlDataReader data_found = run_query.ExecuteReader();
                while (data_found.Read())
                {//start of while
                    message = "UserID: " + data_found["userID"].ToString() + " Name: " + data_found["fullName"].ToString() + " Email: " + data_found["email"].ToString() + " Role: " + data_found["role"].ToString();
                }//end of while
                connect.Close();
            }//end of try
            catch (Exception error)
            {//start of catch
                message = error.Message;
            }//end of catch
            return message;
        }//end of method

        [HttpPost]
        public string addUser(string fullName, string email, string password, string role)
        {// start of method - REGISTER
            string message = "user not added";
            try
            {//start of try
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"INSERT INTO Users (userID, fullName, email, password, role) VALUES ((SELECT ISNULL(MAX(userID),0)+1 FROM Users), '" + fullName + "','" + email + "','" + password + "','" + role + "')";
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                run_query.ExecuteNonQuery();
                message = "User Added: " + fullName + " Role: " + role;
                connect.Close();
            }//end of try
            catch (Exception error)
            {//start of catch
                message = error.Message;
            }//end of catch
            return message;
        }//end of method

        [HttpGet]
        [Route("loginUser/{email}/{password}")]
        public string loginUser(string email, string password)
        {// start of method - LOGIN
            string message = "login failed - invalid email or password";
            try
            {//start of try
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"select * from Users where email='" + email + "' AND password='" + password + "'";
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                SqlDataReader data_found = run_query.ExecuteReader();
                while (data_found.Read())
                {//start of while
                    message = "Login successefull UserID: " + data_found["userID"].ToString() + " Name: " + data_found["fullName"].ToString() + " Role: " + data_found["role"].ToString();
                }//end of while
                connect.Close();
            }//end of try
            catch (Exception error)
            {//start of catch
                message = error.Message;
            }//end of catch
            return message;
        }//end of method

        [HttpPut]
        [Route("updateUser/{userID}")]
        public string updateUser(int userID, string fullName, string email, string password, string role)
        {// start of method - UPDATE PROFILE
            string message = "user not updated";
            try
            {//start of try
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"UPDATE Users SET fullName='" + fullName + "', email='" + email + "', password='" + password + "', role='" + role + "' WHERE userID=" + userID;
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                int rows = run_query.ExecuteNonQuery();
                if (rows > 0) message = "user updated";
                connect.Close();
            }//end of try
            catch (Exception error)
            {//start of catch
                message = error.Message;
            }//end of catch
            return message;
        }//end of method

        [HttpDelete]
        [Route("deleteUser/{userID}")]
        public string deleteUser(int userID)
        {// start of method
            string message = "user not deleted";
            try
            {//start of try
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"DELETE FROM Users WHERE userID=" + userID;
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                int rows = run_query.ExecuteNonQuery();
                if (rows > 0) message = "user deleted";
                connect.Close();
            }//end of try
            catch (Exception error)
            {//start of catch
                message = error.Message;
            }//end of catch
            return message;
        }//end of method

    }//end of class
}//end of namespace
