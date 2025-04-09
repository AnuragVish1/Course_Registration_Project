using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
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

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                try
                {
                    string fileExtension = Path.GetExtension(FileUpload1.FileName).ToLower();

                    // Check if the file is an Excel file
                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        // Create a temporary file to store the uploaded Excel file
                        string fileName = Path.GetTempFileName();
                        FileUpload1.SaveAs(fileName);

                        // Read the Excel file
                        using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
                        {
                        
                            using (var reader = ExcelReaderFactory.CreateReader(stream))
                            {
                                // Initialize DataTable to store data from Excel
                                DataTable dt = ViewState["CoursePlanTable"] as DataTable;
                                if (dt == null)
                                {
                                    InitCoursePlanGrid();
                                    dt = ViewState["CoursePlanTable"] as DataTable;
                                }

                                // Clear existing data
                                dt.Rows.Clear();

                                // Skip the header row (assuming first row is header)
                                reader.Read();

                                // Read data from Excel file and add to DataTable
                                int rowIndex = 1; // Start with 1 to account for session number
                                while (reader.Read())
                                {
                                    DataRow dr = dt.NewRow();

                                    dr["SessionNumber"] = rowIndex++;
                                    dr["Topic"] = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                                    dr["Subtopic"] = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                                    dr["ReadingMaterial"] = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                                    dr["Activity"] = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                                    dr["ImportantDates"] = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);

                                    dt.Rows.Add(dr);
                                }

                            
                                gvCoursePlan.DataSource = dt;
                                gvCoursePlan.DataBind();
                                ViewState["CoursePlanTable"] = dt;
                            }
                        }

                       
                        File.Delete(fileName);
                        ScriptManager.RegisterStartupScript(this, GetType(), "ShowSuccess",
                            "alert('Excel file imported successfully!');", true);
                    }
                    else
                    {
                        // Show error message for invalid file format
                        ScriptManager.RegisterStartupScript(this, GetType(), "ShowError",
                            "alert('Please upload an Excel file (.xls or .xlsx)');", true);
                    }
                }
                catch (Exception ex)
                {
                    // Show error message
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowError",
                        $"alert('Error importing Excel file: {ex.Message}');", true);
                    System.Diagnostics.Debug.WriteLine("Excel import error: " + ex.ToString());
                }
            }
            else
            {
                // Show error message for no file selected
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowError",
                    "alert('Please select an Excel file to import.');", true);
            }
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

        // Importing Data from excel file
        //protected void btnImportExcel_Click(object sender, EventArgs e)
        //{
        //    if (fileUploadExcel.HasFile)
        //    {
        //        try
        //        {
        //            string fileExtension = Path.GetExtension(fileUploadExcel.FileName).ToLower();

        //            // Check if the file is an Excel file
        //            if (fileExtension == ".xls" || fileExtension == ".xlsx")
        //            {
        //                // Create a temporary file to store the uploaded Excel file
        //                string fileName = Path.GetTempFileName();
        //                fileUploadExcel.SaveAs(fileName);

        //                // Read the Excel file
        //                using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
        //                {
        //                    // Auto-detect format, supports:
        //                    // - Binary Excel files (2.0-2003 format; *.xls)
        //                    // - OpenXml Excel files (2007 format; *.xlsx)
        //                    using (var reader = ExcelReaderFactory.CreateReader(stream))
        //                    {
        //                        // Initialize DataTable to store data from Excel
        //                        DataTable dt = ViewState["CoursePlanTable"] as DataTable;
        //                        if (dt == null)
        //                        {
        //                            InitCoursePlanGrid();
        //                            dt = ViewState["CoursePlanTable"] as DataTable;
        //                        }

        //                        // Clear existing data
        //                        dt.Rows.Clear();

        //                        // Skip the header row (assuming first row is header)
        //                        reader.Read();

        //                        // Read data from Excel file and add to DataTable
        //                        int rowIndex = 1; // Start with 1 to account for session number
        //                        while (reader.Read())
        //                        {
        //                            DataRow dr = dt.NewRow();

        //                            // Map Excel columns to DataTable columns
        //                            // Assuming Excel structure: SessionNumber, Topic, Subtopic, ReadingMaterial, Activity, ImportantDates
        //                            dr["SessionNumber"] = rowIndex++;

        //                            // Make sure to check for null values from Excel
        //                            dr["Topic"] = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
        //                            dr["Subtopic"] = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
        //                            dr["ReadingMaterial"] = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
        //                            dr["Activity"] = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
        //                            dr["ImportantDates"] = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);

        //                            dt.Rows.Add(dr);
        //                        }

        //                        // Bind the DataTable to the GridView
        //                        gvCoursePlan.DataSource = dt;
        //                        gvCoursePlan.DataBind();
        //                        ViewState["CoursePlanTable"] = dt;
        //                    }
        //                }

        //                // Delete the temporary file
        //                File.Delete(fileName);

        //                // Show success message
        //                ScriptManager.RegisterStartupScript(this, GetType(), "ShowSuccess",
        //                    "alert('Excel file imported successfully!');", true);
        //            }
        //            else
        //            {
        //                // Show error message for invalid file format
        //                ScriptManager.RegisterStartupScript(this, GetType(), "ShowError",
        //                    "alert('Please upload an Excel file (.xls or .xlsx)');", true);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Show error message
        //            ScriptManager.RegisterStartupScript(this, GetType(), "ShowError",
        //                $"alert('Error importing Excel file: {ex.Message}');", true);
        //            System.Diagnostics.Debug.WriteLine("Excel import error: " + ex.ToString());
        //        }
        //    }
        //    else
        //    {
        //        // Show error message for no file selected
        //        ScriptManager.RegisterStartupScript(this, GetType(), "ShowError",
        //            "alert('Please select an Excel file to import.');", true);
        //    }
        //}






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