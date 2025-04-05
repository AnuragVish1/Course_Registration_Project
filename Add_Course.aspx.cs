using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CourseRegestrationProject
{
    public partial class WebForm6 : System.Web.UI.Page
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
                LoadAvailableCourses();
            }
        }


        protected void gvAvailableCourses_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                // getting the row index
                string courseCode= (e.CommandArgument).ToString();
                // getting the course id
                int courseId = getCourseId(courseCode);
                // finally, adding it to the course semeste map table
                AddCourseToSemester(courseId);
                LoadAvailableCourses();

            }
               
        }

        private int getCourseId(string courseCode)
        {
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string queay = @"SELECT id from courses where couse_code = @Course_Code";
                SqlCommand command = new SqlCommand(queay, conn);
                command.Parameters.AddWithValue("@Course_Code", courseCode);
                try
                {
                    conn.Open();
                    return Convert.ToInt32(command.ExecuteScalar().ToString());
                }
                catch (Exception e)
                {
                    Response.Write(e.ToString());
                    return 0;
                }
            }
        }

        private void AddCourseToSemester(int courseId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string queary = @"INSERT INTO Course_Semester_Map (course_id, semester_id) VALUES (@CourseId, @SemesterId)";
                SqlCommand command = new SqlCommand(queary, conn);
                command.Parameters.AddWithValue("@CourseId", courseId);
                command.Parameters.AddWithValue("@SemesterId", ddlSemester.SelectedValue);
                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    Response.Write(e.ToString());
                }
            }
        }

        protected void liveSearching(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text;
            if (searchTerm == "")
            {
                LoadAvailableCourses();
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
                SELECT c.couse_code as couse_code, c.course_name as course_name, c.credits as credits, s.short as short
                FROM courses c
                LEFT JOIN School_Course_Map scm ON c.id = scm.course_id
                LEFT JOIN School s ON scm.School_id = s.id
                WHERE c.course_name LIKE @SearchTerm OR c.couse_code LIKE @SearchTerm
                AND
                c.id NOT IN (select course_id from Course_Semester_Map where semester_id = @Semesterid)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                command.Parameters.AddWithValue("@Semesterid", ddlSemester.SelectedValue);
                try
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    gvAvailableCourses.DataSource = dt;
                    gvAvailableCourses.DataBind();
                }
                catch (Exception ex)
                {
                    Response.Write("Error: " + ex.Message);
                }
            }
        }

        protected void selectedSemCourses(object sender, EventArgs e)
        {
            LoadAvailableCourses();
        }
        private void LoadAvailableCourses()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string queary = @"SELECT c.couse_code as couse_code, c.course_name as course_name, c.credits as credits, s.short as short from courses c
                LEFT JOIN School_Course_Map scm ON c.id = scm.course_id
                LEFT JOIN School s ON scm.School_id = s.id
                where c.id NOT IN (select course_id from Course_Semester_Map where semester_id = @Semesterid)";
                SqlCommand command = new SqlCommand(queary, connection);
                command.Parameters.AddWithValue("@Semesterid", ddlSemester.SelectedValue);
                try
                {
                    SqlDataAdapter adpter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adpter.Fill(dt);
                    gvAvailableCourses.DataSource = dt;
                    gvAvailableCourses.DataBind();
                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
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
    }
}