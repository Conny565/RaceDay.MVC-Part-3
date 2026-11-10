using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
namespace testingPOE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {//start of class

        [HttpGet]
        public string getAllCategories()
        {// start of method
            string message = "no categories found";
            try
            {//start of try
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"select * from Categories";
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                SqlDataReader data_found = run_query.ExecuteReader();
                message = "";
                while (data_found.Read())
                {//start of while
                    message += "CategoryID: " + data_found["categoryID"].ToString() + " EventID: " + data_found["eventID"].ToString() + " Name: " + data_found["categoryName"].ToString() + " Distance: " + data_found["distance"].ToString() + " AgeGroup: " + data_found["ageGroup"].ToString() + "\n";
                }//end of while
                if (message == "") message = "no categories found";
                connect.Close();
            }//end of try
            catch (Exception error)
            {//start of catch
                message = error.Message;
            }//end of catch
            return message;
        }//end of method

        [HttpGet]
        [Route("getCategory/{categoryID}")]
        public string getCategory(int categoryID)
        {// start of method
            string message = "category not found";
            try
            {//start of try
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"select * from Categories where categoryID=" + categoryID;
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                SqlDataReader data_found = run_query.ExecuteReader();
                while (data_found.Read())
                {//start of while
                    message = "CategoryID: " + data_found["categoryID"].ToString() + " Name: " + data_found["categoryName"].ToString() + " Distance: " + data_found["distance"].ToString() + " AgeGroup: " + data_found["ageGroup"].ToString();
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
        public string addCategory(string eventID, string categoryName, string distance, string ageGroup)
        {// start of method
            string message = "category not added";
            try
            {//start of try
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"INSERT INTO Categories (categoryID, eventID, categoryName, distance, ageGroup) VALUES ((SELECT ISNULL(MAX(categoryID),0)+1 FROM Categories), '" + eventID + "','" + categoryName + "','" + distance + "','" + ageGroup + "')";
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                run_query.ExecuteNonQuery();
                message = "Category Added: " + categoryName;
                connect.Close();
            }//end of try
            catch (Exception error)
            {//start of catch
                message = error.Message;
            }//end of catch
            return message;
        }//end of method

        [HttpPut]
        [Route("updateCategory/{categoryID}")]
        public string updateCategory(int categoryID, string categoryName, string distance, string ageGroup)
        {// start of method
            string message = "category not updated";
            try
            {//start of try
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"UPDATE Categories SET categoryName='" + categoryName + "', distance='" + distance + "', ageGroup='" + ageGroup + "' WHERE categoryID=" + categoryID;
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                int rows = run_query.ExecuteNonQuery();
                if (rows > 0) message = "category updated";
                connect.Close();
            }//end of try
            catch (Exception error)
            {//start of catch
                message = error.Message;
            }//end of catch
            return message;
        }//end of method

        [HttpDelete]
        [Route("deleteCategory/{categoryID}")]
        public string deleteCategory(int categoryID)
        {// start of method
            string message = "category not deleted";
            try
            {//start of try
                string string_connection = @"Data Source=(localdb)\api_endpoint;Initial Catalog=RacingDays;Integrated Security=True;";
                string query = @"DELETE FROM Categories WHERE categoryID=" + categoryID;
                SqlConnection connect = new SqlConnection(string_connection);
                connect.Open();
                SqlCommand run_query = new SqlCommand(query, connect);
                int rows = run_query.ExecuteNonQuery();
                if (rows > 0) message = "category deleted";
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