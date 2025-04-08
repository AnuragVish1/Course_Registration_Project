using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace CourseRegestrationProject
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        
        
        string connectionString = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "(LocalDB)\\MSSQLLocalDB";
            builder.AttachDBFilename = HostingEnvironment.MapPath("~\\App_Data\\Database1.mdf");
            builder.IntegratedSecurity = true;
            connectionString = builder.ToString();
            if (!IsPostBack)
            {
                LoadSemester();
                LoadAllCourse();

            }
           
        }

        // Searching and displaying the courses based on the search data
        protected void LoadeSemCourse(object sender, EventArgs e)
        {
            LoadAllCourse();
        }
        protected void liveSearching(object sender, EventArgs e)
        {
           string searchTerm = txtSearch.Text;
            if (searchTerm == "")
            {
                LoadAllCourse();
                return;
            }
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
            And c.id IN (select course_id from Course_Semester_Map where semester_id = @semester_id)

            ";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                command.Parameters.AddWithValue("@semester_id",ddlSemester.SelectedValue);
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
                    Response.Write("Error: " + ex.Message);
                }
            }
        }
        
        private void LoadSemester()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string queary = @"SELECT id,Sem_Name FROM Semester";
                SqlCommand command = new SqlCommand(queary, conn);
                try
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    ddlSemester.DataSource = dt;
                    Response.Write(dt);
                    ddlSemester.DataTextField = "Sem_Name";
                    ddlSemester.DataValueField = "id";
                    ddlSemester.DataBind();
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
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
            where c.id IN (select course_id from Course_Semester_Map where semester_id = @semester_id)
            

            ";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@semester_id", ddlSemester.SelectedValue);

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