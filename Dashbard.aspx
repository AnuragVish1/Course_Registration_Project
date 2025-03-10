<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Dashbard.aspx.cs" Inherits="CourseRegestrationProject.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        
        .main-content{
            display: flex;
            justify-content: center;
            align-items: center;
            font-family:sans-serif;
        }
        .course_lists{
            border: solid 1.8px #d8d8d8;
            height: 15rem;
            width: 28rem;
            padding: 1rem;
            border-radius: 10px;
            display:flex;
            flex-direction: column;
            justify-content:space-between;
            box-shadow: rgba(149, 157, 165, 0.2) 0px 8px 24px;

        
        }
        .course_info{
            padding-top:25px;
            border-top: solid 1px #b4b4b4;
            display:flex;
            gap: 20px;
            flex-direction: column
        }
        .id_btn{
            display:flex;
            justify-content: space-between;
            align-items: center;
        }
        .id_btn > button{
            width: 5rem;
            padding: 0.58rem;
            border-radius: 5px;
            outline:none;
            border: solid 1.5px #bdbdbd;
            background-color: #fff;
            cursor:pointer;
        }
        button:hover{
            background-color:#efefef
        }
    </style>
    <div style="height: 618px" class="main-content">
 
        <div class="course_lists">
            <div class="id_btn">
                <h1>CourseID</h1>
                <button>Edit</button>
            </div>
            <p style=" color: #414141">name</p>
            <div class="course_info" style=" color:#616161">
                <p>Credits:</p>
                <p>School Name:</p>
                <p>Faculty Name:</p>
            </div>

        </div>
    </div>
</asp:Content>
