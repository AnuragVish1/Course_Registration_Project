<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Delete_Course.aspx.cs" Inherits="CourseRegestrationProject.WebForm4" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .container {
            height: 100vh;
            width: 100vw;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .main-content {
            display: flex;
            justify-content: center;
            align-items: center;
            flex-direction: column;
            font-family: 'Inter';
            width: 25rem;
            border-radius: 8px;
            box-shadow: rgba(0, 0, 0, 0.16) 0px 1px 4px;
            padding: 2rem;
            border: 1px solid #f3f1f1;
        }

        .form-select {
            margin-top: 0.5rem;
            width: 100%;
            padding: 8px 12px;
            border: 1px solid #ddd;
            border-radius: 4px;
            font-size: 14px;
            background-color: white;
        }

        .form-label {
            font-size: 2rem;
            font-weight: 600;
            text-align:center;
            width: 100%;
            padding-bottom: 0.72rem;
            border-bottom: 2px solid #eeeeee;
        }

        .btn-primary {
            margin-top: 2rem;
            background-color: #f74b4b;
            color: white;
            border: none;
            padding: 10px 16px;
            border-radius: 4px;
            cursor: pointer;
            font-weight: 500;
            width: 100%;
        }

        .text-danger {
            color: #ef2f2f
        }

        .form-label_box {
            text-align: left;
            font-size: 15px;
        }

        .labelContainer {
            margin-top: 2rem;
            width: 100%;
            text-align: left;
        }
    </style>
    <div class="container">

        <div class="main-content">
            <label for="ddlCourse" class="form-label">Delete Course</label>
            <div class="labelContainer">

                <label for="ddlCourse" class="form-label_box">Course Name</label>
            </div>
            <asp:DropDownList ID="ddlCourse" runat="server" CssClass="form-select" OnSelectedIndexChanged="LoadSem" AutoPostBack="true">
                <asp:ListItem Value="" Text="-- Delete Course --" Selected="True" class="options"></asp:ListItem>
            </asp:DropDownList>
            <div class="labelContainer">

                <label for="ddlCourse" class="form-label_box">Course Semester</label>
            </div>
            <asp:DropDownList ID="ddlsemester" runat="server" CssClass="form-select">
                <asp:ListItem Value="" Text="-- Select Semester--" Selected="True" class="options"></asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvSchool" runat="server" ControlToValidate="ddlCourse"
                ErrorMessage="Please Select Course" CssClass="text-danger" Display="Dynamic" InitialValue=""></asp:RequiredFieldValidator>

            <asp:Button ID="btnSave" runat="server" Text="Delete Course" CssClass="btn-primary" OnClick="DeleteCourseBtn" />
        </div>
    </div>
</asp:Content>
