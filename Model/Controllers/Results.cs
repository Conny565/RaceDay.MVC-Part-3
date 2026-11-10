using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
namespace testingPOE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private string GetRole(int userID)
        {
            string role = "";
            try
            {
                string conn = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                SqlConnection c = new SqlConnection(conn);
                c.Open();
                string q = @"select role from users where userID=" + userID;
                SqlCommand cmd = new SqlCommand(q, c);
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read()) { role = r["role"].ToString().Trim().ToLower(); }
                c.Close();
            }
            catch { }
            return role;
        }

        [HttpGet]
        public string getAllResults(int userID)
        {
            string role = GetRole(userID);
            if (role == "")
            {
                return "Error: userID " + userID + " not found in users table";
            }
            if (role != "organiser" && role != "organizer")
            {
                return "Access Denied - Only Organiser can view all results. Your role is: " + role;
            }

            string message = "no results found";
            try
            {
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                string query = @"select * from Results";
                SqlCommand run_query = new SqlCommand(query, connect);
                SqlDataReader data_found = run_query.ExecuteReader();
                message = "";
                while (data_found.Read())
                {
                    message += "ID: " + data_found["resultID"].ToString() + " Enrolment: " + data_found["enrolmentID"].ToString() + " Time: " + data_found["finishTime"].ToString() + " Position: " + data_found["position"].ToString() + "\n";
                }
                if (message == "") message = "no results found";
                connect.Close();
            }
            catch (Exception error) { message = error.Message; }
            return message;
        }

        [HttpGet]
        [Route("getMyResult")]
        public string getMyResult(int userID)
        {
            string message = "no result for you";
            try
            {
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                string query = @"select * from Results where enrolmentID IN (select enrolmentID from Enrolments where userID=" + userID + ")";
                SqlCommand run_query = new SqlCommand(query, connect);
                SqlDataReader data_found = run_query.ExecuteReader();
                message = "";
                while (data_found.Read())
                {
                    message += "ResultID: " + data_found["resultID"].ToString() + " Time: " + data_found["finishTime"].ToString() + " Position: " + data_found["position"].ToString() + "\n";
                }
                if (message == "") message = "no result for you";
                connect.Close();
            }
            catch (Exception error) { message = error.Message; }
            return message;
        }

        [HttpPost]
        public string addResult(int userID, int enrolmentID, string finishTime, string position)
        {
            string role = GetRole(userID);
            if (role != "organiser" && role != "organizer")
            {
                return "Access Denied - Organiser functionality must not be accessible to a Participant. Your role is: " + role;
            }
            string message = "result not added";
            try
            {
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"INSERT INTO Results (resultID, enrolmentID, finishTime, position) VALUES ((SELECT ISNULL(MAX(resultID),0)+1 FROM Results), '" + enrolmentID + "','" + finishTime + "','" + position + "')";
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                run_query.ExecuteNonQuery();
                message = "Result Added for enrolment " + enrolmentID;
                connect.Close();
            }
            catch (Exception error) { message = error.Message; }
            return message;
        }

        [HttpPut]
        [Route("updateResult/{resultID}")]
        public string updateResult(int userID, int resultID, string finishTime, string position)
        {
            string role = GetRole(userID);
            if (role != "organiser" && role != "organizer")
            {
                return "Access Denied - Only Organiser can update results";
            }
            string message = "result not updated";
            try
            {
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"UPDATE Results SET finishTime='" + finishTime + "', position='" + position + "' WHERE resultID=" + resultID;
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                int rows = run_query.ExecuteNonQuery();
                if (rows > 0) message = "result updated";
                connect.Close();
            }
            catch (Exception error) { message = error.Message; }
            return message;
        }

        [HttpDelete]
        [Route("deleteResult/{resultID}")]
        public string deleteResult(int userID, int resultID)
        {
            string role = GetRole(userID);
            if (role != "organiser" && role != "organizer")
            {
                return "Access Denied - Only Organiser can delete results";
            }
            string message = "result not deleted";
            try
            {
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"DELETE FROM Results WHERE resultID=" + resultID;
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                int rows = run_query.ExecuteNonQuery();
                if (rows > 0) message = "result deleted";
                connect.Close();
            }
            catch (Exception error) { message = error.Message; }
            return message;
        }
    }
}