var APIManager = (function () {
    var apiPrefix = "api";

    var get = function (controller, action, parameters, successCallback, runAsync) {
        if (runAsync === undefined) {
            runAsync = true;
        }
        $.ajax({
            async: runAsync,
            type: 'GET',
            url: getUrl(controller, action) + "?" + $.param(parameters),
            success: function (data) {
                if (successCallback) {
                    successCallback(data);
                }
            },
            error: function (data) {
                if (apiFailureCallback) {
                    apiFailureCallback(data);
                }
            }
        });
    };

    var post = function (controller, action, bodyData, dataType, successCallback, runAsync) {
        if (runAsync == undefined) {
            runAsync = true;
        }
        $.ajax({
            async: runAsync,
            type: 'POST',
            url: getUrl(controller, action),
            data: bodyData,
            dataType: dataType,
            traditional: true,
            success: successCallback,
            error: apiFailureCallback
        });
    };

    var getUrl = function (controller, action) {
        return appRoot + apiPrefix + "/" + controller + "/" + action;
    };

    var apiFailureCallback = function (jqXHR) {
        console.error(jqXHR.responseText);
    };


    return {
        get: get,
        post: post,
        getUrl: getUrl
    };
});