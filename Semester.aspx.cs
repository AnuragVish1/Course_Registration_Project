using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CourseRegestrationProject
{
    public partial class WebForm8 : System.Web.UI.Page
    {
        string connectionString = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "(LocalDB)\\MSSQLLocalDB";
            builder.AttachDBFilename = HostingEnvironment.MapPath("~\\App_Data\\Database1.mdf");
            builder.IntegratedSecurity = true;
            connectionString = builder.ToString();

        }

        protected void CreateSemester(object sender, EventArgs e)
        {
            DateTime startDate, endDate;
            // Get the semester name
            string semName = semesterName.Text;
            // Get the Start Date
            string start_date = txtStartDate.Text;
            if (!DateTime.TryParse(txtStartDate.Text, out startDate))
            {
                // Handle invalid start date input - maybe show an error message
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage",
                        "alert('Invalid Start Date format.');", true);
                return;
            }

            // Get the End date and convert format
            string end_date = txtEndDate.Text;
            if (!DateTime.TryParse(txtEndDate.Text, out endDate))
            {
                // Handle invalid end date input
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage",
                        "alert('Invalid End Date format.');", true);
                return;
            }

            // Checking if start_date is equal to end date
            if (start_date == end_date)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage",
                            "alert('Start and End Date cannot be the same');", true);
                return;
            }
            if (endDate < startDate)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage",
                       "alert('End Date cannot be before Start Date.');", true);
                return;
            }
            // Save to Semester Table
            SaveToSemester(semName, startDate, endDate);
            Response.Redirect("Dashbard.aspx");
        }

        private void SaveToSemester(string semName, DateTime start_date, DateTime end_date)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string queary = @"INSERT INTO semester(Sem_Name, sem_start, sem_end, sem_year) VALUES(@SemName,@startDate,@endDate,@year)";
                SqlCommand command = new SqlCommand(queary, conn);  
                command.Parameters.AddWithValue("@SemName", semName);
                command.Parameters.AddWithValue("@startDate", start_date);
                command.Parameters.AddWithValue("@endDate", end_date);
                int year = end_date.Year;
                command.Parameters.AddWithValue("@year", year);
                System.Diagnostics.Debug.WriteLine(semName);
                System.Diagnostics.Debug.WriteLine(start_date);
                System.Diagnostics.Debug.WriteLine(end_date);
                System.Diagnostics.Debug.WriteLine(year);
                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
        }
    }


}
