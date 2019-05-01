var ServiceDetailsViewManager = function (
    ServiceDetailsTemplateId,
    ServiceDetailsLoadingWheelId,
    ServiceDetailsContainerId)
{
    var apiManager = new APIManager();
    var serviceDetailsLoadingwheelmanager = new LoadingWheelManager(ServiceDetailsLoadingWheelId);
    var ServiceRequestUtilites = new ServiceRequestHelper();
    var GetAndDisplayServiceDetails = function (requestId) {
        serviceDetailsLoadingwheelmanager.start();
        var param = {};
        param['id'] = requestId;
        apiManager.get(
            "MaintenanceServicesRequests",
            "GetRequestWithId",
            param,
            function (requestDetails) {
                serviceDetailsLoadingwheelmanager.end();
                $("#" + ServiceDetailsContainerId).html($("#" + ServiceDetailsTemplateId)
                    .html()
                    .replace("__Id__", requestDetails.Id)
                    .replace("__ServiceNeeded__", ServiceRequestUtilites.MaintenanceServiceToHtml(requestDetails.MaintenanceServiceNeeded))
                    .replace("__ServiceDescription__", requestDetails.DescriptionOfRequiredService)
                    .replace("__DeliveryDate__", ServiceRequestUtilites.GetDateFromServiceDateTime(requestDetails.TimeOfServiceDelivery))
                    .replace("__DeliveryTime__", ServiceRequestUtilites.GetTimeFromServiceDateTime(requestDetails.TimeOfServiceDelivery))
                    .replace("__DeliveryLocation__", requestDetails.DeliveryLocation)
                    .replace("__ServiceStatus__", ServiceRequestUtilites.StatusToHtml(requestDetails.Status))
                    .replace("__Rating__", ServiceRequestUtilites.RatingToHtml(requestDetails.Rating))
                    .replace("__UserPreferredLanguage__", ServiceRequestUtilites.PreferredLanguageToHtml(requestDetails.UserPreferredLanguage)));
            });
    };

    return {
        GetAndDisplayServiceDetails: GetAndDisplayServiceDetails 
    };
};