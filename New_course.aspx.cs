using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CourseRegestrationProject
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Anurag\\source\\repos\\CourseRegestrationProject\\App_Data\\Database1.mdf;Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSchools();
                InitCoursePlanGrid();
                InitScheduleGrid();
                InitRoomNumber();
            }
            else
            {
                if (ViewState["CoursePlanTable"] == null)
                {
                    InitCoursePlanGrid();
                }
                if (ViewState["ScheduleTable"] == null)
                {
                    InitScheduleGrid();
                }
            }
        }


        // Initializing the Course plan gird
        private void InitCoursePlanGrid()
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("SessionNumber", typeof(int));

            dt.Columns.Add("Topic", typeof(string));
            dt.Columns.Add("Subtopic", typeof(string));

            dt.Columns.Add("ReadingMaterial", typeof(string));
            dt.Columns.Add("Activity", typeof(string));

            dt.Columns.Add("ImportantDates", typeof(DateTime));

            ViewState["CoursePlanTable"] = dt;
            gvCoursePlan.DataSource = dt;
            gvCoursePlan.DataBind();
        }

        // Initializing the Course Schedule gird

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

        //Function for adding new row to the course plan table
        protected void AddCoursePlanBtn_Click(object sender, EventArgs e)
        {

            DataTable dt = ViewState["CoursePlanTable"] as DataTable;

            for (int i = 0; i < gvCoursePlan.Rows.Count; i++)
            {
                GridViewRow row = gvCoursePlan.Rows[i];

                if (i < dt.Rows.Count)
                {
                    dt.Rows[i]["SessionNumber"] = ((TextBox)row.FindControl("txtSessionNumber")).Text;
                    dt.Rows[i]["Topic"] = ((TextBox)row.FindControl("txtTopic")).Text;
                    dt.Rows[i]["Subtopic"] = ((TextBox)row.FindControl("txtSubtopic")).Text;
                    dt.Rows[i]["ReadingMaterial"] = ((TextBox)row.FindControl("txtReadingMaterial")).Text;
                    dt.Rows[i]["Activity"] = ((TextBox)row.FindControl("txtActivity")).Text;
                    DateTime importantDate;
                    if (DateTime.TryParse(((TextBox)row.FindControl("txtImportantDates")).Text, out importantDate))
                        dt.Rows[i]["ImportantDates"] = importantDate;

                }
            }

            DataRow dr = dt.NewRow();
            dr["SessionNumber"] = dt.Rows.Count + 1;
            dt.Rows.Add(dr);
            ViewState["CoursePlanTable"] = dt;
            gvCoursePlan.DataSource = dt;
            gvCoursePlan.DataBind();
        }

        // Function for add new row to the course schedule table
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
            ViewState["ScheduleTable"] = dt;
            gvSchedule.DataSource = dt;
            gvSchedule.DataBind();
        }

        // For on delete course plan row
        protected void GvCoursePlan_Delete(object sender, GridViewDeleteEventArgs e)
        {



            DataTable dt = ViewState["CoursePlanTable"] as DataTable;
            dt.Rows.RemoveAt(e.RowIndex);
            gvCoursePlan.DataSource = dt;
            gvCoursePlan.DataBind();
            ViewState["CoursePlanTable"] = dt;

        }

        // for on delete schedule row
        protected void GvSchedule_Delete(object sender, GridViewDeleteEventArgs e)
        {



            DataTable dt = ViewState["ScheduleTable"] as DataTable;
            dt.Rows.RemoveAt(e.RowIndex);
            gvSchedule.DataSource = dt;
            gvSchedule.DataBind();
            ViewState["ScheduleTable"] = dt;

        }

        // Loading the school names from the database
        private void LoadSchools()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string queary = @"SELECT id,school_name FROM School";
                SqlCommand command = new SqlCommand(queary, conn);
                try
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    ddlSchool.DataSource = dt;
                    Response.Write(dt);
                    ddlSchool.DataTextField = "school_name";
                    ddlSchool.DataValueField = "id";
                    ddlSchool.DataBind();
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
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



        protected void SaveCourseClickEvent(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }
            // get the values of the course form
            string course_code = txtCourseCode.Text;
            string course_name = txtCourseName.Text;
            int course_credit = Convert.ToInt32(txtCredits.Text.Trim());
            string description = txtDescription.Text;
            int School_Id = Convert.ToInt32(ddlSchool.SelectedValue);

            // saving to the courses table

            int courseID = Save_to_courses(course_code, course_name, course_credit, description);

            // saving to School_Course map table

            SaveSchoolCourseMap(School_Id, courseID);

            // saving to CoursePlanTable

            SaveCoursePlanTable(courseID);

            // saving to SchedulePlanTable

            SaveSchedulePlan(courseID);

            System.Diagnostics.Debug.WriteLine("All save operations completed");
            
            // Redirect after delay
            ClientScript.RegisterStartupScript(this.GetType(), "redirectScript",
                "setTimeout(function() { window.location = '/Dashbard.aspx'; }, 1000);", true);


        }

        private void SaveSchedulePlan(int courseID)
        {
            DataTable dt = ViewState["ScheduleTable"] as DataTable;
            if (dt == null || dt.Rows.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("No schedule data to save");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    System.Diagnostics.Debug.WriteLine("Saving schedule data - rows: " + dt.Rows.Count);

                    foreach (DataRow row in dt.Rows)
                    {
                        // Skiping empty rows
                        if (row["Weekday"] == DBNull.Value || string.IsNullOrEmpty(row["Weekday"].ToString()))
                        {
                            System.Diagnostics.Debug.WriteLine("Skipping empty row in schedule");
                            continue;
                        }

                        string query = @"INSERT INTO schedule (course_id, schedule_weekday, start_time, end_time, [Room No.]) 
                                VALUES (@CourseId, @schedule_weekday, @StartTime, @EndTime, @RoomNo);
                                SELECT SCOPE_IDENTITY();";
                        using (SqlCommand command = new SqlCommand(query, conn))
                        {
                            command.Parameters.AddWithValue("@CourseId", courseID);
                            command.Parameters.AddWithValue("@schedule_weekday", row["Weekday"]);

                            // Handling time parsing
                            TimeSpan startTime, endTime;
                            if (TimeSpan.TryParse(row["StartTime"].ToString(), out startTime))
                            {
                                command.Parameters.AddWithValue("@StartTime", startTime);
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("Invalid start time format: " + row["StartTime"]);
                                command.Parameters.AddWithValue("@StartTime", new TimeSpan(0, 0, 0));
                            }

                            if (TimeSpan.TryParse(row["EndTime"].ToString(), out endTime))
                            {
                                command.Parameters.AddWithValue("@EndTime", endTime);
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("Invalid end time format: " + row["EndTime"]);
                                command.Parameters.AddWithValue("@EndTime", new TimeSpan(0, 0, 0));
                            }

                            // Checking if RoomNumber is valid
                            if (row["RoomNumber"] != DBNull.Value && !string.IsNullOrEmpty(row["RoomNumber"].ToString()))
                            {
                                command.Parameters.AddWithValue("@RoomNo", row["RoomNumber"]);
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("Missing room number");
                                command.Parameters.AddWithValue("@RoomNo", DBNull.Value);
                            }

                            try
                            {
                                int scheduleId = Convert.ToInt32(command.ExecuteScalar());
                                System.Diagnostics.Debug.WriteLine("Saved schedule row with ID: " + scheduleId);

                                if (scheduleId > 0 && row["RoomNumber"] != DBNull.Value &&
                                    !string.IsNullOrEmpty(row["RoomNumber"].ToString()))
                                {
                                    SaveRoomSchedule(scheduleId, Convert.ToInt32(row["RoomNumber"]));
                                }

                                if (scheduleId > 0 && row["FacultyMember"] != DBNull.Value &&
                                    !string.IsNullOrEmpty(row["FacultyMember"].ToString()))
                                {
                                    SaveFacultyScheduleMap(scheduleId, Convert.ToInt32(row["FacultyMember"]));
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine("Error saving schedule: " + ex.Message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error in SaveSchedulePlan: " + ex.Message);
                }
            }
        }

        private void SaveFacultyScheduleMap(int scheduleId, int facultyNumber)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Faculty_Schedule_Map (schedule_id, faculty_id) 
                         VALUES (@ScheduleId, @FacultyId)";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@ScheduleId", scheduleId);
                command.Parameters.AddWithValue("@FacultyId", facultyNumber);
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

        private void SaveRoomSchedule(int scheduleId, int roomNumber)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Room_Schedule_Map (Schedule_id, Room_No) 
                         VALUES (@ScheduleId, @RoomId)";

                SqlCommand command = new SqlCommand(query,conn);
                command.Parameters.AddWithValue("@ScheduleId", scheduleId);
                command.Parameters.AddWithValue("@RoomId", roomNumber);
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

        private void SaveCoursePlanTable(int courseID)
        {
            DataTable dt = ViewState["CoursePlanTable"] as DataTable;

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
                    command.Parameters.AddWithValue("@SessionNumber", row["SessionNumber"]);
                    command.Parameters.AddWithValue("@Subtopic", row["Subtopic"] ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ReadingMaterials", row["ReadingMaterial"] ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Activity", row["Activity"] ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ImportantDates", row["ImportantDates"] ?? DBNull.Value);
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

        private void SaveSchoolCourseMap(int school_Id, int course_id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO School_Course_Map (School_id, course_id) 
                         VALUES (@SchoolId, @CourseId)";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@SchoolId", school_Id);
                command.Parameters.AddWithValue("@CourseId", course_id);

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

        private int Save_to_courses(string course_code, string course_name, int course_credit, string description)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                int courseId = 0;
                string query = @"INSERT INTO courses (couse_code, course_name, credits, description) 
                         VALUES (@CourseCode, @CourseName, @Credits, @Description);
                         SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@CourseCode", course_code);
                command.Parameters.AddWithValue("@CourseName", course_name);
                command.Parameters.AddWithValue("@Credits", course_credit);
                command.Parameters.AddWithValue("@Description", description);
                try
                {
                    
                    conn.Open();
                    courseId = Convert.ToInt32(command.ExecuteScalar());

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error saving course: " + ex.Message);
                }
                return courseId;
            }
        }
    }
}