
const _chessBoard = new Object;

let _webID;
let _fromClick = -1;
let _thisMove;
let _color;

let _hideContainer;
let _chessBoardTable;

let _ind_YourMove;
let _ind_IllegalMove;
let _ind_Check;
let _ind_CheckmateWhite;
let _ind_CheckmateBlack;
let _ind_StalemateWhite;
let _ind_StalemateBlack;
let _ind_Stalemate50MoveRule;
let _ind_StalemateNotEnoughPieces;

const _indicatorON = "#000000";
const _indicatorOFF = "#DDDDDD";


function body_onload() {

    _hideContainer = document.getElementById("HideContainer");
    _chessBoardTable = document.getElementById("ChessBoardTable");

    _ind_YourMove = document.getElementById("Ind_YourMove");
    _ind_IllegalMove = document.getElementById("Ind_IllegalMove");
    _ind_Check = document.getElementById("Ind_Check");
    _ind_CheckmateWhite = document.getElementById("Ind_CheckmateWhite");
    _ind_CheckmateBlack = document.getElementById("Ind_CheckmateBlack");
    _ind_StalemateWhite = document.getElementById("Ind_StalemateWhite");
    _ind_StalemateBlack = document.getElementById("Ind_StalemateBlack");
    _ind_Stalemate50MoveRule = document.getElementById("Ind_Stalemate50MoveRule");
    _ind_StalemateNotEnoughPieces = document.getElementById("Ind_StalemateNotEnoughPieces");

    for (var i = 1; i < 65; i++) {
        _chessBoard[i.toString()] = document.getElementById(i.toString());
    }

    // todo:
    // 1. Implement castling, en passant, pawn promotion.

}

function updateChessBoard() {
    for (var i = 1; i < 65; i++) {
        const piece = _thisMove.substr(i - 1, 1);
        loadImage(i, piece);
    }
}

function loadImage(i, piece) {

    const td = _chessBoard[i.toString()]
    const ic = td.children[0];
    const img = ic.children[0];

    if (piece == "R") img.className = "white r";
    else if (piece == "N") img.className = "white n";
    else if (piece == "B") img.className = "white b";
    else if (piece == "K") img.className = "white k";
    else if (piece == "Q") img.className = "white q";
    else if (piece == "P") img.className = "white p";
    else if (piece == "r") img.className = "black r";
    else if (piece == "n") img.className = "black n";
    else if (piece == "b") img.className = "black b";
    else if (piece == "k") img.className = "black k";
    else if (piece == "q") img.className = "black q";
    else if (piece == "p") img.className = "black p";

    else img.className = "hide";
}

function clearIndicators() {
    _ind_YourMove.style.backgroundColor = _indicatorOFF;
    _ind_IllegalMove.style.backgroundColor = _indicatorOFF;
    _ind_Check.style.backgroundColor = _indicatorOFF;
    _ind_CheckmateWhite.style.backgroundColor = _indicatorOFF;
    _ind_CheckmateBlack.style.backgroundColor = _indicatorOFF;
    _ind_StalemateWhite.style.backgroundColor = _indicatorOFF;
    _ind_StalemateBlack.style.backgroundColor = _indicatorOFF;
    _ind_Stalemate50MoveRule.style.backgroundColor = _indicatorOFF;
    _ind_StalemateNotEnoughPieces.style.backgroundColor = _indicatorOFF;
}

function getNewGame(sender, color) {
    sender.parentElement.style.display = "none";
    _color = color;
    var xhr = new XMLHttpRequest();
    xhr.open('GET', 'api/NewGame' + color);
    xhr.onload = function () {
        if (xhr.status === 200) {
            let move = JSON.parse(xhr.responseText);
            _webID = move.WebID;
            _thisMove = move.ThisMove;
            updateChessBoard();
            clearIndicators();
            _ind_YourMove.style.backgroundColor = _indicatorON;
            _hideContainer.style.display = "block";
        }
        else {
            alert('Request failed.  Returned status of ' + xhr.status);
        }
    };
    xhr.send();

}

function pieceClicked(sender) {

    let id = sender.id;
    const pos = parseInt(id) - 1;

    if (_fromClick === pos) {
        const td = document.getElementById(id);
        td.removeAttribute("style");
        _fromClick = -1;
    }

    else if (_fromClick === -1) {
        const td = document.getElementById(id);
        td.style.backgroundColor = "lightblue";
        _fromClick = pos;
    }

    else if (_fromClick > -1) {
        let piece = _thisMove.substr(_fromClick, 1);
        _thisMove = setCharAt(_thisMove, _fromClick, "x");
        _thisMove = setCharAt(_thisMove, pos, piece);
        const td = document.getElementById((_fromClick + 1).toString());
        td.removeAttribute("style");
        updateChessBoard();
        _fromClick = -1;
        move();
    }

    //let debug = true;
}

function setCharAt(str, index, chr) {
    if (index > str.length - 1) return str;
    return str.substr(0, index) + chr + str.substr(index + 1);
}


function move() {

    let xhr = new XMLHttpRequest();
    let url = "api/Move/" + _webID + "|" + _thisMove;
    url = encodeURI(url);
    xhr.open('GET', url);
    xhr.onload = function () {
        if (xhr.status === 200) {
            let move = JSON.parse(xhr.responseText);
            let _webID = move.GameID;
            _thisMove = move.ThisMove;
            clearIndicators();

            if (move.IsCheckmate === true) {
                if (move.ThisMove.indexOf("Black") > -1) {
                    _ind_CheckmateWhite.style.backgroundColor = _indicatorON;
                }
                else {
                    _ind_CheckmateBlack.style.backgroundColor = _indicatorON;
                }
            }

            else if (move.IsLegalMove === false) {
                _ind_YourMove.style.backgroundColor = _indicatorON;
                _ind_IllegalMove.style.backgroundColor = _indicatorON;
                updateChessBoard();
            }

            else if (move.IsLegalMove === true) {
                _ind_YourMove.style.backgroundColor = _indicatorON;
                updateChessBoard();
            }



        }
        else {
            alert('Request failed.  Returned status of ' + xhr.status);
        }
    };
    xhr.send();

}



