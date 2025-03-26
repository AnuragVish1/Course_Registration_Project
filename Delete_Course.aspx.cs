using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CourseRegestrationProject
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Anurag\\source\\repos\\CourseRegestrationProject\\App_Data\\Database1.mdf;Integrated Security=True";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadschoolData();
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
                    Response.Write(dt);
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

        protected void DeleteCourseBtn(object sender, EventArgs e)
        {
            // Get the course id
            int courseId = ddlCourse.SelectedIndex + 1;
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

            // delete course from course table
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
                string query = @"select id from schedule where course_id = @CourseId";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@CourseId", courseId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Add each item to the list
                        ids.Add(Convert.ToInt32(reader["id"]));
                        Response.Write(reader["id"]);
                    }
                }
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
                    Response.Write("Course Student Relation delete");
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
                    Response.Write("Course School Relation delete");
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
                    Response.Write("Course Plan Deleted Sucessfully");
                }
                catch (Exception e)
                {
                    Response.Write(e.Message);
                }
            }
        }
    }
}