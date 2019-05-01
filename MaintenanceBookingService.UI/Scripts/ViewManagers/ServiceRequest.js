var ServiceRequestHelper = function () {
    var MaintenanceServiceToHtml = function (MaintenanceServiceId) {
        var MaintenanceServiceValues = [
            "Carpentry",
            "Electrical Maintenance",
            "Plumbing Service",
            "Air Conditioning Maintenance",
            "Cleaning",
            "Painting Service"
        ];

        if (MaintenanceServiceId < MaintenanceServiceValues.length) {
            return MaintenanceServiceValues[MaintenanceServiceId];
        }
        else {
            return "Unknown";
        }
    };

    var GetTimeFromServiceDateTime = function (serviceDateTime) {
        return new Date(serviceDateTime).toLocaleTimeString();
    };

    var GetDateFromServiceDateTime = function (serviceDateTime) {
        return new Date(serviceDateTime).toDateString();
    };

    var StatusToHtml = function (statusId) {
        var ServiceStatuValues = [
            "Not Specified",
            "Pending Approval",
            "Approved And Waiting Delivery",
            "Delivered"
        ];

        if (statusId < ServiceStatuValues.length) {
            return ServiceStatuValues[statusId];
        }
        else {
            return "Unknown";
        }
    };

    var PreferredLanguageToHtml = function (preferredLanguageId) {
        var SupportedLanguageValues = [
            "English",
            "Arabic"
        ];

        if (preferredLanguageId < SupportedLanguageValues.length) {
            return SupportedLanguageValues[preferredLanguageId];
        }
        else {
            return "Unknown";
        }
    };

    var RatingToHtml = function (ratingId) {
        var RatingValues = [
            "Poor",
            "Fair",
            "Good",
            "Very Good",
            "Excellent"
        ];

        if (ratingId != undefined && ratingId < RatingValues.length) {
            return RatingValues[ratingId];
        }
        else {
            return "Unknown";
        }
    };

    return {
        MaintenanceServiceToHtml: MaintenanceServiceToHtml,
        GetTimeFromServiceDateTime: GetTimeFromServiceDateTime,
        GetDateFromServiceDateTime: GetDateFromServiceDateTime,
        StatusToHtml: StatusToHtml,
        PreferredLanguageToHtml: PreferredLanguageToHtml,
        RatingToHtml: RatingToHtml
    };

};