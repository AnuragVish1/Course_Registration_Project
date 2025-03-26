<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Delete_Course.aspx.cs" Inherits="CourseRegestrationProject.WebForm4" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .main-content {
            display: flex;
            justify-content: center;
            align-items: center;
            flex-direction: column;
            font-family: sans-serif;
        }

        .form-select {
            margin-top: 2rem;
            border-radius: 8px;
            padding: 0.40rem;
            margin-bottom: 2rem;
            outline:none;
            border: 1px solid #a2a2a2;
            padding-top: 10px;
            padding-bottom: 10px;
        }

        .form-label {
            font-size: 2rem;
        }

        .btn-primary {
            margin-top: 1rem;
            background-color: #f74b4b;
            color: white;
            border: none;
            padding: 8px 16px;
            border-radius: 4px;
            cursor: pointer;
            font-weight: 600;
        }
        .text-danger{
            color: #ef2f2f
        }
    </style>
    <div style="height: 618px" class="main-content">
        <label for="ddlCourse" class="form-label required">Delete Course</label>
        <asp:DropDownList ID="ddlCourse" runat="server" CssClass="form-select">
            <asp:ListItem Value="" Text="-- Delete Course --" Selected="True"></asp:ListItem>
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="rfvSchool" runat="server" ControlToValidate="ddlCourse"
            ErrorMessage="Please Select Course" CssClass="text-danger" Display="Dynamic" InitialValue=""></asp:RequiredFieldValidator>

        <asp:Button ID="btnSave" runat="server" Text="Edit Course" CssClass="btn-primary" OnClick="DeleteCourseBtn" />
    </div>
</asp:Content>
