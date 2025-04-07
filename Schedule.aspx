<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Schedule.aspx.cs" Inherits="CourseRegestrationProject.WebForm7" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .main-content {
            height: 100vh;
            width: 100%;
            padding: 20px;
            font-family: 'Inter';
        }

        .form-container {
            background-color: #fff;
            border-radius: 8px;
            box-shadow: 0 1px 5px rgba(0, 0, 0, 0.1);
            padding: 20px;
            border: 1px solid #e3dbdb;
            margin-bottom: 30px;
            min-height: 80%;
        }

        .page-title {
            border-bottom: 1px solid #ebe6e6;
            padding-bottom: 1rem;
        }

        .form-select {
            width: 30%;
            padding: 10px 12px;
            height: 2.6rem;
            border: 1px solid #ddd;
            border-radius: 6px;
            font-size: 16px;
            background-color: white;
            -webkit-appearance: none;
            appearance: none;
            -moz-appearance: none;
            background-image: url('https://www.svgrepo.com/show/80156/down-arrow.svg');
            background-repeat: no-repeat;
            background-size: 12px 12px;
            background-position: calc(100% - 12px);
        }

        .select-sem-container {
            padding-top: 2rem;
            display: flex;
            justify-content: space-between;
            align-items: center;
            gap: 1rem;
        }

        .inputField {
            width: 100%;
            height: 2.6rem;
            outline: none;
            border-radius: 6px;
            font-size: medium;
            padding: 1rem;
            border: 1px solid #e6e2e2;
            transition: border-color 0.3s ease, box-shadow 0.3s ease;
            padding-left: 38px;
            background-image: url("data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIyNCIgaGVpZ2h0PSIyNCIgdmlld0JveD0iMCAwIDI0IDI0IiBmaWxsPSJub25lIiBzdHJva2U9IiM3NTc1NzUiIHN0cm9rZS13aWR0aD0iMiIgc3Ryb2tlLWxpbmVjYXA9InJvdW5kIiBzdHJva2UtbGluZWpvaW49InJvdW5kIiBjbGFzcz0ibHVjaWRlIGx1Y2lkZS1zZWFyY2gtaWNvbiBsdWNpZGUtc2VhcmNoIj48Y2lyY2xlIGN4PSIxMSIgY3k9IjExIiByPSI4Ii8+PHBhdGggZD0ibTIxIDIxLTQuMy00LjMiLz48L3N2Zz4=");
            background-repeat: no-repeat;
            background-size: 18px 18px;
            background-position: 10px center;
        }

        .course-table-container, .schedule-container {
            padding-top: 2rem;
        }

        .action-btn {
            text-decoration: none;
            text-align: center;
            background-color: #f5f5f5;
            padding: 8px;
            color: black;
            border-radius: 6px;
            transition: all 0.3s ease;
        }

        .course-table, .schedule-table {
            margin-top: 1rem;
            background-color: #fdfdfd;
            border-radius: 8px;
            border: 1px solid #e3dbdb;
            padding: 1px;
        }

        .dynamic-table {
            width: 100%;
            border: none;
            border-collapse: collapse;
        }

            .dynamic-table th {
                padding: 1rem;
                text-align: left;
                font-weight: 600;
                border: none;
                border-bottom: 1px solid #eee;
            }

            .dynamic-table tr {
                border: none;
                transition: all 0.3s ease;
            }

            .dynamic-table td {
                padding: 0.8rem;
                border: none;
                border-bottom: 1px solid #eee;
            }

            .dynamic-table tr:last-child td {
                border-bottom: none;
            }

            .dynamic-table th:last-child {
                text-align: center;
            }

            .dynamic-table .action-column {
                width: 12%;
                text-align: center;
            }

            .dynamic-table tr:hover {
                background-color: #f6f6f6;
                transition: all 0.3s ease;
            }

        .inputField:focus {
            border-color: #4a90e2;
            box-shadow: 0 0 0 3px rgba(74, 144, 226, 0.3);
        }

        .add-icon {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 28px;
            height: 28px;
            border-radius: 4px;
            background-color: #f5f5f5;
            color: #333;
            text-decoration: none;
            transition: all 0.3s ease;
        }

            .add-icon:hover {
                background-color: #e0e0e0;
            }

            .add-icon svg {
                width: 14px;
                height: 14px;
            }

        .sub-heading {
            font-weight: 600;
        }

        .schedule-editor {
            padding: 20px;
            border: 1px solid #eee;
            border-radius: 8px;
            margin-top: 20px;
            background-color: #f9f9f9;
        }

        .btn-container {
            margin-top: 20px;
            display: flex;
            justify-content: flex-end;
            gap: 10px;
        }

        .action-button {
            padding: 8px 16px;
            border-radius: 6px;
            border: none;
            font-weight: 500;
            cursor: pointer;
            transition: all 0.2s ease;
        }

        .primary-button {
            background-color: #6ec52c;
            color: white;
            padding: 12px;
            font-size: 15px;
        }

            .primary-button:hover {
                background-color: #2c9a0f;
            }

        .secondary-button {
            background-color: #f5f5f5;
            color: #333;
            padding: 12px;
font-size: 15px;
        }

            .secondary-button:hover {
                background-color: #e5e5e5;
            }

        .no-schedule-message {
            padding: 20px;
            text-align: center;
            color: #666;
            font-style: italic;
        }

        .selected-course-info {
            background-color: #f9c7c715;
            padding: 15px;
            border-radius: 6px;
            margin-bottom: 20px;
            border-left: 4px solid #c23434;
        }

            .selected-course-info > h3 {
                padding-bottom: 0.8rem;
            }

        .input-group {
            display: flex;
            gap: 10px;
            margin-bottom: 15px;
        }

            .input-group .form-control {
                flex: 1;
                padding: 8px 12px;
                border: 1px solid #ddd;
                border-radius: 4px;
            }

        .form-timer {
            font-family: 'Inter';
            padding: 10px 12px;
            height: 2.6rem;
            border: 1px solid #ddd;
            border-radius: 6px;
            font-size: 16px;
            width: 100%;
        }

        .dynamic-table > .form-timer {
            width: 10%;
        }

        .form-select-weekday {
            width: 100%;
            padding: 10px 12px;
            height: 2.6rem;
            border: 1px solid #ddd;
            border-radius: 6px;
            font-size: 16px;
            background-color: white;
            -webkit-appearance: none;
            appearance: none;
            -moz-appearance: none;
            background-image: url('https://www.svgrepo.com/show/80156/down-arrow.svg');
            background-repeat: no-repeat;
            background-size: 12px 12px;
            background-position: calc(100% - 12px);
        }

        .action-column-remove {
            text-align: center;
        }
        .room-column{
            width: 18%;
        }
    </style>
    <div class="main-content">
        <div class="form-container">
            <div>
                <h1 class="page-title">Manage Course Schedule</h1>
            </div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="select-sem-container">
                        <asp:DropDownList ID="ddlSemester" runat="server" CssClass="form-select" OnSelectedIndexChanged="selectedSemesterChanged" AutoPostBack="true">
                            <asp:ListItem Value="" Text="Select Semester" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="inputField" OnTextChanged="liveSearching" AutoPostBack="true" placeholder="Search By Course Name or Code"></asp:TextBox>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="course-table-container">
                        <h3 class="sub-heading">Courses in Selected Semester</h3>
                        <div class="course-table">
                            <asp:GridView ID="gvSemesterCourses" runat="server" CssClass="dynamic-table" AutoGenerateColumns="false" OnRowCommand="gvSemesterCourses_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Course Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCourseCode" runat="server" Text='<%# Eval("couse_code") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="School">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSchool" runat="server" Text='<%# Eval("short") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCourseName" runat="server" Text='<%# Eval("course_name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Credits">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCredits" runat="server" Text='<%# Eval("credits") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Manage Schedule" ItemStyle-CssClass="action-column">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkManageSchedule" runat="server" Text="Select" CommandName="SelectCourse" CssClass="action-btn"
                                                CommandArgument='<%# Eval("id") %>'>
                                                <i class="fa fa-calendar"></i> Select
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlScheduleEditor" runat="server" Visible="false">
                        <div class="schedule-container">
                            <div class="selected-course-info">
                                  <h3>
                                    <asp:Label ID="lblSelectedCourseCode" runat="server"></asp:Label>
                                    - 
                                    <asp:Label ID="lblSelectedCourseName" runat="server"></asp:Label>
                                </h3>
                                <p>
                                    School:
                                    <asp:Label ID="lblSelectedSchool" runat="server"></asp:Label>
                                    | 
                                    Credits:
                                    <asp:Label ID="lblSelectedCredits" runat="server"></asp:Label>
                                </p>
                            </div>

                            <h3 class="sub-heading">Course Schedule</h3>

                            <asp:Panel ID="pnlNoSchedule" runat="server" CssClass="no-schedule-message" Visible="false">
                                
                            </asp:Panel>

                            <div class="schedule-table">
                                <asp:GridView ID="gvSchedule" runat="server" CssClass="dynamic-table"
                                    AutoGenerateColumns="false" OnRowDataBound="gvSchedule_DataBound"
                                    DataKeyNames="id" OnRowDeleting="GvSchedule_Delete">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Weekday">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlWeekday" runat="server" CssClass="form-select-weekday" SelectedValue='<%# Eval("Weekday") %>'>
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
                                                <asp:TextBox ID="txtStartTime" runat="server" CssClass="form-timer"
                                                    Text='<%# Eval("StartTime") %>' TextMode="Time"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="End Time">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEndTime" runat="server" CssClass="form-timer"
                                                    Text='<%# Eval("EndTime") %>' TextMode="Time"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Room" ItemStyle-CssClass="room-column">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlRoomNumber" runat="server" CssClass="form-select-weekday"></asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Faculty" ItemStyle-CssClass="room-column">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlFacultyMember" runat="server" CssClass="form-select-weekday"></asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" ItemStyle-CssClass=" action-column-remove">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="RemoveSchedule" runat="server" Text="Remove" CommandName="Delete" CssClass="action-btn">
        <i class="fa fa-calendar"></i> Remove
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div class="no-schedule-message">
                                            No schedule entries found. Use the button below to add a schedule.
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>

                            <div class="btn-container">
                                <asp:Button ID="btnAddScheduleRow" runat="server" Text="Add Schedule Entry"
                                    OnClick="btnAddScheduleRow_Click" CssClass="action-button secondary-button" />
                                <asp:Button ID="btnSaveSchedule" runat="server" Text="Save All Changes"
                                    OnClick="btnSaveSchedule_Click" CssClass="action-button primary-button" />
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
