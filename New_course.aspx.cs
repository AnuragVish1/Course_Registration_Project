using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CourseRegestrationProject
{
    public partial class WebForm2 : System.Web.UI.Page
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
                LoadSchools();
                InitCoursePlanGrid();
            }
            else
            {
                if (ViewState["CoursePlanTable"] == null)
                {
                    InitCoursePlanGrid();
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

            dt.Columns.Add("ImportantDates", typeof(string));

            ViewState["CoursePlanTable"] = dt;
            gvCoursePlan.DataSource = dt;
            gvCoursePlan.DataBind();
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
                    dt.Rows[i]["ImportantDates"] = ((TextBox)row.FindControl("txtImportantDates")).Text;

                }
            }

            DataRow dr = dt.NewRow();
            dr["SessionNumber"] = dt.Rows.Count + 1;
            dt.Rows.Add(dr);
            gvCoursePlan.DataSource = dt;
            gvCoursePlan.DataBind();
            ViewState["CoursePlanTable"] = dt;
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
            

            // check if the course code or course name already exists in the database
            if (IsExisting(course_code, course_name) != "")
            {
                string errorMessage = IsExisting(course_code, course_name);
                string message = $"Course with same {errorMessage} already exists";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal",
                $"showModal('{message}');", true);

                return;
            }

            // Check if there is no clash when there is Schedule of same day of same start and end time and of same room


            // saving to the courses table

            int courseID = Save_to_courses(course_code, course_name, course_credit, description);

            // saving to School_Course map table

            SaveSchoolCourseMap(School_Id, courseID);

            // saving to CoursePlanTable

            SaveCoursePlanTable(courseID);

            System.Diagnostics.Debug.WriteLine("All save operations completed");
            
            // Redirect after delay
            ClientScript.RegisterStartupScript(this.GetType(), "redirectScript",
                "setTimeout(function() { window.location = '/Dashbard.aspx'; }, 1000);", true);


        }


        private string IsExisting(string course_code, string course_name)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT 
                        CASE 
                            WHEN EXISTS(SELECT 1 FROM courses WHERE couse_code = @CourseCode) 
                            AND EXISTS(SELECT 1 FROM courses WHERE course_name = @CourseName)
                            
                            THEN 'code and name' 
                            WHEN EXISTS(SELECT 1 FROM courses WHERE couse_code = @CourseCode)
                            
                            THEN 'Code' 
                            WHEN EXISTS(SELECT 1 FROM courses WHERE course_name = @CourseName)
                            
                            THEN 'Name' 
                            ELSE '' 
                        END";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@CourseCode", course_code);
                command.Parameters.AddWithValue("@CourseName", course_name);
                
                try
                {
                    conn.Open();
                    return command.ExecuteScalar().ToString();
                }

                catch(Exception ex) 
                {
                    Response.Write(ex.ToString());  
                    return "Course Code and Course Name";
                }
            }
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

                        string query = @"INSERT INTO schedule (course_id, schedule_weekday, start_time, end_time, [Room No.], semester) 
                                VALUES (@CourseId, @schedule_weekday, @StartTime, @EndTime, @RoomNo, @semester);
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
                    command.Parameters.AddWithValue("@Subtopic", row["Subtopic"]);
                    command.Parameters.AddWithValue("@ReadingMaterials", row["ReadingMaterial"]);
                    command.Parameters.AddWithValue("@Activity", row["Activity"]);
                    command.Parameters.AddWithValue("@ImportantDates", row["ImportantDates"]);
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