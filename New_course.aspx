<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="New_course.aspx.cs" Inherits="CourseRegestrationProject.WebForm2" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .main-content {
            height: 100%;
            width: 100%;
            padding: 20px;
            font-family: sans-serif;
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
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
            padding: 20px;
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
            color: #007bff;
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

        .required:after {
            content: " *";
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
            <h3 class="section-title">Course Details</h3>

            <div class="form-group">
                <label for="txtCourseCode" class="form-label required">Course Code</label>
                <asp:TextBox ID="txtCourseCode" runat="server" CssClass="form-control" placeholder="e.g., CS101" required></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCourseCode" runat="server" ControlToValidate="txtCourseCode"
                    ErrorMessage="Course code is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>

            <div class="form-group">
                <label for="txtCourseName" class="form-label required">Course Name</label>
                <asp:TextBox ID="txtCourseName" runat="server" CssClass="form-control" placeholder="e.g., Introduction to Programming" required></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCourseName" runat="server" ControlToValidate="txtCourseName"
                    ErrorMessage="Course name is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>

            <div class="form-group">
                <label for="txtCredits" class="form-label required">Credits</label>
                <asp:TextBox ID="txtCredits" runat="server" CssClass="form-control" TextMode="Number" min="1" max="6" placeholder="e.g., 3" required></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCredits" runat="server" ControlToValidate="txtCredits"
                    ErrorMessage="Credits are required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rvCredits" runat="server" ControlToValidate="txtCredits"
                    MinimumValue="1" MaximumValue="6" Type="Integer"
                    ErrorMessage="Credits must be between 1 and 6" CssClass="text-danger" Display="Dynamic"></asp:RangeValidator>
            </div>

            <div class="form-group">
                <label for="ddlSchool" class="form-label required">School</label>
                <asp:DropDownList ID="ddlSchool" runat="server" CssClass="form-select">
                    <asp:ListItem Value="" Text="-- Select School --" Selected="True"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvSchool" runat="server" ControlToValidate="ddlSchool"
                    ErrorMessage="School is required" CssClass="text-danger" Display="Dynamic" InitialValue=""></asp:RequiredFieldValidator>
            </div>

            <div class="form-group">
                <label for="txtDescription" class="form-label">Description</label>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4"
                    placeholder="Enter course description, objectives, and learning outcomes..."></asp:TextBox>
            </div>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="form-container">
                    <h3 class="section-title">Course Plan</h3>
                    <p>Add sessions and their details for the course plan.</p>


                    <div class="table-actions">
                        <asp:Button ID="btnAddCoursePlanRow" runat="server" Text="+ Add Session" CssClass="btn-add-row" CausesValidation="false" OnClick="AddCoursePlanBtn_Click" />
                    </div>

                    <div class="table-container">
                        <asp:GridView ID="gvCoursePlan" runat="server" CssClass="dynamic-table" AutoGenerateColumns="false" OnRowDeleting="GvCoursePlan_Delete">
                            <Columns>
                                <asp:TemplateField HeaderText="Session Number">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtSessionNumber" runat="server" CssClass="form-control" TextMode="Number" min="1"
                                            Text='<%# Eval("SessionNumber") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Topic">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTopic" runat="server" CssClass="form-control"
                                            Text='<%# Eval("Topic") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Subtopic">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtSubtopic" runat="server" CssClass="form-control" TextMode="MultiLine"
                                            Text='<%# Eval("Subtopic") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reading Material">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtReadingMaterial" runat="server" CssClass="form-control"
                                            Text='<%# Eval("ReadingMaterial") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Activity">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtActivity" runat="server" CssClass="form-control"
                                            Text='<%# Eval("Activity") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Important Dates">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtImportantDates" runat="server" CssClass="form-control" TextMode="Date"
                                            Text='<%# Eval("ImportantDates", "{0:yyyy-MM-dd}") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDeleteCoursePlan" runat="server" Text="Delete" CommandName="Delete" CssClass="delete-btn"
                                            CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('Are you sure you want to delete this session?');"
                                            CausesValidation="false"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="text-center">No sessions added yet. Click "Add Session" to create course plan.</div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class="form-container">
                    <h3 class="section-title">Course Schedule</h3>
                    <p>Add schedule details for the course.</p>

                    <div class="table-actions">
                        <asp:Button ID="btnAddScheduleRow" runat="server" Text="+ Add Schedule" CssClass="btn-add-row" CausesValidation="false" OnClick="AddCourseScheduleBtn_Click" />
                    </div>

                    <div class="table-container">
                        <asp:GridView ID="gvSchedule" runat="server" CssClass="dynamic-table" AutoGenerateColumns="false" OnRowDeleting="GvSchedule_Delete" OnDataBound="gvSchedule_DataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Weekday">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlWeekday" runat="server" CssClass="form-select"
                                            SelectedValue='<%# Eval("Weekday") %>'>
                                            <asp:ListItem Value="" Text="-- Select Day --" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Monday"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Tuesday"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Wednesday"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Thursday"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="Friday"></asp:ListItem>
                                            <asp:ListItem Value="6" Text="Saturday"></asp:ListItem>
                                            <asp:ListItem Value="7" Text="Sunday"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Start Time">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtStartTime" runat="server" CssClass="form-control" TextMode="Time"
                                            Text='<%# Eval("StartTime") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="End Time">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtEndTime" runat="server" CssClass="form-control" TextMode="Time"
                                            Text='<%# Eval("EndTime") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Room Number">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlRoomNumber" runat="server" CssClass="form-select">
                                            <asp:ListItem Value="" Text="-- Select Room --" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Faculty Member">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlFacultyMember" runat="server" CssClass="form-select">
                                            <asp:ListItem Value="" CssClass="form-option" Text="-- Select Faculty --" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDeleteSchedule" runat="server" Text="Delete" CssClass="delete-btn" CommandName="Delete"
                                            CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('Are you sure you want to delete this schedule?');"
                                            CausesValidation="false"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="text-center">No schedules added yet. Click "Add Schedule" to create course schedule.</div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="btn-container">
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn-cancel" CausesValidation="false" />
            <asp:Button ID="btnSave" runat="server" Text="Save Course" CssClass="btn-primary" OnClick="SaveCourseClickEvent"/>
        </div>
    </div>
</asp:Content>
