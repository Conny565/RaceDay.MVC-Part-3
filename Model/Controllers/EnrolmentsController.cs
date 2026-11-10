using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
namespace testingPOE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrolmentsController : ControllerBase
    {//start of class
        private string GetRole(int userID)
        {
            string role = "";
            try
            {
                string conn = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                SqlConnection c = new SqlConnection(conn); c.Open();
                string q = @"select role from Users where userID=" + userID;
                SqlCommand cmd = new SqlCommand(q, c);
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read()) { role = r["role"].ToString(); }
                c.Close();
            }
            catch { }
            return role;
        }

        [HttpGet]
        public string getAllEnrolments(int userID)
        {
            string role = GetRole(userID);
            if (role != "Organiser" && role != "Organizer")
            {
                return "Access Denied - Only Organiser can view all enrolments";
            }
            string message = "no enrolments";
            try
            {
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                string query = @"select * from Enrolments";
                SqlCommand run_query = new SqlCommand(query, connect);
                SqlDataReader data_found = run_query.ExecuteReader();
                message = "";
                while (data_found.Read())
                {
                    message += "EnrolmentID: " + data_found["enrolmentID"].ToString() + " UserID: " + data_found["userID"].ToString() + "\n";
                }
                connect.Close();
            }
            catch (Exception error) { message = error.Message; }
            return message;
        }

        [HttpPost]
        public string addEnrolment(int userID, string eventID, string categoryID)
        {
            string role = GetRole(userID);
            if (role != "Participant")
            {
                return "Access Denied - Participant functionality must not be accessible to an Organiser";
            }
            string message = "enrolment not added";
            try
            {
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"INSERT INTO Enrolments (enrolmentID, userID, eventID, categoryID) VALUES ((SELECT ISNULL(MAX(enrolmentID),0)+1 FROM Enrolments), '" + userID + "','" + eventID + "','" + categoryID + "')";
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                run_query.ExecuteNonQuery();
                message = "Enrolment Added for User " + userID;
                connect.Close();
            }
            catch (Exception error) { message = error.Message; }
            return message;
        }

        [HttpDelete]
        [Route("deleteEnrolment/{enrolmentID}")]
        public string deleteEnrolment(int userID, int enrolmentID)
        {
            string role = GetRole(userID);
            if (role != "Participant")
            {
                return "Access Denied - Only Participant can cancel";
            }
            string message = "enrolment not deleted";
            try
            {
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"DELETE FROM Enrolments WHERE enrolmentID=" + enrolmentID + " AND userID=" + userID;
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                int rows = run_query.ExecuteNonQuery();
                if (rows > 0) message = "enrolment cancelled";
                connect.Close();
            }
            catch (Exception error) { message = error.Message; }
            return message;
        }
    }
}