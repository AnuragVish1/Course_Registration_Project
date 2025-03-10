using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CourseRegestrationProject
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            string pageName = System.IO.Path.GetFileName(url);
            

            if (pageName == "Dashbard.aspx")
            {
                LinkButton1.Attributes.CssStyle.Add("background-color", "white");
                LinkButton1.Attributes.CssStyle.Add("color", "Black");
            }
            else if (pageName == "New_course.aspx")
            {
                LinkButton2.Attributes.CssStyle.Add("background-color", "white");
                LinkButton2.Attributes.CssStyle.Add("color", "Black");
            }
            else if (pageName == "Edit_course.aspx")
            {
                LinkButton3.Attributes.CssStyle.Add("background-color", "white");
                LinkButton3.Attributes.CssStyle.Add("color", "Black");
            }
            else if (pageName == "Student_detail.aspx")
            {
                LinkButton4.Attributes.CssStyle.Add("background-color", "white");
                LinkButton4.Attributes.CssStyle.Add("color", "Black");
            }
        }
    }
}