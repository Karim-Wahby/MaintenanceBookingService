var LoadingWheelManager = (function (loadingDiv) {
    var currentPendingJobs = 0;
    var loadingDivId = loadingDiv;
    
    start = function () {
        if (currentPendingJobs++ == 0) {
            $("#" + loadingDivId).css('display', 'block');
            $("#" + loadingDivId).html('<image src="../Content/LoadingWheel.gif" alt="Loading, please wait" />');
        }
    };

    end = function () {
        if (--currentPendingJobs == 0) {
            $("#" + loadingDivId).css('display', 'none');
            $("#" + loadingDivId).html('');
        }
    };

    forceEnd = function () {
        currentPendingJobs = 1;
        end();
        currentPendingJobs = 0;
    };

    return {
        start: start,
        end: end,
        forceEnd: forceEnd
    };
});