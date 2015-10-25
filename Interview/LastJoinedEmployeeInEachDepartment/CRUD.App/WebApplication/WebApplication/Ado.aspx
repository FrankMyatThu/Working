<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Ado.aspx.cs" Inherits="WebApplication.Ado" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>
                    <!-- Flat Form Area -->
                    <table>
                        <tr>
                            <td>Employee ID</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtEmployeeID"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Employee Name</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtEmployeeName"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Department ID</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtDepartmentID"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Department Name</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtDepartmentName"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Join Date</td>
                            <td>
                                <input type="date" runat="server" id="txtJoinDate"/>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>
                                <asp:Button ID="btnSubmit" runat="server" Text="Save" OnClick="btnSubmit_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <!-- Listing Area -->
                    <asp:GridView ID="grdEmployeeList" runat="server" AutoGenerateColumns="False" 
                        DataKeyNames="EmployeeID">
                        <Columns>
                            <asp:BoundField DataField="EmployeeID" HeaderText="EmployeeID" />
                            <asp:BoundField DataField="EmployeeName" HeaderText="EmployeeName" />
                            <asp:BoundField DataField="DepartmentID" HeaderText="DepartmentID" />
                            <asp:BoundField DataField="DepartmentName" HeaderText="DepartmentName" />
                            <asp:BoundField DataField="JoinDate" HeaderText="JoinDate" />

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%#Eval("EmployeeID")%>' OnCommand="btnEdit_Command">Edit</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<%#Eval("EmployeeID")%>' OnCommand="btnDelete_Command">Delete</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>


                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        
    </div>
    </form>
</body>
</html>
