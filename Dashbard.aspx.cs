using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CourseRegestrationProject
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CourseData();
            }
        }

        private void CourseData()
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Anurag\\source\\repos\\CourseRegestrationProject\\App_Data\\Database1.mdf;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT c.id, c.couse_code as CourseCode, c.course_name as CourseName, c.credits as Credits,
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
                }
                catch (Exception ex)
                {
                    // Handle errors
                    Response.Write("Error: " + ex.Message);
                }
            }
        }
    }
}