var PendingApprovalViewManager = function (
    tableContainerIdDetails,
    tableApproveButtonTemplateId,
    ServiceDetailsTemplateId,
    ServiceDetailsLoadingWheelId,
    ServiceDetailsContainerId,)
{
    var apiManager = new APIManager();
    var serviceDetailsLoadingwheelmanager = LoadingWheelManager(ServiceDetailsLoadingWheelId);

    var pendingApprovalRequestsTable = new TableManager(
        tableContainerIdDetails,
        {
            order: [[2, "desc"], [3, "desc"]],
            createdRow: function (row, data, index) {
                row.id = GetRequestRowId(data.Id);
            },
            columns: [
                {
                    data: 'Id'
                },
                {
                    data: 'MaintenanceServiceNeeded',
                    render: function (neededService, unUsed, requestObject, tableObject) {
                        return MaintenanceServiceToHtml(neededService);
                    }
                },
                {
                    data: 'TimeOfServiceDelivery',
                    render: function (serviceDateTime, unUsed, requestObject, tableObject) {
                        return GetTimeFromServiceDateTime(serviceDateTime);
                    }
                },
                {
                    data: 'TimeOfServiceDelivery',
                    render: function (serviceDateTime, unUsed, requestObject, tableObject) {
                        return GetDateFromServiceDateTime(serviceDateTime);
                    }
                },
                {
                    data: 'Id',
                    render: function (requestId, unUsed, requestObject, tableObject) {
                        return $("#" + tableApproveButtonTemplateId)
                            .html()
                            .replace("__OnClickEventHandlerData__", "'" + requestId + "'")
                            .replace("__id__", GetRequestApproveButtonId(requestId));
                    }
                }
            ],
            columnDefs: [
                {
                    targets: 0,
                    width: '10%'
                },
                {
                    targets: 1,
                    width: '30%'
                },
                {
                    targets: 2,
                    width: '30%'
                },
                {
                    targets: 3,
                    width: '20%'
                },
                {
                    targets: 4,
                    width: '10%'
                }
            ]
        });

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

    var GetRequestApproveButtonId = function (requestId) {
        return 'Request_' + requestId + '_ApproveButton';
    };

    var GetRequestRowId = function (requestId) {
        return 'Request_' + requestId + '_Row';
    };

    var ApprovePendingRequest = function (requestId) {
        var param = {};
        param['id'] = requestId;
        apiManager.get(
            "MaintenanceServicesRequests",
            "ApproveRequest",
            param,
            function (success) {
                if (success) {
                    pendingApprovalRequestsTable.RemoveRow(GetRequestRowId(requestId));
                    alert("user request approved successfully!!");
                }
                else {
                    alert("failed to approve user request");
                }
            });
    };

    var GetPendingRequests = function () {
        pendingApprovalRequestsTable.Hide();
        pendingApprovalRequestsTable.StartLoadingWheel();
        apiManager.get(
            "MaintenanceServicesRequests",
            "PendingApproval",
            {},
            function (allPendingApprovalRequests) {
                pendingApprovalRequestsTable.Rerender(
                    allPendingApprovalRequests,
                    GetAndDisplayServiceDetails,
                    function (requestDetails) { return requestDetails.Id; });
                pendingApprovalRequestsTable.StopLoadingWheel();
                pendingApprovalRequestsTable.Show();
            });
    };

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
                    .replace("__ServiceNeeded__", MaintenanceServiceToHtml(requestDetails.MaintenanceServiceNeeded))
                    .replace("__ServiceDescription__", requestDetails.DescriptionOfRequiredService)
                    .replace("__DeliveryDate__", GetDateFromServiceDateTime(requestDetails.TimeOfServiceDelivery))
                    .replace("__DeliveryTime__", GetTimeFromServiceDateTime(requestDetails.TimeOfServiceDelivery))
                    .replace("__DeliveryLocation__", requestDetails.DeliveryLocation)
                    .replace("__ServiceStatus__", StatusToHtml(requestDetails.Status))
                    .replace("__Rating__", RatingToHtml(requestDetails.Rating))
                    .replace("__UserPreferredLanguage__", PreferredLanguageToHtml(requestDetails.UserPreferredLanguage)));
            });
    };

    return {
        ApproveRequest: ApprovePendingRequest,
        DisplayAllPendingRequests: GetPendingRequests
    };
};
