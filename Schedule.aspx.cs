using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Printing;

namespace CourseRegestrationProject
{
    public partial class WebForm7 : System.Web.UI.Page
    {
        string connectionString = "";
        private int selectedCourseId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "(LocalDB)\\MSSQLLocalDB";
            builder.AttachDBFilename = HostingEnvironment.MapPath("~\\App_Data\\Database1.mdf");
            builder.IntegratedSecurity = true;
            connectionString = builder.ToString();

            if (!IsPostBack)
            {
                LoadSemesters();
                InitializeScheduleTable();
            }
            else
            {
                // Retrieve selected course ID from ViewState if it exists
                if (ViewState["SelectedCourseId"] != null)
                {
                    selectedCourseId = Convert.ToInt32(ViewState["SelectedCourseId"]);
                }
            }
        }

        private void InitializeScheduleTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("Weekday", typeof(string));
            dt.Columns.Add("StartTime", typeof(string));
            dt.Columns.Add("EndTime", typeof(string));
            dt.Columns.Add("RoomNumber", typeof(string));
            dt.Columns.Add("FacultyMember", typeof(string));

            ViewState["ScheduleTable"] = dt;
        }

        private void LoadSemesters()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT id, Sem_Name FROM Semester";
                SqlCommand command = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    ddlSemester.DataSource = dt;
                    ddlSemester.DataTextField = "Sem_Name";
                    ddlSemester.DataValueField = "id";
                    ddlSemester.DataBind();

                    // Adding the default "Select Semester" option
                    ddlSemester.Items.Insert(0, new ListItem("Select Semester", ""));
                }
                catch (Exception ex)
                {
                    Response.Write("Error loading semesters: " + ex.Message);
                }
            }
        }

        protected void selectedSemesterChanged(object sender, EventArgs e)
        {
            LoadCoursesForSemester();
            // Hide schedule panel when semester changes
            pnlScheduleEditor.Visible = false;
        }

        protected void liveSearching(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                LoadCoursesForSemester();
            }
            else
            {
                SearchCourses(searchTerm);
            }
        }

        private void SearchCourses(string searchTerm)
        {
            if (string.IsNullOrEmpty(ddlSemester.SelectedValue))
            {
                return; // No semester selected
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT c.id, c.couse_code, c.course_name, c.credits, s.short
                FROM courses c
                INNER JOIN Course_Semester_Map csm ON c.id = csm.course_id
                LEFT JOIN School_Course_Map scm ON c.id = scm.course_id
                LEFT JOIN School s ON scm.School_id = s.id
                WHERE csm.semester_id = @SemesterId
                AND (c.course_name LIKE @SearchTerm OR c.couse_code LIKE @SearchTerm)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SemesterId", ddlSemester.SelectedValue);
                command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                try
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    gvSemesterCourses.DataSource = dt;
                    gvSemesterCourses.DataBind();
                }
                catch (Exception ex)
                {
                    Response.Write("Error: " + ex.Message);
                }
            }
        }
        // for on delete schedule row


        protected void btnAddScheduleRow_Click(object sender, EventArgs e)
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

        protected void btnSaveSchedule_Click(object sender, EventArgs e)
        {
            // Validate Time and faculty clash

            // Delete Schedule Entries
            List<int> schedule_ids = ScheduleIds(selectedCourseId);

            DeleteFacultySchedule(schedule_ids);

            DeleteRoomSchedule(schedule_ids);

            DeleteScheduleData(selectedCourseId);
            // Save Shedule
            SaveSchedulePlan(selectedCourseId);
        }

        private void DeleteScheduleData(int courseId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"delete from schedule where course_id = @CourseId and semester_id = @SemesterId";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@CourseId", courseId);
                command.Parameters.AddWithValue("@SemesterId", ddlSemester.SelectedValue);
                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }
        private void DeleteRoomSchedule(List<int> schedule_ids)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                foreach (int schedule_id in schedule_ids)
                {
                    string query = @"delete from Room_Schedule_Map where [Schedule_id] = @schedule_id";
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@schedule_id", schedule_id);

                    try
                    {
                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                }
            }
        }
        private void DeleteFacultySchedule(List<int> schedule_ids)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                foreach (int schedule_id in schedule_ids)
                {
                    string query = @"delete from Faculty_Schedule_Map where [schedule_id] = @schedule_id";
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@schedule_id", schedule_id);

                    try
                    {
                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                }
            }
        }
        private List<int> ScheduleIds(int courseId)
        {
            List<int> ids = new List<int>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"select id from schedule where course_id = @CourseId and semester_id = @SemesterId";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@CourseId", courseId);
                command.Parameters.AddWithValue("@SemesterId", ddlSemester.SelectedValue);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Add each item to the list
                        ids.Add(Convert.ToInt32(reader["id"]));

                    }
                }
                conn.Close();
            }

            return ids;

        }


        private void LoadCoursesForSemester()
        {
            if (string.IsNullOrEmpty(ddlSemester.SelectedValue))
            {
                return; // No semester selected
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT c.id, c.couse_code, c.course_name, c.credits, s.short
                FROM courses c
                INNER JOIN Course_Semester_Map csm ON c.id = csm.course_id
                LEFT JOIN School_Course_Map scm ON c.id = scm.course_id
                LEFT JOIN School s ON scm.School_id = s.id
                WHERE csm.semester_id = @SemesterId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SemesterId", ddlSemester.SelectedValue);

                try
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    gvSemesterCourses.DataSource = dt;
                    gvSemesterCourses.DataBind();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        protected void gvSemesterCourses_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SelectCourse")
            {
                int courseId = Convert.ToInt32(e.CommandArgument);
                selectedCourseId = courseId;
                ViewState["SelectedCourseId"] = courseId;


                // Load course details and schedule
                LoadCourseDetails(courseId);
                LoadCourseSchedule(courseId);

                // Show the schedule editor panel
                pnlScheduleEditor.Visible = true;
            }
        }

        private void LoadCourseDetails(int courseId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT c.couse_code, c.course_name, c.credits, s.school_name, s.short
                FROM courses c
                LEFT JOIN School_Course_Map scm ON c.id = scm.course_id
                LEFT JOIN School s ON scm.School_id = s.id
                WHERE c.id = @CourseId";

                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@CourseId", courseId);

                try
                {
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        lblSelectedCourseCode.Text = reader["couse_code"].ToString();
                        lblSelectedCourseName.Text = reader["course_name"].ToString();
                        lblSelectedSchool.Text = reader["school_name"].ToString();
                        lblSelectedCredits.Text = reader["credits"].ToString();
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);

                }
            }
        }

        private void LoadCourseSchedule(int courseId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT s.id, s.schedule_weekday as Weekday, s.start_time as StartTime, s.end_time as EndTime, 
                       s.[Room No.] as RoomNumber, f.faculty_name as FacultyMember
                FROM schedule s
                LEFT JOIN Faculty_Schedule_Map fsm ON s.id = fsm.schedule_id
                LEFT JOIN Faculty f ON fsm.faculty_id = f.id
                WHERE s.course_id = @CourseId and s.semester_id = @SemesterId";

                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@CourseId", courseId);
                command.Parameters.AddWithValue("@Semesterid", ddlSemester.SelectedValue);

                try
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        pnlNoSchedule.Visible = true;
                    }
                    else
                    {
                        pnlNoSchedule.Visible = false;
                    }

                    gvSchedule.DataSource = dt;
                    gvSchedule.DataBind();
                    ViewState["ScheduleTable"] = dt;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        protected void gvSchedule_DataBound(object sender, EventArgs e)
        {
            // Load rooms and faculty for each row
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
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private int GetFacultyNumber(int rowIndex, GridViewRow row)
        {
            int facultyIndex = 0;
            int courseID = selectedCourseId;
            DropDownList ddlWeekDay = (DropDownList)row.FindControl("ddlWeekday");
            string weekday = ddlWeekDay.SelectedValue;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"select faculty_id from Faculty_Schedule_Map where schedule_id = (select id from schedule where schedule_weekday = @weekday and course_id = @Course_ID and semester_id = @SemesterId)";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@weekday", weekday);
                command.Parameters.AddWithValue("@Course_ID", courseID);
                command.Parameters.AddWithValue("@SemesterId", ddlSemester.SelectedValue);

                try
                {
                    conn.Open();
                    facultyIndex = Convert.ToInt32(command.ExecuteScalar());


                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
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
                            ddlRoomNumber.SelectedIndex = GetRoomNumber(row);
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

                }
            }
        }


        private int GetRoomNumber(GridViewRow row)
        {
            int room_number = 0;

            int courseID = selectedCourseId;
            DropDownList ddlWeekDay = (DropDownList)row.FindControl("ddlWeekday");
            string weekday = ddlWeekDay.SelectedValue;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"select [Room No.] from schedule where schedule_weekday = @weekday and course_id = @Course_ID and semester_id = @SemesterId";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@weekday", weekday);
                command.Parameters.AddWithValue("@Course_ID", courseID);
                command.Parameters.AddWithValue("@SemesterId", ddlSemester.SelectedValue);

                try
                {
                    conn.Open();
                    room_number = Convert.ToInt32(command.ExecuteScalar()) - 1;

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            return room_number;
        }

        // Deleting Schedule Row
        protected void GvSchedule_Delete(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dt = ViewState["ScheduleTable"] as DataTable;
            dt.Rows.RemoveAt(e.RowIndex);
            gvSchedule.DataSource = dt;
            gvSchedule.DataBind();
            ViewState["ScheduleTable"] = dt;
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
                        // TODO: check for clash in time or faculty name or room

                        string query = @"INSERT INTO schedule (course_id, schedule_weekday, start_time, end_time, [Room No.], semester_id) 
                                VALUES (@CourseId, @schedule_weekday, @StartTime, @EndTime, @RoomNo, @semester);
                                SELECT SCOPE_IDENTITY();";
                        using (SqlCommand command = new SqlCommand(query, conn))
                        {
                            command.Parameters.AddWithValue("@CourseId", courseID);
                            command.Parameters.AddWithValue("@semester", ddlSemester.SelectedValue);
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
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void SaveRoomSchedule(int scheduleId, int roomNumber)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Room_Schedule_Map (Schedule_id, Room_No) 
                VALUES (@ScheduleId, @RoomId)";

                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@ScheduleId", scheduleId);
                command.Parameters.AddWithValue("@RoomId", roomNumber);
                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                }

                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }
    }

}