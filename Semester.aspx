<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Semester.aspx.cs" Inherits="CourseRegestrationProject.WebForm8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .container {
            height: 100%;
            width: 100%;
            display: flex;
            justify-content: center;
            align-items: center;
            margin-top: 4rem;
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

        .form-timer {
            font-family: 'Inter';
            padding: 10px 12px;
            height: 2.6rem;
            border: 1px solid #ddd;
            border-radius: 6px;
            font-size: 16px;
            width: 100%;
            margin-top: 1rem;
        }

        .form-control {
            width: 100%;
            padding: 8px 12px;
            border-radius: 6px;
            font-size: 14px;
            max-width: 100%;
            min-width: 100%;
            border: 1px solid #ddd;
            height: 2.4rem;
            transition: border-color 0.3s ease, box-shadow 0.3s ease;
            margin-top: 1rem;
        }

            .form-control:focus {
                border-color: #4a90e2;
                box-shadow: 0 0 0 3px rgba(74, 144, 226, 0.3);
            }

            .form-control:focus {
                border-color: #007bff;
                outline: none;
            }
    </style>
    <div class="container">

        <div class="main-content">
            <label for="ddlCourse" class="form-label">Create Semester</label>
            <div class="labelContainer">

                <label class="form-label_box">Semester Name</label>
                <asp:TextBox ID="semesterName" runat="server" CssClass="form-control" placeholder="e.g., Spring(Jan To July 2025)" Text='<%# String.Format("{0:dd-MM-yyyy}") %>'></asp:TextBox>

            </div>

            <div class="labelContainer">

                <label class="form-label_box">Start Date</label>
                <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-timer"
                    TextMode="Date"></asp:TextBox>
            </div>
            <div class="labelContainer">

                <label class="form-label_box">End Date</label>
                <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-timer"
                    TextMode="Date"></asp:TextBox>
            </div>
            <div class="labelContainer">

                <label class="form-label_box">Registration Start Date</label>
                <asp:TextBox ID="TextBox1" runat="server" CssClass="form-timer"
                    TextMode="Date"></asp:TextBox>
            </div>
            <div class="labelContainer">

                <label class="form-label_box">Registration End Date</label>
                <asp:TextBox ID="TextBox2" runat="server" CssClass="form-timer"
                    TextMode="Date"></asp:TextBox>
            </div>


            <asp:RequiredFieldValidator ID="rfvSchool" runat="server" ControlToValidate="semesterName"
                ErrorMessage="Please Select Course" CssClass="text-danger" Display="Dynamic" InitialValue=""></asp:RequiredFieldValidator>

            <asp:Button ID="btnSave" runat="server" Text="Confirm" CssClass="btn-primary" OnClick="CreateSemester" />
        </div>
    </div>
</asp:Content>
