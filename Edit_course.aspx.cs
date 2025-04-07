using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SqlServer.Server;

namespace CourseRegestrationProject
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        string connectionString = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "(LocalDB)\\MSSQLLocalDB";
            builder.AttachDBFilename = HostingEnvironment.MapPath("~\\App_Data\\Database1.mdf");
            builder.IntegratedSecurity = true;
            connectionString = builder.ToString();
            int courseID = Convert.ToInt32(Request.QueryString["id"]);

            if (!IsPostBack)
            {

                if (courseID > 0)
                {

                    LoadFormData(courseID);
                }
            }
        }

        private void LoadFormData(int courseID)
        {
            // Load Course Details
            LoadCourseDetails(courseID);
            LoadSchoolList(courseID);

            // Load Course Plan Table
            LoadCoursePlanTable(courseID);


        }



        private void LoadCoursePlanTable(int courseID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"select * from course_plan where course_id = @CourseId";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@CourseId", courseID);
                try
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    gvCoursePlan.DataSource = dt;
                    gvCoursePlan.DataBind();
                    ViewState["CoursePlanTable"] = dt;

                }

                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }

            }
        }

        // for course plan delete
        protected void GvCoursePlan_Delete(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dt = ViewState["CoursePlanTable"] as DataTable;
            int courseID = Convert.ToInt32(Request.QueryString["id"]);

            int sessionNumber = Convert.ToInt32(dt.Rows[e.RowIndex]["session_number"]);


            dt.Rows.RemoveAt(e.RowIndex);
            gvCoursePlan.DataSource = dt;
            gvCoursePlan.DataBind();
            ViewState["CoursePlanTable"] = dt;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"delete from course_plan where session_number = @session_number and course_id = @Course_Id";
                SqlCommand commnad = new SqlCommand(query, conn);
                commnad.Parameters.AddWithValue("@session_number", sessionNumber);
                commnad.Parameters.AddWithValue("@Course_Id", courseID);

                try
                {
                    conn.Open();
                    commnad.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Response.Write(ex);
                }


            }

        }

        // for on delete schedule row


        protected void AddCoursePlanBtn_Click(object sender, EventArgs e)
        {

            DataTable dt = ViewState["CoursePlanTable"] as DataTable;

            for (int i = 0; i < gvCoursePlan.Rows.Count; i++)
            {
                GridViewRow row = gvCoursePlan.Rows[i];

                if (i < dt.Rows.Count)
                {
                    dt.Rows[i]["session_number"] = ((TextBox)row.FindControl("txtSessionNumber")).Text;
                    dt.Rows[i]["Topic"] = ((TextBox)row.FindControl("txtTopic")).Text;
                    dt.Rows[i]["Subtopic"] = ((TextBox)row.FindControl("txtSubtopic")).Text;
                    dt.Rows[i]["reading_materials"] = ((TextBox)row.FindControl("txtReadingMaterial")).Text;
                    dt.Rows[i]["Activity"] = ((TextBox)row.FindControl("txtActivity")).Text;
                    dt.Rows[i]["Important_dates"] = ((TextBox)row.FindControl("txtImportantDates")).Text;

                }
            }

            DataRow dr = dt.NewRow();
            dr["session_number"] = dt.Rows.Count + 1;
            dt.Rows.Add(dr);
            gvCoursePlan.DataSource = dt;
            gvCoursePlan.DataBind();
            ViewState["CoursePlanTable"] = dt;

        }

        private void LoadSchoolList(int courseID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string queary = @"SELECT id,school_name FROM School";
                SqlCommand command = new SqlCommand(queary, conn);
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    ddlSchool.DataSource = dt;
                    Response.Write(dt);
                    ddlSchool.DataTextField = "school_name";
                    ddlSchool.DataValueField = "id";
                    ddlSchool.DataBind();

                    string query2 = @"select id, school_name from School where id = (select school_id from School_Course_Map where course_id = @CourseId)";
                    SqlCommand command1 = new SqlCommand(query2, conn);
                    command1.Parameters.AddWithValue("@CourseId", courseID);
                    try
                    {
                        SqlDataAdapter adapter1 = new SqlDataAdapter(command1);
                        DataTable dt2 = new DataTable();
                        adapter1.Fill(dt2);
                        ddlSchool.SelectedIndex = Convert.ToInt32(dt2.Rows[0]["id"].ToString()) - 1;

                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.ToString());
                    }


                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
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

        protected void EditCourseBtn(object sender, EventArgs e)
        {
            //Validation
            if (!Page.IsValid)
            {
                return;
            }
            int courseID = Convert.ToInt32(Request.QueryString["id"]);

            string course_code = txtCourseCode.Text;
            string course_name = txtCourseName.Text;
            int course_credit = Convert.ToInt32(txtCredits.Text.Trim());
            string description = txtDescription.Text;
            int School_Id = Convert.ToInt32(ddlSchool.SelectedValue);

            // editing the course details
            EditCourseDetails(course_code, course_name, course_credit, description, courseID);

            // editing School Course detail
            EditSchoolCourse(School_Id, courseID);

            // editing Course Plan
            EditCoursePlan(courseID);

            // Redirecting to the dashboard
            ClientScript.RegisterStartupScript(this.GetType(), "redirectScript",
                "setTimeout(function() { window.location = '/Dashbard.aspx'; }, 1000);", true);
        }

        private void EditCoursePlan(int courseID)
        {
            DataTable dt = ViewState["CoursePlanTable"] as DataTable;
            for (int i = 0; i < gvCoursePlan.Rows.Count; i++)
            {
                GridViewRow row = gvCoursePlan.Rows[i];

                if (i < dt.Rows.Count)
                {
                    dt.Rows[i]["session_number"] = ((TextBox)row.FindControl("txtSessionNumber")).Text;
                    dt.Rows[i]["Topic"] = ((TextBox)row.FindControl("txtTopic")).Text;
                    dt.Rows[i]["Subtopic"] = ((TextBox)row.FindControl("txtSubtopic")).Text;
                    dt.Rows[i]["reading_materials"] = ((TextBox)row.FindControl("txtReadingMaterial")).Text;
                    dt.Rows[i]["Activity"] = ((TextBox)row.FindControl("txtActivity")).Text;
                    dt.Rows[i]["Important_dates"] = ((TextBox)row.FindControl("txtImportantDates")).Text;

                }
            }

            gvCoursePlan.DataSource = dt;
            gvCoursePlan.DataBind();
            ViewState["CoursePlanTable"] = dt;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"delete from course_plan where course_id = @CourseId";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@CourseId", courseID);
                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                foreach (DataRow row in dt.Rows)
                {
                    string query = @"INSERT INTO course_plan (topic, session_number, subtopic, reading_materials, 
                             activity, important_dates, course_id) 
                             VALUES (@Topic, @SessionNumber, @Subtopic, @ReadingMaterials, 
                             @Activity, @ImportantDates, @CourseId)";

                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@Topic", row["Topic"]);
                    command.Parameters.AddWithValue("@SessionNumber", row["session_number"]);
                    command.Parameters.AddWithValue("@Subtopic", row["Subtopic"]);
                    command.Parameters.AddWithValue("@ReadingMaterials", row["reading_materials"]);
                    command.Parameters.AddWithValue("@Activity", row["Activity"]);
                    command.Parameters.AddWithValue("@ImportantDates", row["important_dates"]);
                    command.Parameters.AddWithValue("@CourseId", courseID);

                    try
                    {
                        command.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {
                        Response.Write(ex.ToString());
                    }
                }
            }
        }

        private void EditSchoolCourse(int school_Id, object courseID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"update School_Course_Map set [School_id] = @SchoolId 
                         where [course_id] = @CourseId";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@SchoolId", school_Id);
                command.Parameters.AddWithValue("@CourseId", courseID);

                try
                {


                    conn.Open();
                    command.ExecuteNonQuery();
                }

                catch (Exception ex)
                {
                    Response.Write(ex);
                }
            }
        }

        private void EditCourseDetails(string course_code, string course_name, int course_credit, string description, int courseID)
        {


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"update courses set [couse_code] = @CourseCode, [course_name] = @CourseName, [credits] = @CourseCredit, [description] = @CourseDescription where id = @CourseId";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@CourseCode", course_code);
                command.Parameters.AddWithValue("@CourseName", course_name);
                command.Parameters.AddWithValue("@CourseCredit", course_credit);
                command.Parameters.AddWithValue("@CourseDescription", description);
                command.Parameters.AddWithValue("@CourseId", courseID);

                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
            }
        }
    }
}