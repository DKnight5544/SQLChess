<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelfPlay.aspx.cs" Inherits="WebStuff2.SelfPlay" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SQLChess Self Play Stats Viewer</title>
    <link <%=cssBody%> rel="stylesheet" />
    <script <%=jsSrc %> type="text/javascript"></script>
</head>
<body onload ="body_onload();">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="TimerTechScriptManager" runat="server" EnablePageMethods="true"></asp:ScriptManager>

        <table>
            <tr>
                <td class ="TableHeader" colspan="2">Last 100 Games</td>
            </tr>
            <tr>
                <td class ="KeyColumn">Max Game ID</td>
                <td class ="ValueColumn" id="MaxGameID"></td>
            </tr>
            <tr>
                <td class ="KeyColumn">Min Seconds</td>
                <td class ="ValueColumn" id="MinimumDuration"></td>
            </tr>
            <tr>
                <td class ="KeyColumn">Avg Seconds</td>
                <td class ="ValueColumn" id="AverageDuration"></td>
            </tr>
            <tr>
                <td class ="KeyColumn">Max Seconds</td>
                <td class ="ValueColumn" id="MaximumDuration"></td>
            </tr>
            <tr>
                <td class ="KeyColumn">Min Move Count</td>
                <td class ="ValueColumn" id="MinimumMoveCount"></td>
            </tr>
            <tr>
                <td class ="KeyColumn">Avg Move Count</td>
                <td class ="ValueColumn" id="AverageMoveCount"></td>
            </tr>
            <tr>
                <td class ="KeyColumn">Max Move Count</td>
                <td class ="ValueColumn" id="MaximumMoveCount"></td>
            </tr>
            <tr>
                <td class ="KeyColumn">Checkmate Count</td>
                <td class ="ValueColumn" id="CheckmateCount"></td>
            </tr>
        </table>

        <div id="Messages"></div>

    </form>
</body>
</html>
