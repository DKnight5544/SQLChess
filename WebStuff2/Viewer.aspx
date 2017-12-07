<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Viewer.aspx.cs" Inherits="WebStuff2.Viewer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Viewer One</title>
    <link rel="stylesheet" href="css/Header.css" />
    <link rel="stylesheet" href="css/Viewer.css" />
    <script src="js/Viewer.js"></script>
    <asp:Literal ID="litMoves" runat="server"></asp:Literal>
</head>

<body onload="body_onload();" onsubmit="return false;">

    <form id="form1" runat="server">
        
        <div id="basicHeader">
            SQLChess
        </div>

<%--        <table id="pageMenu">
            <tr>
                <td class="pageButton last">&nbsp;</td>
            </tr>
        </table>--%>

        <table id="ControlsTable">
            <tr>
                <td id="LastMoveButton"><</td>
                <td id="PositionsTextBoxContainer">
                    <input id="PositionsTextBox" type="text" maxlength="64" />
                </td>
                <td id="NextMoveButton">></td>
            </tr>
        </table>

        <table id="ChessBoardTable">
            <tr>
                <td id="57" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="58" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="59" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="60" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="61" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="62" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="63" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="64" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
            </tr>
            <tr>
                <td id="49" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="50" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="51" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="52" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="53" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="54" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="55" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="56" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
            </tr>
            <tr>
                <td id="41" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="42" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="43" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="44" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="45" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="46" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="47" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="48" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
            </tr>
            <tr>
                <td id="33" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="34" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="35" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="36" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="37" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="38" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="39" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="40" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
            </tr>
            <tr>
                <td id="25" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="26" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="27" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="28" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="29" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="30" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="31" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="32" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
            </tr>
            <tr>
                <td id="17" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="18" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="19" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="20" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="21" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="22" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="23" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="24" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
            </tr>
            <tr>
                <td id="9" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="10" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="11" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="12" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="13" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="14" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="15" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="16" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
            </tr>
            <tr>
                <td id="1" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="2" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="3" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="4" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="5" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="6" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="7" class="light"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
                <td id="8" class="dark"><div class="ic"><img src="images/Chess-Pieces.png"/></div></td>
            </tr>

        </table>

    </form>
</body>
</html>
