using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CourseRegestrationProject
{
    public partial class WebForm5 : System.Web.UI.Page
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
                int courseID = Convert.ToInt32(Request.QueryString["id"]);
                LoadCourseDetails(courseID);
                LoadSchoolDetails(courseID);
                LoadCoursePlan(courseID);
            }
        }

        

        private void LoadCoursePlan(int courseID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"select * from course_plan where course_id = @CourseId";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@CourseId", courseID);

                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    gvCoursePlan.DataSource = dt;
                    gvCoursePlan.DataBind();
                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
            }
        }

        private void LoadSchoolDetails(int courseID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"select school_name from school where id = (select school_id from School_Course_Map where course_id = @CourseId)";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@CourseId", courseID);

                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    School.Text = dt.Rows[0]["school_name"].ToString();

                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
            }
        }

        private void LoadCourseDetails(int courseID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"select * from courses where id = @CourseId";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@CourseId", courseID);

                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    txtCourseCode.Text = dt.Rows[0]["couse_code"].ToString();
                    txtCourseName.Text = dt.Rows[0]["course_name"].ToString();
                    txtCredits.Text = dt.Rows[0]["credits"].ToString();
                    txtDescription.Text = dt.Rows[0]["description"].ToString();
                    
                    
                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
            }
        }

        protected void goDashboard(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "redirectScript",
                "setTimeout(function() { window.location = '/Dashbard.aspx'; }, 0);", true);
        }

        protected void goEditCourse(object sender, EventArgs e)
        {
            int courseID = Convert.ToInt32(Request.QueryString["id"]);
            string url = $"/Edit_course.aspx?id={courseID}";
            Response.Redirect(Page.ResolveClientUrl(url));
        }
    }
}