using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using testingPOE.Model;

namespace testingPOE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {//start of class

        [HttpGet]
        [HttpGet("getAllEvents")]
        public IActionResult getAllEvents()
        {// start of method
            List<events> list = new List<events>();
            try
            {//start of try
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"select * from Events";
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                SqlDataReader data_found = run_query.ExecuteReader();
                while (data_found.Read())
                {//start of while
                    events e = new events();
                    e.eventID = Convert.ToInt32(data_found["eventID"]);
                    e.userID = Convert.ToInt32(data_found["userID"]);
                    e.name = data_found["EventName"].ToString();
                    e.description = data_found["description"].ToString();
                    e.eventDate = data_found["EventDate"].ToString();
                    e.location = data_found["location"].ToString();
                    e.eventType = data_found["eventType"].ToString();
                    e.eventTime = "";
                    e.imagePath = "";
                    list.Add(e);
                }//end of while
                connect.Close();
                return Ok(list);
            }//end of try
            catch (Exception error)
            {//start of catch
                return BadRequest("No Events Found: " + error.Message);
            }//end of catch
        }//end of method

        [HttpGet("{eventID}")]
        [HttpGet("getEvent/{eventID}")]
        public IActionResult getEvent(int eventID)
        {// start of method
            try
            {//start of try
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"select * from Events where eventID = @eventID";
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                run_query.Parameters.AddWithValue("@eventID", eventID);
                SqlDataReader data_found = run_query.ExecuteReader();
                if (data_found.Read())
                {//start of if
                    events e = new events();
                    e.eventID = Convert.ToInt32(data_found["eventID"]);
                    e.userID = Convert.ToInt32(data_found["userID"]);
                    e.name = data_found["EventName"].ToString();
                    e.description = data_found["description"].ToString();
                    e.eventDate = data_found["EventDate"].ToString();
                    e.location = data_found["location"].ToString();
                    e.eventType = data_found["eventType"].ToString();
                    e.eventTime = "";
                    e.imagePath = "";
                    connect.Close();
                    return Ok(e);
                }//end of if
                connect.Close();
                return NotFound("event not found");
            }//end of try
            catch (Exception error)
            {//start of catch
                return BadRequest(error.Message);
            }//end of catch
        }//end of method

        // ---- This one is for your WEBSITE (it sends JSON) ----
        [HttpPost]
        public IActionResult addEvent([FromBody] events ev)
        {// start of method
            try
            {//start of try
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"INSERT INTO Events (eventID, userID, EventName, description, EventDate, location, eventType) VALUES ((SELECT ISNULL(MAX(eventID),0)+1 FROM Events), @userID, @name, @description, @eventDate, @location, @eventType)";
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                run_query.Parameters.AddWithValue("@userID", ev.userID);
                run_query.Parameters.AddWithValue("@name", ev.name);
                run_query.Parameters.AddWithValue("@description", ev.description ?? "");
                run_query.Parameters.AddWithValue("@eventDate", ev.eventDate ?? "");
                run_query.Parameters.AddWithValue("@location", ev.location ?? "");
                run_query.Parameters.AddWithValue("@eventType", ev.eventType ?? "");
                run_query.ExecuteNonQuery();
                connect.Close();
                return Ok("Event Added: " + ev.name);
            }//end of try
            catch (Exception error)
            {//start of catch
                return BadRequest(error.Message);
            }//end of catch
        }//end of method

        // ---- This one is for SWAGGER testing (query string) ----
        [HttpPost("addEvent")]
        public string addEventByQuery(int userID, string name, string description, string eventDate, string location, string eventType)
        {// start of method
            string message = "event not added";
            try
            {//start of try
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"INSERT INTO Events (eventID, userID, EventName, description, EventDate, location, eventType) VALUES ((SELECT ISNULL(MAX(eventID),0)+1 FROM Events), @userID, @name, @description, @eventDate, @location, @eventType)";
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                run_query.Parameters.AddWithValue("@userID", userID);
                run_query.Parameters.AddWithValue("@name", name);
                run_query.Parameters.AddWithValue("@description", description);
                run_query.Parameters.AddWithValue("@eventDate", eventDate);
                run_query.Parameters.AddWithValue("@location", location);
                run_query.Parameters.AddWithValue("@eventType", eventType);
                run_query.ExecuteNonQuery();
                message = "Event Added: " + name;
                connect.Close();
            }//end of try
            catch (Exception error)
            {//start of catch
                message = error.Message;
            }//end of catch
            return message;
        }//end of method

        [HttpPut]
        [Route("updateEvent/{eventID}")]
        public string updateEvent(int eventID, int userID, string name, string description, string eventDate, string location, string eventType)
        {// start of method
            string message = "event not updated";
            try
            {//start of try
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"UPDATE Events SET userID=@userID, EventName=@name, description=@description, EventDate=@eventDate, location=@location, eventType=@eventType WHERE eventID=@eventID";
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                run_query.Parameters.AddWithValue("@userID", userID);
                run_query.Parameters.AddWithValue("@name", name);
                run_query.Parameters.AddWithValue("@description", description);
                run_query.Parameters.AddWithValue("@eventDate", eventDate);
                run_query.Parameters.AddWithValue("@location", location);
                run_query.Parameters.AddWithValue("@eventType", eventType);
                run_query.Parameters.AddWithValue("@eventID", eventID);
                int rows = run_query.ExecuteNonQuery();
                if (rows > 0) message = "event updated";
                connect.Close();
            }//end of try
            catch (Exception error)
            {//start of catch
                message = error.Message;
            }//end of catch
            return message;
        }//end of method

        [HttpDelete]
        [Route("deleteEvent/{eventID}")]
        public string deleteEvent(int eventID)
        {// start of method
            string message = "event not deleted";
            try
            {//start of try
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"DELETE FROM Events WHERE eventID=@eventID";
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                run_query.Parameters.AddWithValue("@eventID", eventID);
                int rows = run_query.ExecuteNonQuery();
                if (rows > 0) message = "event deleted";
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