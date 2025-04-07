<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Dashbard.aspx.cs" Inherits="CourseRegestrationProject.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .main-content {
            display: flex;
            justify-content: center;
            align-items: center;
            font-family: 'Inter';
        }

        .course_lists {
            background-color: white;
            height: 14.5rem;
            width: 28rem;
            padding: 1rem;
            border-radius: 10px;
            display: flex;
            flex-direction: column;
            justify-content: space-between;
            transition: all 0.18s ease;
            box-shadow: rgba(99, 99, 99, 0.2) 0px 2px 8px 0px;
            cursor: pointer;
        }

            .course_lists:hover {
                box-shadow: rgba(149, 157, 165, 0.2) 0px 8px 24px;
                transform: scale(1.02,1.02);
                transition: all 0.1s ease-out;
            }

                .course_lists:hover .edit_a {
                    opacity: 1;
                    transition: all 0.3s ease-out;
                }

        .course_info {
            padding-top: 52px;
            border-top: solid 1px #d3d3d3;
            display: flex;
            gap: 18px;
            flex-direction: column
        }

        .id_btn {
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

            .id_btn > .edit_a {
                width: 5rem;
                padding: 0.58rem;
                border-radius: 8px;
                outline: none;
                border: solid 2px #f4efef;
                background-color: #cf4b4b;
                cursor: pointer;
                text-align: center;
                text-decoration: none;
                color: white;
                opacity: 0;
            }

        .edit_a:hover {
            background-color: #b74242
        }

        .container {
            padding: 2rem;
            padding-top: 0.8rem;
            height: 100%;
            width: 100%;
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(22rem, 1fr));
            gap: 2rem;
        }

        .title_Link {
            text-decoration: none;
            color: #000;
            transition: all 0.3s ease;
            letter-spacing: 0.1rem;
        }

            .title_Link:hover {
                color: #f54141;
                transition: all 0.3s ease;
            }

        .inputContainer {
            padding: 2rem;
            width: 100%;
            display: flex;
            justify-content: center;
            align-content: center;
            position: relative;
        }

        .inputField {
            width: 40%;
            height: 2.6rem;
            outline: none;
            border-radius: 6px;
            font-size: medium;
            padding: 1rem;
            border: 1px solid #ddd;
            transition: border-color 0.3s ease, box-shadow 0.3s ease;
            padding-left: 38px;
            background-image: url("data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIyNCIgaGVpZ2h0PSIyNCIgdmlld0JveD0iMCAwIDI0IDI0IiBmaWxsPSJub25lIiBzdHJva2U9IiM3NTc1NzUiIHN0cm9rZS13aWR0aD0iMiIgc3Ryb2tlLWxpbmVjYXA9InJvdW5kIiBzdHJva2UtbGluZWpvaW49InJvdW5kIiBjbGFzcz0ibHVjaWRlIGx1Y2lkZS1zZWFyY2gtaWNvbiBsdWNpZGUtc2VhcmNoIj48Y2lyY2xlIGN4PSIxMSIgY3k9IjExIiByPSI4Ii8+PHBhdGggZD0ibTIxIDIxLTQuMy00LjMiLz48L3N2Zz4=");
            background-repeat: no-repeat;
            background-size: 18px 18px;
            background-position: 10px center;
            box-shadow: rgba(99, 99, 99, 0.1) 0px 2px 8px 0px;
        }

            .inputField:focus {
                border-color: #4a90e2;
                box-shadow: 0 0 0 3px rgba(74, 144, 226, 0.3);
            }

        .form-select {
            width: 20%;
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
            margin-left: 2.5rem;
            position: absolute;
            left: 0;
            box-shadow: rgba(99, 99, 99, 0.1) 0px 2px 8px 0px;
        }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="inputContainer">

                <asp:DropDownList ID="ddlSemester" runat="server" CssClass="form-select" OnSelectedIndexChanged="LoadeSemCourse" AutoPostBack="true">
                    <asp:ListItem Value="" Text="-- Select Semester --" Selected="True"></asp:ListItem>
                </asp:DropDownList>

                <asp:TextBox ID="txtSearch" runat="server" CssClass="inputField" placeholder="Search By Course Name or Code" OnTextChanged="liveSearching" AutoPostBack="true"></asp:TextBox>


            </div>
            <div class="container">

                <asp:Repeater ID="CourseRepeater" runat="server">
                    <ItemTemplate>
                        <div class="main-content">

                            <div class="course_lists">

                                <div class="id_btn">
                                    <a class="title_Link" href="Overview.aspx?id=<%# Eval("CourseID") %>">
                                        <h1><%# Eval("CourseCode") %></h1>
                                    </a>
                                    <a class="edit_a" href="Edit_course.aspx?id=<%# Eval("CourseID") %>">Edit</a>
                                </div>
                                <p style="color: #555555"><%# Eval("CourseName") %></p>
                                <div class="course_info" style="color: #616161">
                                    <p>Credits: <%# Eval("Credits") %></p>
                                    <p><%# Eval("SchoolName") %></p>

                                </div>

                            </div>

                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

