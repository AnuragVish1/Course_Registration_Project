<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Overview.aspx.cs" Inherits="CourseRegestrationProject.WebForm5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
    .main-content {
        height: 100%;
        width: 100%;
        padding: 20px;
        font-family: "Inter";
    }

    .page-title {
        color: #333;
        margin-bottom: 20px;
        padding-bottom: 10px;
        border-bottom: 2px solid #007bff;
    }

    .form-container {
        background-color: #fff;
border-radius: 8px;
box-shadow: 0 1px 5px rgba(0, 0, 0, 0.1);
padding: 20px;
border: 1px solid #e3dbdb;
margin-bottom: 30px;
    }

    .form-group {
        margin-bottom: 35px;
    }

    .form-label {
        font-weight: 600;
        display: block;
        margin-bottom: 5px;
        color: #555;
    }

    .form-control {
        width: 100%;
        padding: 8px 12px;
        border: 1px solid #ddd;
        border-radius: 4px;
        font-size: 14px;
    }

    .form-control:focus {
        border-color: #007bff;
        outline: none;
    }

    .section-title {
        color: #000;
        margin: 25px 0 15px 0;
        padding-bottom: 15px;
        border-bottom: 1px solid #eee;
        font-size: 32px;
    }

    .btn-container {
        margin-top: 20px;
        text-align: right;
    }

    .btn-primary {
        background-color: #007bff;
        color: white;
        border: none;
        padding: 8px 16px;
        border-radius: 4px;
        cursor: pointer;
        font-weight: 600;
    }

    .btn-cancel {
        background-color: #fbbdbd;
        color: #e34646;
        border: none;
        padding: 8px 16px;
        border-radius: 4px;
        cursor: pointer;
        margin-right: 10px;
        font-weight: 600;
    }

    .btn-primary:hover, .btn-secondary:hover {
        opacity: 0.9;
    }

    .table-container {
        margin-top: 20px;
        overflow-x: auto;
    }

    .dynamic-table {
        width: 100%;
        border-radius: 10px;
        
    }

    .delete-btn {
        text-decoration: none;
        text-align: center;
        background-color: #f5f5f5;
        padding: 8px;
        color: black;
        border-radius: 6px;
        transition: all 0.3s ease;
    }

        .delete-btn:hover {
            transform: scale(0.8, 0.8);
            transition: all 0.3s ease;
        }

    .dynamic-table th {
        background-color: #fafafa;
        padding: 12px;
        text-align: left;
        font-weight: 600;
        color: #495057;
        border-radius: 10px;
        
       
    }

    .dynamic-table td {
        padding: 12px;

    }

    .table-actions {
        display: flex;
        justify-content: space-between;
        margin-bottom: 10px;
    }

    .btn-add-row {
        background-color: #28a745;
        color: white;
        border: none;
        padding: 10px 18px;
        border-radius: 6px;
        cursor: pointer;
        font-size: 14px;
        margin-top: 20px;
    }

    .form-select {
        width: 100%;
        padding: 8px 12px;
        border: 1px solid #ddd;
        border-radius: 4px;
        font-size: 14px;
        background-color: white;

    }

    .text-danger {
        color: #dc3545;
    }


    .form-option
    {
        padding:10px;
        border-radius:2px;
    }
</style>

<div class="main-content">

    <div class="form-container">
        <h3 class="section-title">Edit Course Details</h3>

        <div class="form-group">
            <label for="txtCourseCode" class="form-label required">Course Code</label>
            <asp:Label ID="txtCourseCode" runat="server"><%# Eval("course_code") %></asp:Label>
           
        </div>

        <div class="form-group">
            <label for="txtCourseName" class="form-label required">Course Name</label>
            <asp:Label ID="txtCourseName" runat="server"><%# Eval("course_name") %></asp:Label>
           
        </div>

        <div class="form-group">
            <label for="txtCredits" class="form-label required">Credits</label>
            <asp:Label ID="txtCredits" runat="server"><%# Eval("credits") %></asp:Label>
            
            
        </div>

        <div class="form-group">
            <label for="ddlSchool" class="form-label required">School</label>
            <asp:Label ID="School" runat="server">
                <%# Eval("school_name") %>
            </asp:Label>

        </div>
        <div class="form-group">
            <label for="txtDescription" class="form-label">Description</label>

            <asp:Label ID="txtDescription" runat="server"
    Text='<%# Eval("description") %>'></asp:Label>
        </div>
   
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="form-container">
                <h3 class="section-title">Course Plan</h3>
                

                <div class="table-actions">
                   
                </div>

                <div class="table-container">
                    <asp:GridView ID="gvCoursePlan" runat="server" CssClass="dynamic-table" AutoGenerateColumns="false" >
                        <Columns>
                            <asp:TemplateField HeaderText="Session Number">
                                <ItemTemplate>
                                    <asp:Label ID="txtSessionNumber" runat="server" TextMode="Number" min="1"
                                        Text='<%# Eval("session_number") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Topic">
                                <ItemTemplate>
                                    <asp:Label ID="txtTopic" runat="server"
                                        Text='<%# Eval("Topic") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Subtopic">
                                <ItemTemplate>
                                    <asp:Label ID="txtSubtopic" runat="server"
                                        Text='<%# Eval("Subtopic") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reading Material">
                                <ItemTemplate>
                                    <asp:Label ID="txtReadingMaterial" runat="server" 
                                        Text='<%# Eval("reading_materials") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Activity">
                                <ItemTemplate>
                                    <asp:Label ID="txtActivity" runat="server"
                                        Text='<%# Eval("Activity") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Important Dates">
                                <ItemTemplate>
                                    <asp:Label ID="txtImportantDates" runat="server"
                                        Text='<%# Eval("Important_dates") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                  
                    </asp:GridView>
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div class="form-container">
                <h3 class="section-title">Course Schedule</h3>
                

                <div class="table-actions">
                    
                </div>

                <div class="table-container">
                    <asp:GridView ID="gvSchedule" runat="server" CssClass="dynamic-table" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField HeaderText="Weekday">
                                <ItemTemplate>
                                    <asp:Label ID="txtWeekday" runat="server"
    Text='<%# Eval("schedule_weekday") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Start Time">
                                <ItemTemplate>
                                    <asp:Label ID="txtStartTime" runat="server"
                                        Text='<%# Eval("StartTime") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="End Time">
                                <ItemTemplate>
                                    <asp:Label ID="txtEndTime" runat="server" 
                                        Text='<%# Eval("EndTime") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Room Number">
                                <ItemTemplate>
                                    <asp:Label ID="txtRoomNumber" runat="server"
    Text='<%# Eval("RoomNumber") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Faculty Member">
                                <ItemTemplate>
                        <asp:Label ID="txtFacultyMember" runat="server" 
Text='<%# Eval("faculty_name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                        </Columns>
                        
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="btn-container">
        <asp:Button ID="btnDash" runat="server" Text="Go to Dashboard" CssClass="btn-primary" OnClick="goDashboard" />
        <asp:Button ID="btnEdit" runat="server" Text="Edit Course" CssClass="btn-primary" OnClick="goEditCourse" />
    </div>
</div>
</asp:Content>

