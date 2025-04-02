using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SqlServer.Server;

namespace CourseRegestrationProject
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Anurag\\source\\repos\\CourseRegestrationProject\\App_Data\\Database1.mdf;Integrated Security=True";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                int courseID = Convert.ToInt32(Request.QueryString["id"]);
                if (courseID > 0)
                {

                    LoadFormData(courseID);
                }
            }
            else
            {
                if (ViewState["ScheduleTable"] == null)
                {
                    InitScheduleGrid();
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
            InitScheduleGrid();

            // Load Schedule Plan Table
            LoadCourseSchedule(courseID);

        }

        private void InitScheduleGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Weekday", typeof(string));
            dt.Columns.Add("StartTime", typeof(string));
            dt.Columns.Add("EndTime", typeof(string));
            dt.Columns.Add("RoomNumber", typeof(string));
            dt.Columns.Add("FacultyMember", typeof(string));

            ViewState["ScheduleTable"] = dt;
            gvSchedule.DataSource = dt;
            gvSchedule.DataBind();
        }

        private void LoadCourseSchedule(int courseID)
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT id, schedule_weekday as Weekday, start_time as StartTime, end_time as EndTime,  [Room No.] as RoomNumber 
            FROM schedule
            WHERE course_id = @CourseID";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@CourseID", courseID);
                try
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    gvSchedule.DataSource = dt;
                    gvSchedule.DataBind();
                    ViewState["ScheduleTable"] = dt;
                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
            }
        }

        protected void gvSchedule_DataBound(object sender, EventArgs e)
        {
            InitRoomNumber();
            InitFacultyName();
        }

        private void InitFacultyName()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"select id, faculty_name from Faculty";
                SqlCommand command = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Get the current data from ViewState to preserve selections
                    DataTable scheduleDt = ViewState["ScheduleTable"] as DataTable;

                    foreach (GridViewRow row in gvSchedule.Rows)
                    {
                        DropDownList ddlFacultyMember = (DropDownList)row.FindControl("ddlFacultyMember");
                        if (ddlFacultyMember != null)
                        {
                            // Store the index to access the correct row in the DataTable
                            int rowIndex = row.RowIndex;
                            string currentValue = "";

                            // Get the saved value from the DataTable for this row if it exists
                            if (rowIndex < scheduleDt.Rows.Count &&
                                scheduleDt.Rows[rowIndex]["FacultyMember"] != DBNull.Value)
                            {
                                currentValue = scheduleDt.Rows[rowIndex]["FacultyMember"].ToString();
                            }

                            ddlFacultyMember.Items.Clear();
                            ddlFacultyMember.Items.Add(new ListItem("-- Select Faculty --", ""));
                            ddlFacultyMember.DataSource = dt;
                            ddlFacultyMember.DataTextField = "faculty_name";
                            ddlFacultyMember.DataValueField = "id";
                            ddlFacultyMember.SelectedIndex = GetFacultyNumber(rowIndex, row);
                            ddlFacultyMember.DataBind();

                            if (!string.IsNullOrEmpty(currentValue))
                            {
                                ListItem item = ddlFacultyMember.Items.FindByValue(currentValue);
                                if (item != null)
                                {
                                    item.Selected = true;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }
        }

        private int GetFacultyNumber(int rowIndex, GridViewRow row)
        {
            int facultyIndex = 0;
            int courseID = Convert.ToInt32(Request.QueryString["id"]);
            DropDownList ddlWeekDay = (DropDownList)row.FindControl("ddlWeekday");
            int weekday = ddlWeekDay.SelectedIndex;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"select faculty_id from Faculty_Schedule_Map where schedule_id = (select id from schedule where schedule_weekday = @weekday and course_id = @Course_ID)";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@weekday", weekday);
                command.Parameters.AddWithValue("@Course_ID", courseID);

                try
                {
                    conn.Open();
                    facultyIndex = Convert.ToInt32(command.ExecuteScalar());


                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }

            }
            return facultyIndex - 1;

        }

        // Loading the room numbers from the database
        private void InitRoomNumber()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"select id, Room_No from Room";
                SqlCommand command = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    DataTable scheduleDt = ViewState["ScheduleTable"] as DataTable;

                    if (scheduleDt == null)
                    {
                        Response.Write("dt is empty");
                    }

                    System.Diagnostics.Debug.WriteLine("Room data rows: " + dt.Rows.Count);

                    foreach (GridViewRow row in gvSchedule.Rows)
                    {
                        DropDownList ddlRoomNumber = (DropDownList)row.FindControl("ddlRoomNumber");
                        if (ddlRoomNumber != null)
                        {
                            int rowIndex = row.RowIndex;
                            string currentValue = "";

                            // Get the saved value from the DataTable for this row if it exists
                            if (rowIndex < scheduleDt.Rows.Count &&
                                scheduleDt.Rows[rowIndex]["RoomNumber"] != DBNull.Value)
                            {
                                currentValue = scheduleDt.Rows[rowIndex]["RoomNumber"].ToString();
                            }




                            ddlRoomNumber.Items.Clear();

                            ddlRoomNumber.Items.Add(new ListItem("-- Select Room --", ""));

                            ddlRoomNumber.DataSource = dt;
                            ddlRoomNumber.DataTextField = "Room_No";
                            ddlRoomNumber.DataValueField = "id";
                            ddlRoomNumber.SelectedIndex = GetRoomNumber(rowIndex, row);
                            ddlRoomNumber.DataBind();

                            if (!string.IsNullOrEmpty(currentValue))
                            {
                                ListItem item = ddlRoomNumber.Items.FindByValue(currentValue);
                                if (item != null)
                                {
                                    item.Selected = true;
                                }
                            }
                        }
                    }


                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error in InitRoomNumber: " + ex.Message);
                    Response.Write("Error loading rooms: " + ex.Message);
                }
            }
        }

        private int GetRoomNumber(int rowIndex, GridViewRow row)
        {
            int room_number = 0;
            int courseID = Convert.ToInt32(Request.QueryString["id"]);
            DropDownList ddlWeekDay = (DropDownList)row.FindControl("ddlWeekday");
            int weekday = ddlWeekDay.SelectedIndex;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"select [Room No.] from schedule where schedule_weekday = @weekday and course_id = @Course_ID";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@weekday", weekday);
                command.Parameters.AddWithValue("@Course_ID", courseID);

                try
                {
                    conn.Open();
                    room_number = Convert.ToInt32(command.ExecuteScalar());


                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
            }
            return room_number;
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
        protected void GvSchedule_Delete(object sender, GridViewDeleteEventArgs e)
        {

            // getting selected row
            GridViewRow row = gvSchedule.Rows[e.RowIndex];
            int courseID = Convert.ToInt32(Request.QueryString["id"]);
            DropDownList ddlWeekDay = (DropDownList)row.FindControl("ddlWeekday");
            int weekday = ddlWeekDay.SelectedIndex;

            //Establishing connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"delete from schedule where schedule_weekday = @weekday and course_id = @Course_Id";
                SqlCommand commnad = new SqlCommand(query, conn);
                commnad.Parameters.AddWithValue("@weekday", weekday);
                commnad.Parameters.AddWithValue("@Course_Id", courseID);

                try
                {
                    //Executing the query
                    conn.Open();
                    commnad.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
            }

        }
        protected void AddCourseScheduleBtn_Click(object sender, EventArgs e)
        {

            DataTable dt = ViewState["ScheduleTable"] as DataTable;

            for (int i = 0; i < gvSchedule.Rows.Count; i++)
            {
                GridViewRow row = gvSchedule.Rows[i];

                if (i < dt.Rows.Count)
                {
                    dt.Rows[i]["Weekday"] = ((DropDownList)row.FindControl("ddlWeekday")).SelectedValue;
                    dt.Rows[i]["StartTime"] = ((TextBox)row.FindControl("txtStartTime")).Text;
                    dt.Rows[i]["EndTime"] = ((TextBox)row.FindControl("txtEndTime")).Text;
                    dt.Rows[i]["RoomNumber"] = ((DropDownList)row.FindControl("ddlRoomNumber")).SelectedValue;
                    dt.Rows[i]["FacultyMember"] = ((DropDownList)row.FindControl("ddlFacultyMember")).SelectedValue;

                }
            }

            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);
            
            gvSchedule.DataSource = dt;
            gvSchedule.DataBind();
            ViewState["ScheduleTable"] = dt;
        }
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