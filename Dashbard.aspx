<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Dashbard.aspx.cs" Inherits="CourseRegestrationProject.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .main-content {
            display: flex;
            justify-content: center;
            align-items: center;
            font-family: sans-serif;
        }

        .course_lists {
            background-color: white;
            border: solid 1.8px #d8d8d8;
            height: 15rem;
            width: 28rem;
            padding: 1rem;
            border-radius: 10px;
            display: flex;
            flex-direction: column;
            justify-content: space-between;
            transition: all 0.18s ease;
            cursor: pointer;
        }

            .course_lists:hover {
                box-shadow: rgba(149, 157, 165, 0.2) 0px 8px 24px;
                transform: scale(1.02,1.02);
                transition: all 0.1s ease-out;
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

            .id_btn > button {
                width: 5rem;
                padding: 0.58rem;
                border-radius: 5px;
                outline: none;
                border: solid 1.5px #bdbdbd;
                background-color: #fff;
                cursor: pointer;
            }

        button:hover {
            background-color: #efefef
        }

        .container {
            padding: 2rem;
            padding-top: 2rem;
            height: 100%;
            width: 100%;
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(22rem, 1fr));
            gap: 2rem;
        }
    </style>
    <div class="container">
        <asp:Repeater ID="CourseRepeater" runat="server">
            <ItemTemplate>
                <div class="main-content">

                    <div class="course_lists">
                        <div class="id_btn">
                            <h1><%# Eval("CourseCode") %></h1>
                            <button>Edit</button>
                        </div>
                        <p style="color: #555555"><%# Eval("CourseName") %></p>
                        <div class="course_info" style="color: #616161">
                            <p>Credits: <%# Eval("Credits") %></p>
                            <p>School Name: <%# Eval("SchoolName") %></p>
                            
                        </div>

                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

</asp:Content>
