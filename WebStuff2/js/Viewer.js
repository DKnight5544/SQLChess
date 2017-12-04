
let PositionsTextBox;
let LastMoveButton;
let NextMoveButton;

const ChessBoard = new Object;

let moveIndex = 1;

function body_onload() {

    PositionsTextBox = document.getElementById("PositionsTextBox");
    LastMoveButton = document.getElementById("LastMoveButton");
    NextMoveButton = document.getElementById("NextMoveButton");

    for (var i = 1; i < 65; i++) {
        ChessBoard[i.toString()] = document.getElementById(i.toString());
    }

    PositionsTextBox.onchange = PositionsTextBox_onchange;

    LastMoveButton.onclick = lastMove;
    NextMoveButton.onclick = nextMove;

    PositionsTextBox.value = MoveObj[1];

    PositionsTextBox.onchange();


    //updateChessBoard(PositionsTextBox.value);
}

function lastMove() {
    moveIndex--;
    if (MoveObj[moveIndex.toString()]) {
        PositionsTextBox.value = MoveObj[moveIndex.toString()];
        PositionsTextBox.onchange();
    }
    else moveIndex++;
}

function nextMove() {
    moveIndex++;
    if (MoveObj[moveIndex.toString()]) {
        PositionsTextBox.value = MoveObj[moveIndex.toString()].trim();
        if (PositionsTextBox.value != "Checkmate" && PositionsTextBox.value != "Stalemate") PositionsTextBox.onchange();
    }
    else moveIndex--;
}

function PositionsTextBox_onchange(board) {
    if (PositionsTextBox.value.trim().length < 64) return false;
    for (var i = 1; i < 65; i++) {
        loadImage(i, PositionsTextBox.value);
    }
}

function updateChessBoard(board) {
    for (var i = 1; i < 65; i++) {
        loadImage(i, board);
    }
}

function loadImage(i, board) {
    const piece = board.substr(i - 1, 1);
    const td = ChessBoard[i.toString()]
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