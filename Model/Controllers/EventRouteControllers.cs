using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
namespace testingPOE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventRouteController : ControllerBase
    {//start of class

        [HttpGet]
        public string getAllEventRoutes()
        {// start of method
            string message = "no routes found";
            try
            {//start of try
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"select * from EventRoute";
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                SqlDataReader data_found = run_query.ExecuteReader();
                message = "";
                while (data_found.Read())
                {//start of while
                    message += "RouteID: " + data_found["routeID"].ToString() + " EventID: " + data_found["eventID"].ToString() + " RouteName: " + data_found["routeName"].ToString() + " Distance: " + data_found["distance"].ToString() + "\n";
                }//end of while
                if (message == "") message = "no routes found";
                connect.Close();
            }//end of try
            catch (Exception error)
            {//start of catch
                message = error.Message;
            }//end of catch
            return message;
        }//end of method

        [HttpGet]
        [Route("getEventRoute/{routeID}")]
        public string getEventRoute(int routeID)
        {// start of method
            string message = "route not found";
            try
            {//start of try
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"select * from EventRoute where routeID=" + routeID;
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                SqlDataReader data_found = run_query.ExecuteReader();
                while (data_found.Read())
                {//start of while
                    message = "RouteID: " + data_found["routeID"].ToString() + " RouteName: " + data_found["routeName"].ToString();
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
        public string addEventRoute(string eventID, string routeName, string distance, string description)
        {// start of method
            string message = "route not added";
            try
            {//start of try
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"INSERT INTO EventRoute (routeID, eventID, routeName, distance, description) VALUES ((SELECT ISNULL(MAX(routeID),0)+1 FROM EventRoute), '" + eventID + "','" + routeName + "','" + distance + "','" + description + "')";
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                run_query.ExecuteNonQuery();
                message = "Route Added: " + routeName;
                connect.Close();
            }//end of try
            catch (Exception error)
            {//start of catch
                message = error.Message;
            }//end of catch
            return message;
        }//end of method

        [HttpPut]
        [Route("updateEventRoute/{routeID}")]
        public string updateEventRoute(int routeID, string routeName, string distance, string description)
        {// start of method
            string message = "route not updated";
            try
            {//start of try
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"UPDATE EventRoute SET routeName='" + routeName + "', distance='" + distance + "', description='" + description + "' WHERE routeID=" + routeID;
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                int rows = run_query.ExecuteNonQuery();
                if (rows > 0) message = "route updated";
                connect.Close();
            }//end of try
            catch (Exception error)
            {//start of catch
                message = error.Message;
            }//end of catch
            return message;
        }//end of method

        [HttpDelete]
        [Route("deleteEventRoute/{routeID}")]
        public string deleteEventRoute(int routeID)
        {// start of method
            string message = "route not deleted";
            try
            {//start of try
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"DELETE FROM EventRoute WHERE routeID=" + routeID;
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                int rows = run_query.ExecuteNonQuery();
                if (rows > 0) message = "route deleted";
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