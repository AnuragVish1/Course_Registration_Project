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

            .course_lists:hover .edit_a{
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
                text-decoration:none;
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
        .title_Link
        {
            text-decoration: none;
            color:#000;
            transition: all 0.3s ease;
            letter-spacing: 0.1rem;
        }
        .title_Link:hover{
            color:#f54141;
            transition: all 0.3s ease;
        }
        .inputContainer{
            padding: 2rem;
            width: 100%;
            display: flex;
            justify-content: center;
            align-content: center;
        }
        .inputField{
            width: 40%;
            height: 2.6rem;
            outline: none;
            border-radius: 6px;
            font-size: medium;
            padding: 1rem;
            border: 2px solid #e9e1e1;
            box-shadow: rgba(99, 99, 99, 0.2) 0px 2px 8px 0px;

        }


        
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <div class="inputContainer">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="inputField" placeholder="Search By..." OnTextChanged="liveSearching" AutoPostBack="true"></asp:TextBox>

        
    </div>
    <div class="container">
        
        <asp:Repeater ID="CourseRepeater" runat="server">
            <ItemTemplate>
                <div class="main-content">
                    
                    <div class="course_lists">
                        
                        <div class="id_btn">
                            <a class="title_Link" href="Overview.aspx?id=<%# Eval("CourseID") %>"><h1><%# Eval("CourseCode") %></h1></a>
                            <a  class="edit_a" href="Edit_course.aspx?id=<%# Eval("CourseID") %>">Edit</a>
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

