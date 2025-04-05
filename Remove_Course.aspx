<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Remove_Course.aspx.cs" Inherits="CourseRegestrationProject.WebForm4" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .container {
            height: 100vh;
            width: 100%;
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
            width: 32rem;
            border-radius: 8px;
            box-shadow: 0 1px 5px rgba(0, 0, 0, 0.1);
            padding: 2rem;
            border: 1px solid #e3dbdb;
        }

        .form-select {
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
            margin-top: 1rem;
        }

        .form-label {
            font-size: 2rem;
            font-weight: 600;
            text-align: center;
            width: 100%;
            padding-bottom: 1rem;
            border-bottom: 2px solid #eeeeee;
            margin-bottom: 1rem;
        }

        .btn-primary {
            margin-top: 2.5rem;
            background-color: #f74b4b;
            color: white;
            border: none;
            padding: 10px 16px;
            border-radius: 6px;
            cursor: pointer;
            font-weight: 600;
            font-size: medium;
            width: 100%;
            font-family: 'Inter';
        }

        .text-danger {
            color: #ef2f2f
        }

        .form-label_box {
            text-align: left;
            font-size: 18px;
            
        }

        .labelContainer {
            margin-top: 2rem;
            width: 100%;
            text-align: left;
        }
    </style>
    <div class="container">

        <div class="main-content">
            <label for="ddlCourse" class="form-label">Remove Course</label>
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

            <asp:Button ID="btnSave" runat="server" Text="Confirm" CssClass="btn-primary" OnClick="DeleteCourseBtn" />
        </div>
    </div>
</asp:Content>
