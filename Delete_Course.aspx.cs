using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Web.Hosting;

namespace CourseRegestrationProject
{
    public partial class WebForm4 : System.Web.UI.Page
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
                LoadschoolData();
                LoadSemester();
                
            }
        }

       

        private void LoadSemester()
        {
            // getting all the semester the course is in and loading it in ddlSemester
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string queary = @"SELECT id, Sem_Name from semester where id IN (SELECT semester_id from Course_Semester_Map where course_id = @CourseId)";
                SqlCommand command = new SqlCommand(queary, conn);
                command.Parameters.AddWithValue("@CourseId", ddlCourse.SelectedValue);
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    ddlsemester.DataSource = dt;
                    ddlsemester.DataTextField = "Sem_Name";
                    ddlsemester.DataValueField = "id";
                    ddlsemester.DataBind();
                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }

            }
        }

        private void LoadschoolData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string queary = @"SELECT id, [course_name] FROM courses";
                SqlCommand command = new SqlCommand(queary, conn);
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    ddlCourse.DataSource = dt;
                    
                    ddlCourse.DataTextField = "course_name";
                    ddlCourse.DataValueField = "id";
                    ddlCourse.DataBind();


                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }
        }

        protected void LoadSem(object sender, EventArgs e)
        {

            LoadSemester();
        }

        protected void DeleteCourseBtn(object sender, EventArgs e)
        {
            // Get the course id
            int courseId = Convert.ToInt32(ddlCourse.SelectedValue);
            // delete course plan
            DeleteCoursePlan(courseId);
            // delete school course
            DeleteSchoolCourseMap(courseId);
            // delete Course_Student_Map Entry
            DeleteCourseStudentMap(courseId);
            // get schedule ids for the course
            List<int> schedule_ids = ScheduleIds(courseId);
            // delete Faculty_Schedule_Map Entry
            DeleteFacultySchedule(schedule_ids);
            // delete Room_Schedule Entry
            DeleteRoomSchedule(schedule_ids);
            // delete schedule entries
            DeleteScheduleData(courseId);
            // delete from Course_Semester_Map Table
            DeleteCourseSemesterMap(courseId);
            //DeleteCourseData(courseId);
            // redirect to dashboard
            ClientScript.RegisterStartupScript(this.GetType(), "redirectScript",
                "setTimeout(function() { window.location = '/Dashbard.aspx'; }, 1000);", true);
        }

        private void DeleteCourseSemesterMap(int courseId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string queary = @"delete from Course_Semester_Map where course_id = @CourseId and semester_id = @SemesterId";
                SqlCommand command = new SqlCommand(queary, conn);  
                command.Parameters.AddWithValue("@CourseId", courseId);
                command.Parameters.AddWithValue("@SemesterId", ddlsemester.SelectedValue);

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

        private void DeleteCourseData(int courseId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"delete from courses where id = @CourseId";
                SqlCommand command = new SqlCommand(query,conn);
                command.Parameters.AddWithValue("@CourseId",courseId);
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

        private void DeleteScheduleData(int courseId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"delete from schedule where course_id = @CourseId";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@CourseId", courseId);
                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Response.Write(e.ToString());
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
                    catch (Exception e)
                    {
                        Response.Write(e.ToString());
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
                    catch (Exception e)
                    {
                        Response.Write(e.ToString());   
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
                command.Parameters.AddWithValue("@SemesterId", ddlsemester.SelectedValue);
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

        private void DeleteCourseStudentMap(int courseId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"delete from Course_Student_Map where course_id = @CourseId";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@CourseId", courseId);
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
        }

        private void DeleteSchoolCourseMap(int courseId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"delete from School_Course_Map where course_id = @CourseId";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@CourseId", courseId);
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
        }

        private void DeleteCoursePlan(int courseId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"delete from course_plan where course_id = @CourseId";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("CourseId", courseId);
                try
                {
                    con.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Response.Write(e.Message);
                }
            }
        }
    }
}