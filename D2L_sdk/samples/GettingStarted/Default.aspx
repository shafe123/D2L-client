<%@ Page Title="Home Page" Language="C#"  AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%
/**
 * Copyright (c) 2012 Desire2Learn Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the license at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 */

%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Desire2Learn Auth SDK Sample</title>
	<style type = "text/css">
		table.plain
		{
		  border-color: transparent;
		  border-collapse: collapse;
		}

		table td.plain
		{
		  padding: 5px;
		  border-color: transparent;
		}

		table th.plain
		{
		  padding: 6px 5px;
		  text-align: left;
		  border-color: transparent;
		}

		tr:hover
		{
			background-color: transparent !important;
		}
	</style>


</head>
<body>
<form id="configForm" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
        

	
        <table>
            <tr>
                <td>
                    <h4>Host: </h4></td>
                <td>
                    <asp:TextBox ID="hostField" runat="server"></asp:TextBox>
                    
                <td>
                    <h4>Port:</h4> </td>
                <td>
                    <asp:TextBox ID="portField" runat="server"></asp:TextBox>
            </tr>
            <tr>
                <td>
                    <h4>App ID:</h4></td>
                <td>
                    <asp:TextBox ID="appIDField" runat="server"></asp:TextBox>
                </td>
                <td>
                    <h4>App Key:</h4></td>
                <td>
                    <asp:TextBox ID="appKeyField" runat="server"></asp:TextBox>
                </td>
            </tr>
    		<tr>
                <td>
                    <h4>User ID:</h4></td>
                <td>
                    <asp:TextBox ID="userIDField" runat="server"></asp:TextBox>
                </td>

                <td>
                    <h4>User Key:</h4></td>
                <td>
                    <asp:TextBox ID="userKeyField" runat="server"></asp:TextBox></td>
            </tr>
        </table>
        <asp:Button ID="authenticateButton" OnClick="Authenticate" runat="server" Text="Authenticate" /> Note: to authenticate against the test server, you can user username "sampleapiuser" and password "Tisabiiif".
    <asp:Button ID="saveButton" runat="server" OnClick="SaveInfo" Text="Save" />
    <asp:Button ID="resetButton" runat="server" OnClick="ResetInfo" Text="Reset" />
    <hr />

    <table style="float:left;" class = "plain">
        <tr class = "plain">
            <td class = "plain">
                <asp:Button ID="getVersionsButton" runat="server" Text="Get Versions" OnClick="GetVersions" /><br /><br />
                <asp:Button ID="whoAmIButton" runat="server" Text="Who Am I" OnClick="WhoAmI" /><br /><br />
                <asp:Button ID="clearButton" runat="server" Text="Clear" OnClick="ClearResults" />
            </td>
            <td class = "plain">
                <span id = "resultHeading" runat="server" style = "clear:both;float:left;color: black;" ></span>
                <span id = "resultBox" runat="server" style = "clear:both;float:left;color: black;text-align:left" ></span>
            </td>
        </tr>
    </table>

    </form>
</body>
</html>
