<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Add_Course.aspx.cs" Inherits="CourseRegestrationProject.WebForm6" %>

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

        .select-sem-container {
            display: flex;
            justify-content: center;
            align-items: center;
            gap: 1rem;
        }

        .course-table-container {
            padding-top: 2rem;
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

        .course-table {
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

            .dynamic-table .credits-column {
                width: 15%;
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
    </style>
    <div class="main-content">

        <div class="form-container">
            <div>
                <h1 class="page-title">Add Course To Semester</h1>
            </div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <div class="select-sem-container">

                        <asp:DropDownList ID="ddlSemester" runat="server" CssClass="form-select" OnSelectedIndexChanged="selectedSemCourses" AutoPostBack="true">
                            <asp:ListItem Value="" Text="Select Semester" Selected="True" class="options"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="inputField" OnTextChanged="liveSearching" AutoPostBack="true" placeholder="Search By Course Name or Code"></asp:TextBox>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="updatePanel2" runat="server">
                <ContentTemplate>
                    <div class="course-table-container">
                        <h3 class="sub-heading">Available Courses</h3>
                        <div class="course-table">
                            <asp:GridView ID="gvAvailableCourses" runat="server" CssClass="dynamic-table" AutoGenerateColumns="false" OnRowCommand="gvAvailableCourses_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Course Code">
                                        <ItemTemplate>
                                            <asp:Label ID="txtCourseCode" runat="server"
                                                Text='<%# Eval("couse_code") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="School">
                                        <ItemTemplate>
                                            <asp:Label ID="txtSchool" runat="server" Text='<%# Eval("short")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name">
                                        <ItemTemplate>
                                            <asp:Label ID="txtCourseName" runat="server"
                                                Text='<%# Eval("course_name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Credits" ItemStyle-CssClass="credits-column">
                                        <ItemTemplate>
                                            <asp:Label ID="txtCredits" runat="server"
                                                Text='<%# Eval("credits") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Add Course" ItemStyle-CssClass="action-column">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkAddCoursePlan" runat="server" Text="Add" CommandName="Add" CssClass="add-icon"
                                                CommandArgument='<%# Eval("couse_code") %>'
                                                OnClientClick="return confirm('Are you sure you want to Add Course to this semester?');"
                                                CausesValidation="false">
                                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
        <line x1="12" y1="5" x2="12" y2="19"></line>
        <line x1="5" y1="12" x2="19" y2="12"></line>
    </svg>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
