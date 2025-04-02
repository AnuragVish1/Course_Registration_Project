using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace CourseRegestrationProject
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Anurag\\source\\repos\\CourseRegestrationProject\\App_Data\\Database1.mdf;Integrated Security=True";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAllCourse();
            }
           
        }

        // Searching and displaying the courses based on the search data
        protected void SearchForCourse(object sender, EventArgs e)
        {
            
        }

        protected void liveSearching(object sender, EventArgs e)
        {
           string searchTerm = txtSearch.Text;
            System.Diagnostics.Debug.WriteLine(searchTerm);
            SearchCourses(searchTerm);
        }
        private void SearchCourses(string searchTerm)
        {
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT c.id as CourseID, c.couse_code as CourseCode, c.course_name as CourseName, c.credits as Credits,
                   s.school_name AS SchoolName
            FROM courses c
            LEFT JOIN School_Course_Map scm ON c.id = scm.course_id
            LEFT JOIN School s ON scm.School_id = s.id
            WHERE c.course_name LIKE @SearchTerm OR c.couse_code LIKE @SearchTerm

            ";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                try
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    CourseRepeater.DataSource = dt;
                    CourseRepeater.DataBind();
                    Response.Write(dt.ToString());
                }
                catch (Exception ex)
                {
                    Response.Write("Error: " + ex.Message);
                }
            }
        }
        private void LoadAllCourse()
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT c.id as CourseID, c.couse_code as CourseCode, c.course_name as CourseName, c.credits as Credits,
                   s.school_name AS SchoolName
            FROM courses c
            LEFT JOIN School_Course_Map scm ON c.id = scm.course_id
            LEFT JOIN School s ON scm.School_id = s.id
            

            ";

                SqlCommand command = new SqlCommand(query, connection);
                

                try
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    CourseRepeater.DataSource = dt;
                    CourseRepeater.DataBind();
                    Response.Write(dt.ToString());
                }
                catch (Exception ex)
                {
                    Response.Write("Error: " + ex.Message);
                }
            }
        }
    }
}