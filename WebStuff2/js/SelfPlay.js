let MinimumDuration;
let AverageDuration;
let MaximumDuration;
let MinimumMoveCount;
let AverageMoveCount;
let MaximumMoveCount;
let CheckmateCount;
let MaxGameID;
let Messages;

let prevGameID;

function body_onload() {

    MinimumDuration = document.getElementById("MinimumDuration");
    AverageDuration = document.getElementById("AverageDuration");
    MaximumDuration = document.getElementById("MaximumDuration");
    MinimumMoveCount = document.getElementById("MinimumMoveCount");
    AverageMoveCount = document.getElementById("AverageMoveCount");
    MaximumMoveCount = document.getElementById("MaximumMoveCount");
    CheckmateCount = document.getElementById("CheckmateCount");
    MaxGameID = document.getElementById("MaxGameID");
    Messages = document.getElementById("Messages");

    getStats();
    restartEngine();

    setInterval(getStats, 5000); // 5 seconds
    setInterval(checkEngine, 30000); // 30 seconds

}



function getStats() {

    var fnSuccess = function (result) {
        MinimumDuration.innerHTML = result.MinimumDuration;
        AverageDuration.innerHTML = result.AverageDuration;
        MaximumDuration.innerHTML = result.MaximumDuration;
        MinimumMoveCount.innerHTML = result.MinimumMoveCount;
        AverageMoveCount.innerHTML = result.AverageMoveCount;
        MaximumMoveCount.innerHTML = result.MaximumMoveCount;
        CheckmateCount.innerHTML = result.CheckmateCount
        MaxGameID.innerHTML = result.MaxGameID;
    };

    var fnError = function (result) {
        Messages.innerHTML = "<p> getStats Failure - " + result + "</p>" + Messages.innerHTML;
    };


    PageMethods.GetStats(fnSuccess, fnError);

}

function checkEngine() {
    if (prevGameID == MaxGameID.innerHTML) {
        restartEngine();
    }
    prevGameID = MaxGameID.innerHTML;
}

function restartEngine() {

    var fnSuccess = function (result) {
        Messages.innerHTML = "<p>" + result + "</p>" + Messages.innerHTML;
    };

    var fnError = function (result) {
        Messages.innerHTML = "<p> restartEngine Failure - " + result + "</p>" + Messages.innerHTML;
    };



    PageMethods.RestartEngine(fnSuccess, fnError);

}